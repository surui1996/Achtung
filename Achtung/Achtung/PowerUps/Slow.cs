using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Achtung
{
    class Slow : PowerUp
    {
        private const float SLOW = 0.5f;        

        public Slow(Vector2 position, PowerUpType type) : base(position)
        {
            Type = type;
            if (type == PowerUpType.Yourself)
                Name = "SlowYourself";
            else if (type == PowerUpType.Others)
                Name = "SlowOthers";
        }

        public override void Start(List<Snake> affected, TimeSpan gameTime)
        {
            if (!started)
            {
                started = true;
                this.affected = affected;                
                this.startTime = gameTime;

                foreach (Snake s in affected)                    
                    s.UpdateVelocity(SLOW);               
            }
        }



        public override void Stop()
        {
            foreach (Snake s in affected)
                s.UpdateVelocity(1 / SLOW);           
        }
    }
}
