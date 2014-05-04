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
using BreakABrick.Screens;

namespace BreakABrick
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MainMenu mainMenu;
        Options options;
        HowToPlay howToPlay;
        Play play;

        Rectangle gameField;

        Screen currentScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;

            //graphics.IsFullScreen = true;
            //graphics.ApplyChanges();

            

            gameField = new Rectangle(
                0,
                0,
                graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {      
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            mainMenu = new MainMenu(this.Content, new EventHandler(MainMenuEvent), this);
            options = new Options(this.Content, new EventHandler(OptionsEvent), this);
            howToPlay = new HowToPlay(this.Content, new EventHandler(HowToPlayEvent), this);
            play = new Play(this.Content, new EventHandler(PlayEvent), this, gameField );

            IsMouseVisible = true;
            currentScreen = mainMenu;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            currentScreen.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void MainMenuEvent(object obj, EventArgs e)
        {
            ScreenChoice sc = (ScreenChoice)e;
            switch (sc.choice)
            {
                case 0:
                    IsMouseVisible = false;
                    currentScreen = play;
                    break;
                case 1:
                    currentScreen = howToPlay;
                    break;
                case 2:
                    currentScreen = options;
                    break;                
                default:
                    break;
            }
        }

        public void OptionsEvent(object obj, EventArgs e)
        {
            currentScreen = mainMenu;
        }

        public void HowToPlayEvent(object obj, EventArgs e)
        {
            currentScreen = mainMenu;
        }

        public void PlayEvent(object obj, EventArgs e)
        {
            IsMouseVisible = true;
            play = new Play(this.Content, new EventHandler(PlayEvent), this, gameField);
            currentScreen = mainMenu;
        }
    }
}
