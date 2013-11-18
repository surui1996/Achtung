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
        private TimeSpan DEFAULT_TIME = new TimeSpan(0, 0, 5);
        private const int PIXEL_MARGIN = 200;

        private int screenWidth, screenHeight;

        private List<PowerUp> drawPowerUps, activePowerUps, remove;

        private Texture2D powerUpsTexture;
        

        public PowerUpsManager(Texture2D powerUpsTexture, int screenWidth, int screenHeight)
        {
            this.powerUpsTexture = powerUpsTexture;            
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            drawPowerUps = new List<PowerUp>();
            activePowerUps = new List<PowerUp>();
            remove = new List<PowerUp>();
        }

        public void Update(Snake snake, GameTime gameTime)
        {
            while (drawPowerUps.Count < MAX)
            {
                PowerUp p = AddRandomPowerUp();
                while(snake.Head.Intersects((p)))
                    p = AddRandomPowerUp();
                drawPowerUps.Add(p);
            }

            foreach (PowerUp p in drawPowerUps)
            {
                if (snake.Head.Intersects(p))
                {
                    //TODO: head iontersects without really touching it
                    p.Start(snake, gameTime.TotalGameTime);
                    activePowerUps.Add(p);
                    remove.Add(p);                   
                }
            }

            foreach (PowerUp p in remove)
            {
                drawPowerUps.Remove(p);
            }
            remove = new List<PowerUp>();
            
            foreach (PowerUp p in activePowerUps)
            {
                drawPowerUps.Remove(p);
                if (gameTime.TotalGameTime.Subtract(p.StartTime) > DEFAULT_TIME)
                {
                    p.Stop();
                    remove.Add(p);
                }                
            }
            foreach (PowerUp p in remove)
            {
                activePowerUps.Remove(p);
            }
            remove = new List<PowerUp>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (PowerUp p in drawPowerUps)
                spriteBatch.Draw(p.Texture, p.Position, null, Color.CornflowerBlue, 0.0f,
                    new Vector2(p.Texture.Width / 2, p.Texture.Height / 2), 1.0f, SpriteEffects.None, 0);
        }

        public PowerUp AddRandomPowerUp()
        {
            Vector2 pos = new Vector2();
            Random rnd = new Random();
            pos.X = rnd.Next(PIXEL_MARGIN, screenWidth - PIXEL_MARGIN);
            pos.Y = rnd.Next(PIXEL_MARGIN, screenHeight - PIXEL_MARGIN);            

            return new SlowYourself(pos, powerUpsTexture);
        }
    }
}
