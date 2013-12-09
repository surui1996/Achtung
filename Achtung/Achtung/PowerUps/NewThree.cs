using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Achtung.PowerUps
{
    class NewThree : PowerUp
    {
        public NewThree(Vector2 position)
            : base(position)
        {
            Name = "NewThree";
            Type = PowerUpType.All;
        }

        public override void Start(List<Snake> affected, TimeSpan gameTime)
        {
        }

        public override void Stop()
        {
        }
    }
}
