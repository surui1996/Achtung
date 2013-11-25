using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Achtung
{
    enum Direction { Left, Right, None };

    class Snake
    {
        private const int HEAD_RADIUS = 69;
        private const int STARTING_LENGTH = 5;
        private const float ANGLE_INCREASE = 3.5f;
        private const int PIXEL_MARGIN = 200;
        private const float DEFAULT_VELOCITY = 2.0f;
        private const float DEFAULT_SCALE = 0.05f;
        private const float MIN_DISTANCE = 125.0f;

        public int Score { get; set; }
        public float Scale { get; set; }
        public Color SnakeColor { get; set; }
        public string Name { get; set; }
        private Rectangle nodeRectangle;
        private Dictionary<string, Rectangle> headDic;
        private Dictionary<Direction, Keys> keysDic;

        private int nodesCount;
        

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
        public List<Node> Nodes { get { return nodes;} }
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

        public Snake(Texture2D head, Texture2D node, string name, Color color, SpriteFont font, Keys[] keys, SnakesManager sm, int screenWidth, int screenHeight)
        {
            this.head2D = head;
            this.node2D = node;
            this.Name = name;
            this.SnakeColor = color;
            this.font = font;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.Scale = DEFAULT_SCALE;
            this.Score = 0;
            this.Square = false;
            this.NoWalls = false;
            this.FreeNodes = false;
            this.changedDirection = false;
            this.rnd = new Random();

            headDic = new Dictionary<string, Rectangle>();
            headDic.Add("Fred", new Rectangle(0, 0, HEAD_RADIUS, HEAD_RADIUS));
            headDic.Add("Greenlee", new Rectangle(HEAD_RADIUS, 0, HEAD_RADIUS, HEAD_RADIUS));
            headDic.Add("Bluebell", new Rectangle(HEAD_RADIUS * 2, 0, HEAD_RADIUS, HEAD_RADIUS));
            this.nodeRectangle = headDic[name];

            keysDic = new Dictionary<Direction, Keys>();
            keysDic.Add(Direction.Left, keys[0]);
            keysDic.Add(Direction.Right, keys[1]);

            this.velocity = DEFAULT_VELOCITY;
            this.nodes = new List<Node>();
            this.nodesCount = (int)(1000 * DEFAULT_SCALE * (1 / DEFAULT_VELOCITY));           
        }

        public void Move(KeyboardState state)
        {
            if(collided)
                return;
            if (Square)
            {
                
                nodes.Add(new Node(head.Position, head.Angle, node2D, nodeRectangle, Scale));
                if(state.IsKeyDown(keysDic[Direction.Right]))
                {
                    if (!changedDirection) head.Angle += 90.0f;
                    changedDirection = true;
                }
                else if(state.IsKeyDown(keysDic[Direction.Left]))
                {
                    if (!changedDirection) head.Angle -= 90.0f;
                    changedDirection = true;
                }
                else
                    changedDirection = false;

                head.Position += new Vector2((float)Math.Cos(MathHelper.ToRadians(head.Angle)),
                    (float)Math.Sin(MathHelper.ToRadians(head.Angle))) * velocity;
                return;
            }
            
            if (hole < -5)
              RandomHole();
            if(hole < 0 && !FreeNodes)
               nodes.Add(new Node(head.Position, head.Angle, node2D, nodeRectangle, Scale));
            hole--;

            if (state.IsKeyDown(keysDic[Direction.Right]))
                head.Angle += ANGLE_INCREASE;
            else if (state.IsKeyDown(keysDic[Direction.Left]))
                head.Angle -= ANGLE_INCREASE;
            
            head.Position += new Vector2((float)Math.Cos(MathHelper.ToRadians(head.Angle)),
                                (float)Math.Sin(MathHelper.ToRadians(head.Angle))) * velocity;
        }
        

        public void Draw(SpriteBatch spriteBatch)
        {            
            if (!collided)
                collided = HasCollided();
            if (NoWalls && (noWallsCounter % 25 >= 0) && (noWallsCounter % 25 < 15)) 
                head.Draw(spriteBatch, 2.0f);
            else if (!NoWalls)
                head.Draw(spriteBatch);
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw(spriteBatch);
            }

            noWallsCounter++;
        }

        public void NewGame(SnakesManager sm)
        {           
            this.Scale = DEFAULT_SCALE;
            this.Square = false;
            this.NoWalls = false;
            this.FreeNodes = false;
            this.changedDirection = false;
            this.collided = false;
            this.velocity = DEFAULT_VELOCITY;
            this.nodes = new List<Node>();
            this.nodesCount = (int)(100 * DEFAULT_SCALE * DEFAULT_VELOCITY);


            RandomHead(sm);            
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

            if (FreeNodes) return false;

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

        public void RandomHead(SnakesManager sm)
        {
            Vector2 pos = new Vector2();
            bool intersects = true;
            while (intersects)
            {
                intersects = false;
                pos.X = rnd.Next(PIXEL_MARGIN, screenWidth - PIXEL_MARGIN);
                pos.Y = rnd.Next(PIXEL_MARGIN, screenHeight - PIXEL_MARGIN);
                foreach (Snake s in sm.Snakes)
                    if(s.Head != null)
                        if (Distance(pos, s.Head.Position) < MIN_DISTANCE)
                        {
                            intersects = true;
                            break;
                        }
            }

            this.head = new Node(pos, rnd.Next(0, 360), head2D, nodeRectangle, Scale);
            for (int i = 0; i < STARTING_LENGTH; i++)
            {
                nodes.Add(new Node(head.Position, head.Angle, node2D, nodeRectangle, Scale));
                head.Position += new Vector2((float)Math.Cos(MathHelper.ToRadians(head.Angle)),
                                (float)Math.Sin(MathHelper.ToRadians(head.Angle))) * velocity;
            }
            

        }

        private float Distance(Vector2 v1, Vector2 v2)
        {
            float x = (float)Math.Sqrt((double)(Math.Pow((double)(v1.X - v2.X), 2.0) + Math.Pow((double)(v1.Y - v2.Y), 2.0)));
            return x;
        }

        

    }
}
