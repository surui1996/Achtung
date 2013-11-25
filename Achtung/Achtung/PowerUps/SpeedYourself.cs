using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Achtung
{
    class SpeedYourself : PowerUp
    {
        private const float SPEED = 1.3f;

        public SpeedYourself(Vector2 position)
            : base(position)
        {
            Name = "SpeedYourself";
        }

        public override void Start(Snake taker, TimeSpan gameTime)
        {
            if (!started)
            {
                this.startTime = gameTime;
                taker.UpdateVelocity(SPEED);
                this.taker = taker;
                started = true;
            }
        }

        public override void Stop()
        {
            taker.UpdateVelocity(1 / SPEED);
        }
    }
}
