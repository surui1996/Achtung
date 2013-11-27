using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Achtung
{
    class Square : PowerUp
    {
        public Square(Vector2 position, PowerUpType type)
            : base(position)
        {
            Type = type;
            if (type == PowerUpType.Yourself)
                Name = "SquareYourself";
            else if (type == PowerUpType.Others)
                Name = "SquareOthers";
        }

        public override void Start(List<Snake> affected, TimeSpan gameTime)
        {
            if (!started)
            {
                started = true;
                this.affected = affected;
                this.startTime = gameTime;

                foreach (Snake s in affected)
                    s.Square = true;
            }
        }

        public override void Stop()
        {
            foreach (Snake s in affected)
                    s.Square = false;
        }
    }
}
