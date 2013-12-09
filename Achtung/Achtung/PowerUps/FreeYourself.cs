using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Achtung.PowerUps
{
    class FreeYourself : PowerUp
    {
        public FreeYourself(Vector2 position)
            : base(position)
        {
            Name = "FreeYourself";
            Type = PowerUpType.Yourself;
        }

        public override void Start(List<Snake> affected, TimeSpan gameTime)
        {
            if (!started)
            {
                started = true;
                this.affected = affected;
                this.startTime = gameTime;

                foreach (Snake s in affected)
                    s.FreeNodes = true;
            }
        }

        public override void Stop()
        {
            foreach (Snake s in affected)
                    s.FreeNodes = false;
        }
    }
}
