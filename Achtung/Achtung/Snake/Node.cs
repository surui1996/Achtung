﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Achtung.PowerUps;
namespace Achtung
{
    class Node
    {
        public Node(Vector2 position, float angle, Texture2D texture, Rectangle sourceRec, float scale)
        {
            this.position = position;
            this.angle = angle;
            this.texture = texture;
            this.scale = scale;
            this.SourceRectangle = sourceRec;
        }

        public Rectangle SourceRectangle { get; set; }

        public Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        private float scale;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, SourceRectangle, Color.White,
                0.0f,
                new Vector2(texture.Width / 2, texture.Height / 2),
                scale, SpriteEffects.None, 0);
        }
        public void Draw(SpriteBatch spriteBatch, float scaleMultiplier)
        {
            spriteBatch.Draw(texture, position, SourceRectangle, Color.White,
                0.0f,
                new Vector2(texture.Width / 2, texture.Height / 2),
                scale * scaleMultiplier, SpriteEffects.None, 0);
        }

        public bool Intersects(Node other)
        {            
            Rectangle a = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * scale), (int)(texture.Height * scale));
            Rectangle b = new Rectangle((int)other.Position.X, (int)other.Position.Y, (int)(other.Texture.Width * scale), (int)(other.Texture.Height * scale));
            
            return a.Intersects(b);
        }

        public bool Intersects(PowerUp powerup)
        {
            
            Rectangle a = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * scale), (int)(texture.Height * scale));
            Rectangle b = new Rectangle((int)powerup.Position.X, (int)powerup.Position.Y, PowerUpsManager.POWERUP_WIDTH, PowerUpsManager.POWERUP_HEIGHT);

            return a.Intersects(b);
        }

        public bool IsOutOfBounds(int screenWidth, int screenHeight)
        {
            int border = AchtungGame.BORDER_PX;
            return position.X < border || position.X > screenWidth - border
                || position.Y < border || position.Y > screenHeight - border;
        }
    }
}
