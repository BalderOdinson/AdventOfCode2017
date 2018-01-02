using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentiethPuzzle
{
    public struct Collision
    {
        public Collision(Particle firstParticle, Particle secondParticle, int timeOfCollision)
        {
            FirstParticle = firstParticle;
            SecondParticle = secondParticle;
            TimeOfCollision = timeOfCollision;
        }

        public Particle FirstParticle { get; }
        public Particle SecondParticle { get; }
        public int TimeOfCollision { get; }
    }
}
