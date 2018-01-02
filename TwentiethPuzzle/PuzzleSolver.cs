using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwentiethPuzzle
{
    public class PuzzleSolver
    {
        private readonly IEnumerable<Point> _pTransitions;
        private readonly IEnumerable<Point> _vTransitions;
        private readonly IEnumerable<Point> _aTransitions;
        private readonly IEnumerable<Particle> _particles;

        public PuzzleSolver(string transitions)
        {
            _pTransitions = Regex.Matches(transitions, "((?<=(p\\=\\<))(.*?)(?=\\>))+").OfType<Match>().Select(m =>
                {
                    var temp = Regex.Split(m.Value, ",");
                    return new Point(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]));
                })
                .ToList();
            _vTransitions = Regex.Matches(transitions, "((?<=(v\\=\\<))(.*?)(?=\\>))+").OfType<Match>().Select(m =>
                {
                    var temp = Regex.Split(m.Value, ",");
                    return new Point(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]));
                })
                .ToList();
            _aTransitions = Regex.Matches(transitions, "((?<=(a\\=\\<))(.*?)(?=\\>))+").OfType<Match>().Select(m =>
                {
                    var temp = Regex.Split(m.Value, ",");
                    return new Point(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]), Convert.ToInt32(temp[2]));
                })
                .ToList();
            var pList = (List<Point>)_pTransitions;
            var vList = (List<Point>)_vTransitions;
            var aList = (List<Point>)_aTransitions;
            var count = pList.Count;
            _particles = new List<Particle>();
            var particles = (List<Particle>)_particles;
            for (int i = 0; i < count; i++)
            {
                particles.Add(new Particle(i,
                    pList[i], vList[i], aList[i]));
            }
        }

        public int SolveFirst()
        {
            var nullPoint = new Point(0, 0, 0);
            var closestParticle = 0;
            var smallestManhattan = Point.ManhattanDistance(nullPoint, _aTransitions.FirstOrDefault());
            var counter = 0;
            foreach (var aTransition in _aTransitions)
            {
                var manhattanDIstance = Point.ManhattanDistance(nullPoint, aTransition);
                if (manhattanDIstance < smallestManhattan)
                {
                    smallestManhattan = manhattanDIstance;
                    closestParticle = counter;
                }

                counter++;
            }

            return closestParticle;
        }

        public int SolveSecond()
        {
            var collisionQueue = new ConcurrentQueue<Collision>();
            var particles = _particles.ToList();
            var count = particles.Count;

            Parallel.For(0, count, new ParallelOptions { MaxDegreeOfParallelism = 10 }, i =>
              {
                  for (int j = i + 1; j < count; j++)
                  {
                      var result = WillCollide(particles[i], particles[j]);
                      if (!result.HasValue) continue;
                      collisionQueue.Enqueue(new Collision(particles[i], particles[j], result.Value));
                  }
              });

            var list = collisionQueue.OrderBy(collision => collision.TimeOfCollision).ToList();
            var deletedDictionary = new Dictionary<Particle, int>();
            foreach (var collision in list)
            {
                if (deletedDictionary.ContainsKey(collision.FirstParticle) && deletedDictionary[collision.FirstParticle] != collision.TimeOfCollision ||
                   deletedDictionary.ContainsKey(collision.SecondParticle) && deletedDictionary[collision.SecondParticle] != collision.TimeOfCollision) continue;
                if (!deletedDictionary.ContainsKey(collision.FirstParticle))
                    deletedDictionary.Add(collision.FirstParticle, collision.TimeOfCollision);
                if (!deletedDictionary.ContainsKey(collision.SecondParticle))
                    deletedDictionary.Add(collision.SecondParticle, collision.TimeOfCollision);
            }

            return count - deletedDictionary.Count;
        }

        private static int? WillCollide(Particle first, Particle second)
        {
            var ax = (first.Acceleration.X - second.Acceleration.X) / 2.0;
            var bx = first.Velocity.X - second.Velocity.X + ax;
            var cx = first.Position.X - second.Position.X;
            List<int> xResults = null;
            if (!(ax == 0 && bx == 0 && cx == 0))
            {
                xResults = SolveQuadraticEquation(ax, bx, cx).ToList();
            }
            if (xResults != null && !xResults.Any()) return null;
            var ay = (first.Acceleration.Y - second.Acceleration.Y) / 2.0;
            var by = first.Velocity.Y - second.Velocity.Y + ay;
            var cy = first.Position.Y - second.Position.Y;
            List<int> yResults = null;
            if (!(ay == 0 && by == 0 && cy == 0))
            {
                yResults = SolveQuadraticEquation(ay, by, cy).ToList();
            }
            if (yResults != null && !yResults.Any()) return null;
            var az = (first.Acceleration.Z - second.Acceleration.Z) / 2.0;
            var bz = first.Velocity.Z - second.Velocity.Z + az;
            var cz = first.Position.Z - second.Position.Z;
            List<int> zResults = null;
            if (!(az == 0 && bz == 0 && cz == 0))
            {
                zResults = SolveQuadraticEquation(az, bz, cz).ToList();
            }
            if (zResults != null && !zResults.Any()) return null;

            var intersect = xResults;
            if (yResults != null)
            {
                intersect = intersect?.Intersect(yResults).ToList() ?? yResults;
            }

            if (zResults != null)
            {
                intersect = intersect?.Intersect(zResults).ToList() ?? zResults;
            }

            if (intersect == null || !intersect.Any()) return null;
            return intersect.Min();
        }

        private static IEnumerable<int> SolveQuadraticEquation(double a, double b, double c)
        {
            if (a == 0)
            {
                var result = -c / b;
                if (Math.Abs(result - Math.Round(result)) < 0.01 && result >= 0)
                    yield return (int)Math.Round(result);
                yield break;
            }
            var discriminant = Math.Pow(b, 2) - 4.0 * a * c;
            if (discriminant < 0)
                yield break;
            var firstResult = (-b + Math.Sqrt(discriminant)) / (2.0 * a);
            if (Math.Abs(firstResult - Math.Round(firstResult)) < 0.01 && firstResult >= 0)
                yield return (int)Math.Round(firstResult);
            var secondResult = (-b - Math.Sqrt(discriminant)) / (2.0 * a);
            if (Math.Abs(secondResult - Math.Round(secondResult)) < 0.01 && secondResult >= 0)
                yield return (int)Math.Round(secondResult);
        }
    }
}
