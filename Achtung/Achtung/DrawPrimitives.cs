using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Achtung
{
    class DrawPrimitives
    {
        private BasicEffect basicEffect;
        private VertexPositionColor[] vertices;

        private bool init = false;
        public void Init(GraphicsDeviceManager graphics)
        {
            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter
               (0, graphics.GraphicsDevice.Viewport.Width,     // left, right
                graphics.GraphicsDevice.Viewport.Height, 0,    // bottom, top
                0, 1);                                         // near, far plane

            vertices = new VertexPositionColor[2];
            init = true;
        }

        public void DrawLine(GraphicsDeviceManager graphics, Vector2 p1, Vector2 p2)
        {
            if (!init)
                return;
            vertices[0].Position = new Vector3(p1.X, p1.Y, 0);
            vertices[0].Color = Color.Yellow;
            vertices[1].Position = new Vector3(p2.X, p2.Y, 0);
            vertices[1].Color = Color.Yellow;

            basicEffect.CurrentTechnique.Passes[0].Apply();
            graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList,
                vertices, 0, 1);
        }
    }
}
