using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Achtung.PowerUps
{
    class Resize : PowerUp
    {
        private const float THIN = 0.5f;
        private const float THICK = 2.0f;

        private float factor;
        public Resize(Vector2 position, PowerUpType type)
            : base(position)
        {
            this.Type = type;
            if (type == PowerUpType.Yourself)
            {
                Name = "ThinYourself";
                factor = THIN;
            }
            else if (type == PowerUpType.Others)
            {
                Name = "ThickOthers";
                factor = THICK;
            }
       }

        public override void Start(List<Snake> affected, TimeSpan gameTime)
        {
            this.startTime = gameTime;
            this.affected = affected;
            if (!started)
            {                
                started = true;
                foreach (Snake s in affected)
                    s.UpdateScale(factor); 
            }
        }

        public override void Stop()
        {
            foreach (Snake s in affected)
                s.UpdateScale(1 / factor);
        }
    }
}
