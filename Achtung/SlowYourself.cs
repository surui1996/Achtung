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

        public SlowYourself(Vector2 position, Texture2D texture) : base(position, texture)
        {
            
        }

        public override void Start(Snake taker, TimeSpan gameTime)
        {
            this.startTime = gameTime;
            taker.UpdateVelocity(SLOW);
            this.taker = taker;
        }

        public override void Stop()
        {
            taker.UpdateVelocity(1 / SLOW);
        }
    }
}
