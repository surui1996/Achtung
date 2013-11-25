using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Achtung
{
    class SlowYourself : PowerUp
    {
        private const float SLOW = 0.5f;        

        public SlowYourself(Vector2 position) : base(position)
        {
            Name = "SlowYourself";
        }

        public override void Start(Snake taker, TimeSpan gameTime)
        {
            if (!started)
            {
                this.startTime = gameTime;
                taker.UpdateVelocity(SLOW);
                this.taker = taker;
                started = true;
            }
        }



        public override void Stop()
        {
            taker.UpdateVelocity(1 / SLOW);
        }
    }
}
