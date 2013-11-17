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
        private List<PowerUp> activePowerUps;
        private Texture2D powerUpsTexture;
        private bool activated;

        public PowerUpsManager(Texture2D powerUpsTexture, int screenWidth, int screenHeight)
        {
            this.powerUpsTexture = powerUpsTexture;            
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            drawPowerUps = new List<PowerUp>();
            activePowerUps = new List<PowerUp>();
        }

        public void Update(Snake snake, GameTime gameTime)
        {
            while(drawPowerUps.Count != MAX)
                AddRandomPowerUp();

            foreach (PowerUp p in drawPowerUps)
            {
                if (snake.Head.Intersects(p))
                {
                    p.Start(snake, gameTime.TotalGameTime);
                    activePowerUps.Add(p);
                    activated = true;
                }
            }
            
            
            foreach (PowerUp p in activePowerUps)
            {
                if(activated)
                    drawPowerUps.Remove(p);
                if (gameTime.TotalGameTime.Subtract(p.StartTime) > DEFAULT_TIME)
                {
                    p.Stop();
                    activePowerUps.Remove(p);
                }                
            }
            activated = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (PowerUp p in drawPowerUps)
            {
                spriteBatch.Draw(p.Texture, p.Position, null, Color.CornflowerBlue, 0.0f,
                    new Vector2(p.Texture.Width / 2, p.Texture.Height / 2), 1.0f, SpriteEffects.None, 0);
            }
        }

        public void AddRandomPowerUp()
        {
            Vector2 pos = new Vector2();
            Random rnd = new Random();
            pos.X = rnd.Next(PIXEL_MARGIN, screenWidth - PIXEL_MARGIN);
            pos.Y = rnd.Next(PIXEL_MARGIN, screenHeight - PIXEL_MARGIN);

            drawPowerUps.Add(new SlowYourself(pos, powerUpsTexture));
        }
    }
}
