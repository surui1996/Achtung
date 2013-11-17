using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Achtung
{
    enum Direction { Left, Right, None };

    class Snake
    {
        private const float ANGLE_INCREASE = 0.05f;
        private const int PIXEL_MARGIN = 200;
        private const float DEFAULT_VELOCITY = 2.5f;
        private const float DEFAULT_SCALE = 0.1f;
        private const int NODES_COUNT = 8;
        private const string LOST = "You Lost!!!";

        private bool collided = false;
        private float velocity;
        
        private List<Node> nodes;
        Texture2D head2D, node2D;
        SpriteFont font; 

        private int screenWidth, screenHeight;
        private int hole = -1;

        private Node head;
        public Node Head 
        {
            get { return head; }
            set { head = value; }
        }

        public Snake(Texture2D head, Texture2D node, SpriteFont font, int screenWidth, int screenHeight)
        {
            this.head2D = head;
            this.node2D = node;
            this.font = font;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            RandomHead();
            //this.head = new Node(RandomPosition(screenWidth, screenHeight), 0.0f, head2D, DEFAULT_SCALE);
            this.velocity = DEFAULT_VELOCITY;
            this.nodes = new List<Node>();            
        }

        public void Move(Direction d)
        {
            if (!collided)
            {                
                if (hole < -5)
                    RandomHole();
                if(hole < 0)
                   nodes.Add(new Node(head.Position, head.Angle, node2D, DEFAULT_SCALE));
                hole--;
                
                switch (d)
                {
                    case Direction.Right: head.Angle += ANGLE_INCREASE;
                        break;
                    case Direction.Left: head.Angle -= ANGLE_INCREASE;
                        break;
                }
                head.Position += new Vector2((float)Math.Cos(head.Angle), (float)Math.Sin(head.Angle)) * velocity;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!collided)
                collided = HasCollided();
            
            head.Draw(spriteBatch);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw(spriteBatch);
            }
            
            if(collided)
                spriteBatch.DrawString(font, LOST, 
                    new Vector2((screenWidth - font.MeasureString(LOST).X) / 2,
                        (screenHeight - font.MeasureString(LOST).Y) / 2), Color.Black);
            
        }

        public void UpdateVelocity(float multiplier)
        {
            this.velocity = velocity * multiplier;
        }

        private bool HasCollided()
        {
            if (head.IsOutOfBounds(screenWidth, screenHeight))
                return true;

            if (nodes.Count > NODES_COUNT - 1)
            {
                for (int i = nodes.Count - NODES_COUNT; i >= 0; i--)
                    if (nodes[i].Intersects(head))
                        return true;
            }
            

            return false;
        }

        private void RandomHole()
        {
            Random rnd = new Random();
            if ((int)rnd.Next(65) == 15)
                hole = rnd.Next(8, 15);
            else
                hole = -6;
        }

        private void RandomHead()
        {
            Vector2 pos = new Vector2();
            Random rnd = new Random();
            pos.X = rnd.Next(PIXEL_MARGIN, screenWidth - PIXEL_MARGIN);
            pos.Y = rnd.Next(PIXEL_MARGIN, screenHeight - PIXEL_MARGIN);

            this.head = new Node(pos, rnd.Next(0, 360), head2D, DEFAULT_SCALE);
        }

    }
}
