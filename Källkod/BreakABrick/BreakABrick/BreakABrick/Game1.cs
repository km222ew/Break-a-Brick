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
        GameComplete gameComplete;
        Difficulty difficulty;
        HowToPlay howToPlay;
        MainMenu mainMenu;     
        Options options;
        Play play;

        Rectangle gameField;

        Screen currentScreen;

        ScreenChoice sc;

        //Bakgrundsmusik
        Cue music = Audio.SoundBank.GetCue("thearea");
        
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
            mainMenu        = new MainMenu(this.Content, new EventHandler(MainMenuEvent), this);
            options         = new Options(this.Content, new EventHandler(OptionsEvent), this, graphics);
            howToPlay       = new HowToPlay(this.Content, new EventHandler(HowToPlayEvent), this);
            difficulty      = new Difficulty(this.Content, new EventHandler(DifficultyEvent), this);
            gameComplete    = new GameComplete(this.Content, new EventHandler(GameCompleteEvent), this);

            //Sätter igång bakgrundsmusiken
            music.Play();

            IsMouseVisible = true;

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
            if (IsActive)
            {
                music.Resume();

                Audio.AudioEngine.Update();

                currentScreen.Update(gameTime);

                base.Update(gameTime);               
            }

            if (!IsActive)
            {
                music.Pause();
            }                  
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
            sc = (ScreenChoice)e;
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

        //Antinger går man till huvudmenyn eller så är spelet avklarat
        public void PlayEvent(object obj, EventArgs e)
        {
            sc = (ScreenChoice)e;

            IsMouseVisible = true;

            switch (sc.choice)
            {
                case 0:
                    currentScreen = mainMenu;
                    break;
                case 1:
                    currentScreen = gameComplete;
                    break;
                default:
                    break;
            }
        }

        //När man återvänder från svårighetsgrader startar spelet med vald svårighetsgrad, eller återvänder till huvudmenyn
        public void DifficultyEvent(object obj, EventArgs e)
        {
            sc = (ScreenChoice)e;            

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

        public void GameCompleteEvent(object obj, EventArgs e)
        {
            currentScreen = mainMenu;
        }

        #endregion
    }
}
