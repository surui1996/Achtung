using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Achtung
{
    class MathHelper
    {
        public static float ToRadians(float angle)
        {
            return (float)(angle * Math.PI / 180.0f);
        }
    }
}
