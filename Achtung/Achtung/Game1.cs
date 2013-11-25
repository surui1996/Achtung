using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Achtung
{
    

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D head, node, powerUps;
        List<Snake> players;
        SnakesManager snakesManager;
        PowerUpsManager powerUpsManager;

        SpriteFont font;

        delegate void MoveDel(KeyboardState state);
        event MoveDel MoveSnakes;

        private const string LOST = "Game Over!!!";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {         

            spriteBatch = new SpriteBatch(GraphicsDevice);
            head = Content.Load<Texture2D>("head");
            node = Content.Load<Texture2D>("node");
            powerUps = Content.Load<Texture2D>("PowerUps");
            font = Content.Load<SpriteFont>("defaultFont");

            snakesManager = SnakesManager.GetInstance();            
            powerUpsManager = new PowerUpsManager(powerUps, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            players = new List<Snake>();
            players.Add(new Snake(head, node, "Red", font,
                new Keys[] { Keys.Left, Keys.Right }, snakesManager,
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            players.Add(new Snake(head, node, "Green", font,
               new Keys[] { Keys.A, Keys.D }, snakesManager,
               graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            players.Add(new Snake(head, node, "Blue", font,
               new Keys[] { Keys.G, Keys.J }, snakesManager,
               graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));
            
            snakesManager.Snakes = players;
            foreach (Snake s in players)
            {
                MoveSnakes += s.Move;
                s.RandomHead(snakesManager);
            }
                

            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private bool start = false;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Enter))
                start = true;

            bool gameOver = snakesManager.IsGameOver();
            if (state.IsKeyDown(Keys.Space) && gameOver) // new game
            {
                start = false;
                powerUpsManager.Reset();
                snakesManager.NewGame();
            }
            else if (gameOver)
            {
                powerUpsManager.Lost();
            }
            else if(start)
            {
                MoveSnakes(state);
                snakesManager.Intersection();
                powerUpsManager.Update(players, gameTime);
            }

            base.Update(gameTime);
        }

       

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            
            if (snakesManager.IsGameOver())
                spriteBatch.DrawString(font, LOST,
                    new Vector2((graphics.PreferredBackBufferWidth - font.MeasureString(LOST).X) / 2,
                        (graphics.PreferredBackBufferHeight - font.MeasureString(LOST).Y) / 2), Color.Green);
            
            foreach (Snake s in players)
                s.Draw(spriteBatch);
            powerUpsManager.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
