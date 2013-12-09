using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Achtung.PowerUps
{
    class AntiWall : PowerUp
    {
        public AntiWall(Vector2 position, PowerUpType type)
            : base(position)
        {
            Type = type;
            if (type == PowerUpType.Yourself)
                Name = "AntiWallYourself";
            else if (type == PowerUpType.All)
                Name = "AntiWallAll";
        }

        public override void Start(List<Snake> affected, TimeSpan gameTime)
        {
            if (!started)
            {
                started = true;
                this.affected = affected;
                this.startTime = gameTime;

                foreach (Snake s in affected)
                    s.NoWalls = true;
            }
        }

        public override void Stop()
        {
            foreach (Snake s in affected)
                s.NoWalls = false;
        }
    }
}
