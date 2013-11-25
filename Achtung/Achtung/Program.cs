using System;

namespace Achtung
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (AchtungGame game = new AchtungGame())
            {
                game.Run();
            }
        }
    }
#endif
}

