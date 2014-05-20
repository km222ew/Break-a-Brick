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
using BreakABrick.ApplicationComponents;

namespace BreakABrick
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Skärmar
        MainMenu mainMenu;
        Options options;
        HowToPlay howToPlay;
        Play play;
        Difficulty difficulty;

        //Spelfält
        Rectangle gameField;

        //Aktuell skärm
        Screen currentScreen;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Fönstrets upplösning
            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;

            //Spelfält
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

            //Skapar nya skärmar
            mainMenu = new MainMenu(this.Content, new EventHandler(MainMenuEvent), this);
            options = new Options(this.Content, new EventHandler(OptionsEvent), this, graphics);
            howToPlay = new HowToPlay(this.Content, new EventHandler(HowToPlayEvent), this);
            difficulty = new Difficulty(this.Content, new EventHandler(DifficultyEvent), this);

            //Sätter igång bakgrundsmusiken
            Audio.SoundBank.PlayCue("thearea");

            //Visar muspekaren i fönstret
            IsMouseVisible = true;

            //Sätter startskärm till huvudmenyn
            currentScreen = mainMenu;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        #region Update
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

            //Uppdaterar ljudet
            Audio.AudioEngine.Update();

            //Uppdaterar aktuell skärm
            currentScreen.Update(gameTime);

            base.Update(gameTime);
        }
        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Ritar aktuell skärm
            spriteBatch.Begin();
            currentScreen.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        //Vad som händer när en skärm invokar ett event ("återvänder") till Game1
        #region Events
        
        //Skärm sätts beroende på vilket val man gjorde i huvudmenyn
        public void MainMenuEvent(object obj, EventArgs e)
        {
            ScreenChoice sc = (ScreenChoice)e;
            switch (sc.choice)
            {
                case 0:
                    currentScreen = difficulty;
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
            currentScreen = mainMenu;
        }

        //När man återvänder från svårighetsgrader startar spelet med vald svårighetsgrad, eller återvänder till huvudmenyn
        public void DifficultyEvent(object obj, EventArgs e)
        {
            ScreenChoice sc = (ScreenChoice)e;            

            if (sc.choice <= 2)
            {
                play = new Play(this.Content, new EventHandler(PlayEvent), this, gameField, sc.choice);
                currentScreen = play;
            }
            else
            {
                currentScreen = mainMenu;
            }
        }

        #endregion
    }
}
