using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using BreakABrick.GameComponents;
using BreakABrick.ApplicationComponents;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace BreakABrick.Screens
{
    class Play : Screen
    {
        #region Variabler och samlingar
        //Grafik för bakgrund
        Texture2D playBackground;
        Texture2D playHud;
        Texture2D menuBox;

        //Platta och plattans grafik
        Paddle paddle;
        Texture2D paddleTexture;

        //Boll med grafik och hastighet
        Ball ball;
        Texture2D ballTexture;
        Vector2 ballMotion;

        //Aktuell bana, lista för brickor och grafik
        int currentLevel = 2;
        List<Brick> bricks = new List<Brick>();
        Texture2D brickTexture;

        //Spelfält
        Rectangle gameField;
        Game1 game;

        //Föregående mus-status
        MouseState prevMouseState;
        //Aktuell mus-status
        MouseState currMouseState;
        
        //Paus-backgrund, yta och paus-status
        Texture2D pausedBackground;
        Rectangle pausedRectangle;
        KeyboardState prevKeyboardState;
        bool paused = false;

        //Spel status
        int active = 0;
        bool gameOver;
        bool levelComplete;
        string text;

        //Grafik, rektangel och ny knapp för nästa bana
        Texture2D nextLevelTexture;
        Rectangle nextLevelRectangle;
        Button nextLevelButton;

        //index för paus-knappar
        const int nPausButtons = 4,
            continueGameIndex = 0,
            resetGameIndex = 1,
            mainMenuIndex = 2,
            quitGameIndex = 3;
        //höjd och bredd på knappar
        int menuButtonHeight = 70,
            menuButtonWidth = 180;

        //Listor med grafik, plats och färg för knappar i pausmenyn
        Texture2D[] pausButtonTexture = new Texture2D[nPausButtons];
        Rectangle[] pausButtonRectangle = new Rectangle[nPausButtons];
        Color[] pausButtonColor = new Color[nPausButtons];

        //Lista med knappar för pausmenyn
        List<Button> pausButtons = new List<Button>();

        //för att flytta "main menu"-knappen vid olika lägen
        Rectangle defaultMainMenuLocation;
        Rectangle mainMenuLocation;

        //för att flytta "restart"-knappen vid olika lägen
        Rectangle defaultResetGameLocation;
        Rectangle resetGameLocation;
 
        //font för text
        SpriteFont font;
        SpriteFont font2;

        //poäng
        int score;
        
        #endregion

        #region Konstruktor

        public Play(ContentManager content, EventHandler screenEvent, Game1 game1, Rectangle gameField)
        : base(screenEvent)
        {
            game = game1;

            this.gameField = gameField;

            //Grafik
            playBackground = content.Load<Texture2D>("Images/Menu/Background/playS");
            playHud = content.Load<Texture2D>("Images/Game/gameHud");
            paddleTexture = content.Load<Texture2D>("Images/Game/paddle2");
            ballTexture = content.Load<Texture2D>("Images/Game/ball");
            brickTexture = content.Load<Texture2D>("Images/Game/brick");

            menuBox = content.Load<Texture2D>("Images/Menu/Background/box");

            //paus
            pausedBackground = content.Load<Texture2D>("Images/Menu/Background/pausbackground");
            pausedRectangle = new Rectangle(0, 0, pausedBackground.Width, pausedBackground.Height);
            //paus knappar
            pausButtonTexture[continueGameIndex] = content.Load<Texture2D>(@"Images/Menu/continuegame");
            pausButtonTexture[resetGameIndex] = content.Load<Texture2D>(@"Images/Menu/restart");
            pausButtonTexture[mainMenuIndex] = content.Load<Texture2D>(@"Images/Menu/mainmenu");
            pausButtonTexture[quitGameIndex] = content.Load<Texture2D>(@"Images/Menu/quitgame");
            nextLevelTexture = content.Load<Texture2D>(@"Images/Menu/nextlevel");

            //font
            font = content.Load<SpriteFont>("Font/SpriteFont1");
            font2 = content.Load<SpriteFont>("Font/SpriteFont2");

            //spel-objekt
            paddle = new Paddle(paddleTexture, gameField, 3);
            ball = new Ball(ballTexture, gameField);

            nextLevelButton = new Button(nextLevelTexture, nextLevelRectangle = new Rectangle(440, 410, menuButtonWidth, menuButtonHeight));
            mainMenuLocation = new Rectangle(660, 410, menuButtonWidth, menuButtonHeight);
            resetGameLocation = new Rectangle(440, 410, menuButtonWidth, menuButtonHeight);
            MenuButtonsPrep();

            for (int i = 0; i < nPausButtons; i++)
            {
                pausButtons.Add(new Button(pausButtonTexture[i], pausButtonRectangle[i]));
            }

            defaultMainMenuLocation = pausButtons[mainMenuIndex].Rectangle;
            defaultResetGameLocation = pausButtons[resetGameIndex].Rectangle;

            NewGame();
        }
        #endregion

        #region Metoder

        public void LoadLevel(int currentLevel)
        {
            StreamReader levelReader = new StreamReader(@"Content/Levels/Level" + currentLevel.ToString() + ".txt");

            string level = levelReader.ReadToEnd();

            levelReader.Close();

            int nextBrickX = 0;
            int nextBrickY = 0;

            for (int i = 0; i < level.Length; i++)
            {
                switch (level[i])
                {
                    case '0':
                        nextBrickX++;
                        break;
                    case '1':
                        if (!(nextBrickX >= 14))
                        {
                            bricks.Add(new Brick(brickTexture, new Rectangle((nextBrickX * 70) + 150, (nextBrickY * 25) + 50, 70, brickTexture.Height)));
                            nextBrickX++;
                        }                        
                        break;
                    case '\n':
                        if (!(nextBrickY >= 14))
                        {
                            nextBrickY++;
                            nextBrickX = 0;
                        }                      
                        break;
                }
            }
        }

        public void MenuButtonsPrep()
        {
            int x = (game.Window.ClientBounds.Width - menuButtonWidth) / 2;
            int y = game.Window.ClientBounds.Height / 2 - nPausButtons / 2 *
                menuButtonHeight - (nPausButtons % 2) * menuButtonHeight / 2
                + 50;
            for (int i = 0; i < nPausButtons; i++)
            {
                pausButtonColor[i] = Color.White;
                pausButtonRectangle[i] = new Rectangle(x, y, menuButtonWidth, menuButtonHeight);
                y += menuButtonHeight + 10;
            }


        }

        public void NewGame()
        {
            bricks.Clear();

            LoadLevel(currentLevel);

            //for (int i = 0; i < brickRows; i++)
            //{
            //    for (int j = 0; j < brickCols; j++)
            //    {
            //        bricks.Add(new Brick(brickTexture, new Rectangle((j*80) + 251, (i * 30) + 100, brickTexture.Width, brickTexture.Height)));
            //    }
            //}

            paddle.StartPosition();
            ball.Idle(paddle.Position);
        }

        public void GameTransition(int state)
        {
            switch (state)
            {
                case 1:
                    text = "GAME OVER\n\n\n";
                    gameOver = true;
                    break;
                case 2:
                    text = "LEVEL COMPLETE\n\n\n"; 
                    levelComplete = true;
                    break;
            }
        }

        public void BrickSound()
        {
            Random rnd = new Random();
            Audio.SoundBank.PlayCue(rnd.Next(1, 37).ToString());
        }
        #endregion

        #region Update
        public override void Update(GameTime gameTime)
        {
            KeyboardState currKeyboardState = Keyboard.GetState();
            currMouseState = Mouse.GetState();

            if (!paused && !gameOver && !levelComplete)
            {
                game.IsMouseVisible = false;

                if (prevKeyboardState.IsKeyUp(Keys.Escape) && currKeyboardState.IsKeyDown(Keys.Escape))
                {
                    Audio.SoundBank.PlayCue("paus");
                    //soundBank.PlayCue("paus");
                    paused = true;
                }

                paddle.Update(currMouseState);

                if (active == 0 || ball.Position.Y > gameField.Height)
                {
                    active = 0;

                    if (ball.Position.Y > gameField.Height)
                    {
                        paddle.RemoveLife();

                        if (paddle.Life == 0)
                        {
                            Audio.SoundBank.PlayCue("gameover");
                            GameTransition(1);
                        }
                    }
                    
                    ball.Idle(paddle.Position);

                    if (currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        Audio.SoundBank.PlayCue("tubeshot");
                        Random rnd = new Random();
                        ballMotion = new Vector2(rnd.Next(-4, 5), -8);
                        ball.Motion = ballMotion;
                        active = 1;
                    }
                }

                if (active == 1)
                {
                    if (bricks.Count == 0)
                    {
                        Audio.SoundBank.PlayCue("levelcomplete");
                        active = 0;
                        GameTransition(2);
                    }

                    ball.Update();
                    ball.PaddleCollision(new Rectangle((int)paddle.Position.X, (int)paddle.Position.Y, paddleTexture.Width, paddleTexture.Height), currMouseState);

                    for (int i = 0; i < bricks.Count; i++)
                    {
                        if (ball.BrickCollision(bricks[i].Position))
                        {
                            BrickSound();
                            bricks.RemoveAt(i);
                            score += 100;
                        }
                    }
                }
            }
            else if (paused || gameOver || levelComplete)
            {
                game.IsMouseVisible = true;
                if (levelComplete)
                {
                    if (nextLevelButton.ButtonUpdate())
                    {
                        pausButtons[mainMenuIndex].Rectangle = defaultMainMenuLocation;
                        pausButtons[resetGameIndex].Rectangle = defaultResetGameLocation;

                        currentLevel += 1;
                        NewGame();
                        levelComplete = false;
                    }
                }
                

                for (int i = 0; i < pausButtons.Count; i++)
                {
                    if (pausButtons[i].ButtonUpdate())
                    {
                        if (i == continueGameIndex)
                        {
                            paused = false;
                        }
                        if (i == resetGameIndex)
                        {
                            pausButtons[mainMenuIndex].Rectangle = defaultMainMenuLocation;
                            pausButtons[resetGameIndex].Rectangle = defaultResetGameLocation;
                            active = 0;
                            paddle.Life = 3;
                            score = 0;
                            NewGame();
                            paused = false;
                            gameOver = false;
                        }
                        if (i == mainMenuIndex)
                        {
                            screenEvent.Invoke(this, new EventArgs());
                        }
                        if (i == quitGameIndex)
                        {
                            game.Exit();
                        }                  
                    }
                }

                if (prevKeyboardState.IsKeyUp(Keys.Escape) && currKeyboardState.IsKeyDown(Keys.Escape))
                {
                    paused = false;
                }                
            }

            prevMouseState = currMouseState;
            prevKeyboardState = currKeyboardState;

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playBackground, Vector2.Zero, Color.White);
            spriteBatch.Draw(playHud, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "Lives \n" + paddle.Life, new Vector2(1140, 80), Color.HotPink);
            spriteBatch.DrawString(font, "Score \n" + score, new Vector2(1140, 15), Color.HotPink);
            spriteBatch.DrawString(font, "Level \n" + currentLevel, new Vector2(10, 15), Color.HotPink);
            spriteBatch.DrawString(font2, "ESC = Pause", new Vector2(10, 680), Color.HotPink);

            paddle.Draw(spriteBatch);

            foreach (Brick item in bricks)
            {
                item.Draw(spriteBatch);
            }

            ball.Draw(spriteBatch);

            if (paused || gameOver || levelComplete)
            {
                spriteBatch.Draw(pausedBackground, Vector2.Zero, Color.White);

                if (gameOver || levelComplete)
                {
                    pausButtons[mainMenuIndex].Rectangle = mainMenuLocation;
                    pausButtons[resetGameIndex].Rectangle = resetGameLocation;

                    spriteBatch.Draw(menuBox, new Vector2((game.Window.ClientBounds.Width - menuBox.Width) / 2, 200), Color.MediumPurple);

                    
                    spriteBatch.DrawString(font, score.ToString(), new Vector2(720, 260), Color.HotPink);
                    spriteBatch.DrawString(font, "Your Score", new Vector2(450, 260), Color.HotPink);
                    pausButtons[mainMenuIndex].Draw(spriteBatch);

                    if (gameOver)
                    {
                        spriteBatch.DrawString(font, text, new Vector2(550, 210), Color.HotPink);
                        pausButtons[resetGameIndex].Draw(spriteBatch);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, text, new Vector2(505, 210), Color.HotPink);
                        nextLevelButton.Draw(spriteBatch);
                    }
                }
                else
                {
                    foreach (Button button in pausButtons)
                    {
                        button.Draw(spriteBatch);
                    }
                }                
            }

            base.Draw(spriteBatch);
        }
        #endregion
    }
}
