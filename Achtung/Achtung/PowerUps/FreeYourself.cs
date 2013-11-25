using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Achtung
{
    class FreeYourself : PowerUp
    {
        public FreeYourself(Vector2 position)
            : base(position)
        {
            Name = "FreeYourself";
        }

        public override void Start(Snake taker, TimeSpan gameTime)
        {
            if (!started)
            {
                this.startTime = gameTime;
                taker.FreeNodes = true;
                this.taker = taker;
                started = true;
            }
        }

        public override void Stop()
        {
            taker.FreeNodes = false;
        }
    }
}
