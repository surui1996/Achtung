using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Achtung
{
    class SnakesManager
    {
        public List<Snake> Snakes { get; set; }
        
        private static SnakesManager manager;

        public static SnakesManager GetInstance()
        {
            if (manager == null)
                manager = new SnakesManager();
            return manager;
        }

        private SnakesManager()
        {
            Snakes = new List<Snake>();
        }

        public void Intersection()
        {
            Node head;
            //TODO: make the list randomly pick who's first for 'eveness'
            for (int i = 0; i < Snakes.Count; i++)
            {
                if(!Snakes[i].FreeNodes)
                {
                    head = Snakes[i].Head;
                    for (int j = 0; j < Snakes.Count; j++)
                        if (head != Snakes[j].Head)
                            foreach (Node n in Snakes[j].Nodes)
                                if (head.Intersects(n))
                                    Snakes[i].Collided = true;
                }
                
            }
        }

        public void ScoreWinner()
        {
            foreach (Snake s in Snakes)
                if (!s.Collided) s.Score += 1;
        }

        public bool IsGameOver()
        {
            int counter = 0;
            foreach (Snake s in Snakes)
                if (!s.Collided) counter++;
            if (counter <= 1)
                return true;
            return false;
        }

        public void NewGame()
        {
            foreach (Snake s in Snakes)
                s.NewGame(this);
        }
    }
}
