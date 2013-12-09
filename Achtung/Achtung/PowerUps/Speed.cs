using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Achtung.PowerUps
{
    class Speed : PowerUp
    {
        private const float SPEED = 1.3f;


        public Speed(Vector2 position, PowerUpType type)
            : base(position)
        {
            this.Type = type;
            if (type == PowerUpType.Yourself)
                Name = "SpeedYourself";
            else if (type == PowerUpType.Others)
                Name = "SpeedOthers";
        }

        public override void Start(List<Snake> affected, TimeSpan gameTime)
        {
            if (!started)
            {
                started = true;
                this.startTime = gameTime;
                this.affected = affected;

                foreach (Snake s in affected)             
                    s.UpdateVelocity(SPEED);                                        
            }
        }

        public override void Stop()
        {
            foreach (Snake s in this.affected)
                s.UpdateVelocity(1 / SPEED);
        }
    }
}
