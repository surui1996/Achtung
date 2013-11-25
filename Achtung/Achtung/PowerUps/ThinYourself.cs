using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Achtung
{
    class ThinYourself : PowerUp
    {
        private const float THIN = 0.5f;

        public ThinYourself(Vector2 position)
            : base(position)
        {
            Name = "ThinYourself";
        }

        public override void Start(Snake taker, TimeSpan gameTime)
        {
            if (!started)
            {
                this.startTime = gameTime;
                taker.UpdateScale(THIN);
                this.taker = taker;
                started = true;
            }
        }

        public override void Stop()
        {
            taker.UpdateScale(1 / THIN);
        }
    }
}
