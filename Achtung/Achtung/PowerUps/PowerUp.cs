﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Achtung.PowerUps
{
    public enum PowerUpType { Yourself, Others, All }

    abstract class PowerUp
    {
        public TimeSpan EffectTime { get; set; }
        public PowerUpType Type { get; set; }
        protected bool started;
        protected List<Snake> affected;
        

        protected PowerUp(Vector2 position)
        {
            this.position = position;
            this.EffectTime = new TimeSpan(0, 0, 3);
            started = false;
        }

        public abstract void Start(List<Snake> affected, TimeSpan gameTime);
        public abstract void Stop();

        public bool Intersects(PowerUp other)
        {
            Rectangle a = new Rectangle((int)position.X, (int)position.Y, PowerUpsManager.POWERUP_WIDTH, PowerUpsManager.POWERUP_HEIGHT);
            Rectangle b = new Rectangle((int)other.Position.X, (int)other.Position.Y, PowerUpsManager.POWERUP_WIDTH, PowerUpsManager.POWERUP_HEIGHT);

            return a.Intersects(b);
        }

        protected Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected TimeSpan startTime;
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public string Name { get; set; }
    }
}
