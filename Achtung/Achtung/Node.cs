using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Achtung
{
    class Node
    {
        public Node(Vector2 position, float angle, Texture2D texture, float scale)
        {
            this.position = position;
            this.angle = angle;
            this.texture = texture;
            this.scale = scale;
        }

        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
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
            spriteBatch.Draw(texture, position, null, Color.CornflowerBlue, angle,
                    new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0);
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
            Rectangle b = new Rectangle((int)powerup.Position.X, (int)powerup.Position.Y, (int)(powerup.Texture.Width), (int)(powerup.Texture.Height));

            //if ((b.Left < a.Right && b.Left > a.Left) && )
            //    return true;
            //if(
            return a.Intersects(b);
        }

        public bool IsOutOfBounds(int screenWidth, int screenHeight)
        {
            return position.X < 0 || position.X > screenWidth
                || position.Y < 0 || position.Y > screenHeight;
        }
    }
}
