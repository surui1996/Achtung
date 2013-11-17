using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Achtung
{
    public enum PowerUpType { You, Others, All }

    abstract class PowerUp
    {
        
        protected PowerUp(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }

        public abstract void Start(Snake taker, TimeSpan gameTime);
        public abstract void Stop();

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

        protected Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        protected Snake taker;
    }
}
