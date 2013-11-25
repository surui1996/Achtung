using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Achtung
{
    class AntiWallYourself : PowerUp
    {
        public AntiWallYourself(Vector2 position)
            : base(position)
        {
            Name = "AntiWallYourself";
        }

        public override void Start(Snake taker, TimeSpan gameTime)
        {
            if (!started)
            {
                this.startTime = gameTime;
                taker.NoWalls = true;
                this.taker = taker;
                started = true;
            }
        }

        public override void Stop()
        {
            taker.NoWalls = false;
        }
    }
}
