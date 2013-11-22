using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Achtung
{
    class SquareYourself : PowerUp
    {
        public SquareYourself(Vector2 position)
            : base(position)
        {
            Name = "SquareYourself";
        }

        public override void Start(Snake taker, TimeSpan gameTime)
        {
            if (!started)
            {
                this.startTime = gameTime;
                taker.Square = true;
                this.taker = taker;
                started = true;
            }
        }

        public override void Stop()
        {
            taker.Square = false;
        }
    }
}
