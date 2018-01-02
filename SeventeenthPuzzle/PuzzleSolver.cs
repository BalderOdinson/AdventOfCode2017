using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeventeenthPuzzle
{
    static class Extensions
    {
        public static LinkedListNode<T> AddAtPositon<T>(this LinkedList<T> list, T element, int position)
        {
            if(position < 0 || position >= list.Count)
                throw new IndexOutOfRangeException();
            if (position > list.Count)
            {
                LinkedListNode<T> node = list.Last; 
                for (int i = list.Count - 1; i != position; i--)
                {
                    node = node.Previous;
                }

                list.AddAfter(node, new LinkedListNode<T>(element));
                return node.Next;
            }
            else
            {
                LinkedListNode<T> node = list.First;
                for (int i = 0; i != position; i++)
                {
                    node = node.Next;
                }

                list.AddAfter(node, new LinkedListNode<T>(element));
                return node.Next;
            }
        }
    }

    public class PuzzleSolver
    {
        private readonly int _numOfSteps;
        private const int Iterations = 50000000;

        public PuzzleSolver(string input)
        {
            _numOfSteps = Convert.ToInt32(input);
        }

        public int SolveFirst()
        {
            var buffer = new LinkedList<int>();
            var tempNode = new LinkedListNode<int>(0);
            var currentPosition = 0;
            buffer.AddFirst(tempNode);
            for (int i = 1; i < 2018; i++)
            {
                currentPosition += _numOfSteps;
                currentPosition %= buffer.Count;
                tempNode = buffer.AddAtPositon(i, currentPosition);
                currentPosition++;
            }

            return tempNode.Next.Value;
        }

        public int SolveSecond()
        {
            var currentPosition = 0;
            var count = 1;
            var elementAfterZero = 0;
            for (int i = 1; i <= Iterations; i++)
            {
                currentPosition += _numOfSteps;
                currentPosition %= count;
                //Add
                if (currentPosition == 0)
                {
                    elementAfterZero = i;
                }
                count++;
                //EndAdd
                currentPosition++;
            }

            return elementAfterZero;
        }
    }
}
