using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Achtung.PowerUps;

namespace Achtung
{
    class PowerUpsManager
    {
        public static int POWERUP_WIDTH = 42;
        public static int POWERUP_HEIGHT = 41;

        private const int MAX = 5;
        private TimeSpan DEFAULT_TIME = new TimeSpan(0, 0, 3);
        private const int PIXEL_MARGIN = 200;

        private const float X = 48.0f;

        private int screenWidth, screenHeight;
        private bool ThreeAdded;

        private List<PowerUp> drawPowerUps, activePowerUps, remove;
        private List<Snake> affectedSnakes;

        private Dictionary<string, Rectangle> powerUpsDic;
        private Texture2D powerUpsTexture;

        private Random rnd;

        public PowerUpsManager(Texture2D powerUpsTexture, int screenWidth, int screenHeight)
        {
            this.powerUpsTexture = powerUpsTexture;            
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            drawPowerUps = new List<PowerUp>();
            activePowerUps = new List<PowerUp>();
            remove = new List<PowerUp>();
            affectedSnakes = new List<Snake>();
            powerUpsDic = new Dictionary<string, Rectangle>();

            ThreeAdded = false;

            //Yourself
            powerUpsDic.Add("SlowYourself", new Rectangle(0, 0, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("SpeedYourself", new Rectangle((int)X, 0, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("ThinYourself", new Rectangle((int)(X * 2), 0, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("SquareYourself", new Rectangle((int)(X * 3), 0, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("AntiWallYourself", new Rectangle((int)(X * 4), 0, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("FreeYourself", new Rectangle((int)(X * 5), 0, POWERUP_WIDTH, POWERUP_HEIGHT));
            //Others
            powerUpsDic.Add("SlowOthers", new Rectangle(0, 43, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("SpeedOthers", new Rectangle((int)X, 43, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("ThickOthers", new Rectangle((int)(X * 2), 43, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("SquareOthers", new Rectangle((int)(X * 3), 43, POWERUP_WIDTH, POWERUP_HEIGHT));
            //All
            powerUpsDic.Add("Clean", new Rectangle(0, 43 * 2, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("NewThree", new Rectangle((int)X, 43 * 2, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("AntiWallAll", new Rectangle((int)(X * 2), 43 * 2, POWERUP_WIDTH, POWERUP_HEIGHT));
            //Random
            powerUpsDic.Add("RandomPowerUp", new Rectangle((int)(X * 3), 43 * 2, POWERUP_WIDTH, POWERUP_HEIGHT));
            
            rnd = new Random();
        }

        public void Update(List<Snake> snakes, GameTime gameTime)
        {
            foreach (Snake s in snakes)
            {
                UpdateSnake(s, snakes, gameTime);
            }
        }


        private void AddPowerUp(Snake s)
        {
            PowerUp p = AddRandomPowerUp();
            while (p == null)
                p = AddRandomPowerUp();
            while (s.Head.Intersects((p)))
            {
                p = AddRandomPowerUp();
                while (p == null) p = AddRandomPowerUp();
            }
            drawPowerUps.Add(p);
        }

        private void UpdateSnake(Snake snake, List<Snake> players, GameTime gameTime)
        {
            if(affectedSnakes != null)
                affectedSnakes = new List<Snake>();
            //Add new powerups to the field, randomly
            while (drawPowerUps.Count < MAX && ((int)rnd.Next(125) == 0))
                AddPowerUp(snake);

            //Start powerups effect on intersect event
            foreach (PowerUp p in drawPowerUps)
            {
                if (snake.Head.Intersects(p))
                {
                    if (p.Type == PowerUpType.Yourself)
                        affectedSnakes.Add(snake);
                    else if (p.Type == PowerUpType.Others)
                    {
                        foreach (Snake s in players)
                            if (s != snake)
                                affectedSnakes.Add(s);
                    }
                    else if (p.Name.Equals("NewThree")) //TODO: wait 3 seconds before adding the powerUps
                       ThreeAdded = true;
                    else
                        foreach (Snake s in players) affectedSnakes.Add(s);

                    p.Start(affectedSnakes, gameTime.TotalGameTime);
                    foreach (PowerUp active in activePowerUps)
                        if (active.Name.Equals(p.Name)) //TODO: Also the same class name besides the sub-class name
                            active.EffectTime += DEFAULT_TIME;
                    activePowerUps.Add(p);
                    if (remove == null)
                        remove = new List<PowerUp>();
                    remove.Add(p);
                }
            }
            if(ThreeAdded)
            {
                 for (int i = 0; i < 3; i++) AddPowerUp(snake);
                 ThreeAdded = false;
            }



            if (remove != null)
            {
                foreach (PowerUp p in remove) // remove powerUps from drawing
                    drawPowerUps.Remove(p);
                remove = null;
            }

            // calculate when the powerUp effect has to stop
            foreach (PowerUp p in activePowerUps)
            {
                if (gameTime.TotalGameTime.Subtract(p.StartTime) > p.EffectTime)
                {
                    //in p there is no affected snakes any more
                    p.Stop();
                    if (remove == null)
                        remove = new List<PowerUp>();
                    remove.Add(p);
                }
            }

            if (remove != null)
            {
                foreach (PowerUp p in remove) // remove stopped powerUps
                    activePowerUps.Remove(p);
                remove = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (PowerUp p in drawPowerUps)
                spriteBatch.Draw(powerUpsTexture, p.Position, powerUpsDic[p.Name], Color.White, 0.0f,
                    new Vector2(0,0), 1.0f, SpriteEffects.None, 0);
        }

        public void Lost()
        {
            foreach (PowerUp p in activePowerUps)
                p.Stop();

            activePowerUps = new List<PowerUp>();
            remove = null;
        }

        public void Reset()
        {
            drawPowerUps = new List<PowerUp>();
            activePowerUps = new List<PowerUp>();
            remove = null;
        }

        private PowerUp AddRandomPowerUp()
        {
            Vector2 pos = new Vector2(rnd.Next(PIXEL_MARGIN, screenWidth - PIXEL_MARGIN),
                rnd.Next(PIXEL_MARGIN, screenHeight - PIXEL_MARGIN));

            PowerUp power;
            int random = (int)rnd.Next(powerUpsDic.Count);
            if (random == 13)
            {
                random = (int)rnd.Next(powerUpsDic.Count - 1);
                power = MakePowerUp(pos, random);
                power.Name = "RandomPowerUp";
            }
            else
                power = MakePowerUp(pos, random);
            
            foreach (PowerUp p in drawPowerUps)
                if (power.Intersects(p))
                    return null;

            return power;
        }

        private PowerUp MakePowerUp(Vector2 pos, int random)
        {
            switch (random)
            {
                case 0: return new Slow(pos, PowerUpType.Yourself);
                    
                case 1: return new Speed(pos, PowerUpType.Yourself);
                    
                case 2: return new Resize(pos, PowerUpType.Yourself);
                    
                case 3: return new Square(pos, PowerUpType.Yourself);
                    
                case 4: return new AntiWall(pos, PowerUpType.Yourself);
                    
                case 5: return new FreeYourself(pos);
                    
                case 6: return new Slow(pos, PowerUpType.Others);
                    
                case 7: return new Speed(pos, PowerUpType.Others);
                    
                case 8: return new Resize(pos, PowerUpType.Others);
                    
                case 9: return new Square(pos, PowerUpType.Others);
                    
                case 10: return new Clean(pos);
                    
                case 11: return new NewThree(pos);
                    
                case 12: return new AntiWall(pos, PowerUpType.All);
                    
                default:
                    return null;
            }
        }
    }
}
