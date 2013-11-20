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
        private const int MAX = 2;
        private TimeSpan DEFAULT_TIME = new TimeSpan(0, 0, 3);
        private const int PIXEL_MARGIN = 200;
        private const int POWERUP_WIDTH = 38;
        private const int POWERUP_HEIGHT = 40;

        private int screenWidth, screenHeight;

        private List<PowerUp> drawPowerUps, activePowerUps, remove;

        private Dictionary<string, Rectangle> powerUpsDic;
        private Texture2D powerUpsTexture;
        

        public PowerUpsManager(Texture2D powerUpsTexture, int screenWidth, int screenHeight)
        {
            this.powerUpsTexture = powerUpsTexture;            
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            drawPowerUps = new List<PowerUp>();
            activePowerUps = new List<PowerUp>();
            remove = new List<PowerUp>();
            powerUpsDic = new Dictionary<string, Rectangle>();
            powerUpsDic.Add("SlowYourself", new Rectangle(0, 0, POWERUP_WIDTH, POWERUP_HEIGHT));
            powerUpsDic.Add("SpeedYourself", new Rectangle(POWERUP_WIDTH, 0, POWERUP_WIDTH, POWERUP_HEIGHT));
        }

        public void Update(Snake snake, GameTime gameTime)
        {
            //Add new powerups to the field
            while (drawPowerUps.Count < MAX) //TODO: appear at random times
            {
                PowerUp p = AddRandomPowerUp();
                while (p == null) p = AddRandomPowerUp();
                while (snake.Head.Intersects((p)))
                {
                    p = AddRandomPowerUp();
                    while (p == null) p = AddRandomPowerUp();
                }                    
                drawPowerUps.Add(p);
            }

            //Start powerups effect on intersect event
            //TODO: implemet it with events and not if's
            foreach (PowerUp p in drawPowerUps)
            {
                if (snake.Head.Intersects(p))
                {
                    snake.Head.Intersects(p);
                    p.Start(snake, gameTime.TotalGameTime);
                    activePowerUps.Add(p);
                    remove.Add(p);                   
                }
            }

            foreach (PowerUp p in remove) // remove powerUps from drawing
                drawPowerUps.Remove(p);
            remove = new List<PowerUp>();

            // calculate when the powerUp effect has to stop
            foreach (PowerUp p in activePowerUps) 
            {
                drawPowerUps.Remove(p);
                if (gameTime.TotalGameTime.Subtract(p.StartTime) > DEFAULT_TIME)
                {
                    p.Stop();
                    remove.Add(p);
                }                
            }

            foreach (PowerUp p in remove) // remove stopped powerUps
                activePowerUps.Remove(p);
            remove = new List<PowerUp>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (PowerUp p in drawPowerUps)
                spriteBatch.Draw(p.Texture, p.Position, powerUpsDic[p.Name], Color.CornflowerBlue, 0.0f,
                    new Vector2(0,0), 1.0f, SpriteEffects.None, 0);
        }

        public PowerUp AddRandomPowerUp()
        {
            Vector2 pos = new Vector2();
            Random rnd = new Random();
            pos.X = rnd.Next(PIXEL_MARGIN, screenWidth - PIXEL_MARGIN);
            pos.Y = rnd.Next(PIXEL_MARGIN, screenHeight - PIXEL_MARGIN);

            PowerUp power;
            if((int)rnd.Next(2) == 0)
                power = new SlowYourself(pos, powerUpsTexture);
            else
                power = new SpeedYourself(pos, powerUpsTexture);
            foreach (PowerUp p in drawPowerUps)
                if (power.Intersects(p))
                    return null;

            return power;
        }
    }
}
