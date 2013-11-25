using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Achtung
{
    class ScoreManager
    {
        private SpriteFont font;
        private Rectangle scoreRectangle;
        private const float Y_MARGIN = 35.0f;
        private float X_OFFSET;
        public ScoreManager(SpriteFont font, Rectangle scoreRectangle)
        {
            this.font = font;
            this.scoreRectangle = scoreRectangle;

            string s = "Greenlee";
            X_OFFSET = font.MeasureString(s).X + 10.0f;
        }

        private int count = 0;
        public void Draw(SpriteBatch spriteBatch, List<Snake> snakes)
        {
            string s1, s2;
            int i = 1;
            foreach (Snake s in snakes)
            {
                s1 = s.Name;
                s2 = s.Score.ToString();
                float x1 = scoreRectangle.X;
                float x2 = scoreRectangle.X + X_OFFSET;
                float y = Y_MARGIN * i;
                spriteBatch.DrawString(font, s1, new Vector2(x1, y), s.SnakeColor);
                spriteBatch.DrawString(font, s2, new Vector2(x2, y), s.SnakeColor);
                i++;
            }
            count = snakes.Count;
        }

        public void DrawLost(SpriteBatch spriteBatch, string lost)
        {
            if (count == 0)
                return;
            float x = scoreRectangle.X;
            float y = scoreRectangle.Height / 2;
            spriteBatch.DrawString(font, lost,
                    new Vector2(x, y), Color.BurlyWood);

        }

    }
}
