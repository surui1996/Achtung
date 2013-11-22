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
        private const float ANGLE_INCREASE = 3.5f;
        private const int PIXEL_MARGIN = 200;
        private const float DEFAULT_VELOCITY = 2.5f;
        private const float DEFAULT_SCALE = 0.05f;

        public float Scale { get; set; }

        private int nodesCount;
        private const string LOST = "You Lost!!!";

        private bool collided = false;
        public bool Collided
        {
            get { return collided; }
            set { collided = value; }
        }
        private float velocity;

        public bool Square { get; set; }
        public bool NoWalls { get; set; }
        public bool FreeNodes { get; set; }
        private bool changedDirection;

        private List<Node> nodes;
        Texture2D head2D, node2D;
        SpriteFont font;
        private Random rnd;

        private int screenWidth, screenHeight;
        private int hole = -1;
        private int noWallsCounter = 0;

        private Node head;
        public Node Head
        {
            get
            {
                return head;
            }
            set { head = value; }
        }

        public Snake(Texture2D head, Texture2D node, SpriteFont font, int screenWidth, int screenHeight)
        {
            this.head2D = head;
            this.node2D = node;
            this.font = font;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.Scale = DEFAULT_SCALE;
            this.Square = false;
            this.NoWalls = false;
            this.FreeNodes = false;
            this.changedDirection = false;
            this.rnd = new Random();

            RandomHead();
            this.velocity = DEFAULT_VELOCITY;
            this.nodes = new List<Node>();
            this.nodesCount = (int)(1000 * DEFAULT_SCALE);
        }

        public void Move(Direction d)
        {
            
            if(collided)
                return;
            if (Square)
            {
                
                nodes.Add(new Node(head.Position, head.Angle, node2D, Scale));
                switch (d)
                {
                    case Direction.Right: if (!changedDirection) head.Angle += 90.0f; changedDirection = true;
                        break;
                    case Direction.Left: if (!changedDirection) head.Angle -= 90.0f; changedDirection = true;
                        break;
                    case Direction.None: changedDirection = false;
                        break;
                }

                head.Position += new Vector2((float)Math.Cos(MathHelper.ToRadians(head.Angle)),
                    (float)Math.Sin(MathHelper.ToRadians(head.Angle))) * velocity;
                return;
            }
            
            if (hole < -5)
              RandomHole();
            if(hole < 0 && !FreeNodes)
               nodes.Add(new Node(head.Position, head.Angle, node2D, Scale));
            hole--;
                
            switch (d)
            {
                case Direction.Right: head.Angle += ANGLE_INCREASE;
                    break;
                case Direction.Left: head.Angle -= ANGLE_INCREASE;
                    break;
            }
            head.Position += new Vector2((float)Math.Cos(MathHelper.ToRadians(head.Angle)),
                                (float)Math.Sin(MathHelper.ToRadians(head.Angle))) * velocity;
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {            
            if (!collided)
                collided = HasCollided();
            if (NoWalls && (noWallsCounter % 25 >= 0) && (noWallsCounter % 25 < 15)) 
                head.Draw(spriteBatch, Color.Black);
            else if (!NoWalls)
                head.Draw(spriteBatch);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw(spriteBatch);
            }

            if (collided)
                spriteBatch.DrawString(font, LOST,
                    new Vector2((screenWidth - font.MeasureString(LOST).X) / 2,
                        (screenHeight - font.MeasureString(LOST).Y) / 2), Color.Black);
            noWallsCounter++;
        }

        public void UpdateVelocity(float multiplier)
        {
            this.velocity = velocity * multiplier;
            nodesCount = (int)(nodesCount / multiplier);
        }

        public void UpdateScale(float multiplier)
        {
            float newScale = Scale * multiplier;
            head.Scale = newScale;
            Scale = newScale;
        }

        private bool HasCollided()
        {

            if (head.IsOutOfBounds(screenWidth, screenHeight))
            {
                //Vector2 p = head.Position;
                if (NoWalls)
                {
                    if (head.Position.X < 0) head.position.X = (float)screenWidth;
                    else if (head.Position.X > screenWidth) head.position.X = 0.0f;

                    if (head.Position.Y < 0) head.position.Y = (float)screenHeight;
                    else if (head.Position.Y > screenHeight) head.position.Y = 0.0f;
                }
                else
                    return true;
            }
                

            if (nodes.Count > nodesCount - 1)
            {
                for (int i = nodes.Count - nodesCount; i >= 0; i--)
                    if (nodes[i].Intersects(head))
                        return true;
            }


            return false;
        }

        private void RandomHole()
        {
            if ((int)rnd.Next(65) == 15)
                hole = rnd.Next(8, 15);
            else
                hole = -6;
        }

        private void RandomHead()
        {
            Vector2 pos = new Vector2();
            pos.X = rnd.Next(PIXEL_MARGIN, screenWidth - PIXEL_MARGIN);
            pos.Y = rnd.Next(PIXEL_MARGIN, screenHeight - PIXEL_MARGIN);

            this.head = new Node(pos, 0.0f, head2D, Scale);//rnd.Next(0, 360), head2D, Scale);
        }

    }
}
