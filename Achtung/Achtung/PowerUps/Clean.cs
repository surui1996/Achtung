using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Achtung.PowerUps
{
    class Clean : PowerUp
    {
        public Clean(Vector2 position)
            : base(position)
        {
            Name = "Clean";
            Type = PowerUpType.All;
        }

        public override void Start(List<Snake> affected, TimeSpan gameTime)
        {
            if (!started)
            {
                started = true;
                this.affected = affected;
                this.startTime = gameTime;
                this.EffectTime = new TimeSpan(0, 0, 1);

                foreach (Snake s in affected)
                    s.Clean();
            }
        }

        public override void Stop()
        {
        }
    }
}
