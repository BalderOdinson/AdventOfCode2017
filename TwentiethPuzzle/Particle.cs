using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentiethPuzzle
{
    public struct Particle
    {
        public Particle(int id, Point position, Point velocity, Point acceleration)
        {
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
            Id = id;
        }

        public int Id { get; }
        public Point Position { get; }
        public Point Velocity { get; }
        public Point Acceleration { get; }

        public Particle Move(int time)
        {
            var positionX = Position.X;
            var positionY = Position.Y;
            var positionZ = Position.Z;
            var velocityX = Velocity.X;
            var velocityY = Velocity.Y;
            var velocityZ = Velocity.Z;
            var accelerationX = Acceleration.X;
            var accelerationY = Acceleration.Y;
            var accelerationZ = Acceleration.Z;
            var id = Id;
            for (int i = 0; i < time; i++)
            {
                velocityX += accelerationX;
                velocityY += accelerationY;
                velocityZ += accelerationZ;
                positionX += velocityX;
                positionY += velocityY;
                positionZ += velocityZ;
            }
            return new Particle(id, new Point(positionX, positionY, positionZ), new Point(velocityX, velocityY, velocityZ), new Point(accelerationX, accelerationY, accelerationZ));
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (!(obj is Particle particle))
                return false;
            return particle.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
