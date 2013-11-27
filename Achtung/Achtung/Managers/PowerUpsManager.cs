using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
            rnd = new Random();
        }

        public void Update(List<Snake> snakes, GameTime gameTime)
        {
            foreach (Snake s in snakes)
            {
                UpdateSnake(s, snakes, gameTime);
            }
        }

        private void UpdateSnake(Snake snake, List<Snake> players, GameTime gameTime)
        {
            if(affectedSnakes != null)
                affectedSnakes = new List<Snake>();
            //Add new powerups to the field, randomly
            while (drawPowerUps.Count < MAX && ((int)rnd.Next(100) == 0))
            {
                PowerUp p = AddRandomPowerUp();
                while (p == null)
                    p = AddRandomPowerUp();
                while (snake.Head.Intersects((p)))
                {
                    p = AddRandomPowerUp();
                    while (p == null) p = AddRandomPowerUp();
                }
                drawPowerUps.Add(p);
            }

            //Start powerups effect on intersect event
            foreach (PowerUp p in drawPowerUps)
            {
                if (snake.Head.Intersects(p))
                {
                    if (p.Type == PowerUpType.Yourself)
                        affectedSnakes.Add(snake);
                    else if (p.Type == PowerUpType.Others)
                        foreach (Snake s in players)
                            if(s != snake)
                                affectedSnakes.Add(s);
                        
                    p.Start(affectedSnakes, gameTime.TotalGameTime);
                    foreach (PowerUp active in activePowerUps)
                        if (active.Name.Equals(p.Name))
                            active.EffectTime += DEFAULT_TIME;
                    activePowerUps.Add(p);
                    if (remove == null)
                        remove = new List<PowerUp>();
                    remove.Add(p);
                }
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
            switch (random)
            {
                case 0: power = new Slow(pos, PowerUpType.Yourself);
                    break;
                case 1: power = new Speed(pos, PowerUpType.Yourself);
                    break;
                case 2: power = new Resize(pos, PowerUpType.Yourself);
                    break;
                case 3: power = new Square(pos, PowerUpType.Yourself);
                    break;
                case 4: power = new AntiWallYourself(pos);
                    break;
                case 5: power = new FreeYourself(pos);
                    break;
                case 6: power = new Slow(pos, PowerUpType.Others);
                    break;
                case 7: power = new Speed(pos, PowerUpType.Others);
                    break;
                case 8: power = new Resize(pos, PowerUpType.Others);
                    break;
                case 9: power = new Square(pos, PowerUpType.Others);
                    break;
                default:
                    return null;
            }
            
            foreach (PowerUp p in drawPowerUps)
                if (power.Intersects(p))
                    return null;

            return power;
        }
    }
}
