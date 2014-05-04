using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using BreakABrick.GameComponents;

namespace BreakABrick.Screens
{
    class Play : Screen
    {
        //Bakgrund och utrymme för spel-status
        Texture2D playBackground;
        Texture2D playHud;

        //Ny platta och plattans grafik
        Paddle paddle;
        Texture2D paddleTexture;

        //Ny boll med grafik och bollens rörelse
        Ball ball;
        Texture2D ballTexture;
        Vector2 ballMotion;

        //Brickor med grafik
        Texture2D brickTexture;

        int brickCols = 10;
        int brickRows = 5;
        List<Brick> bricks = new List<Brick>();

        Game1 game;

        MouseState prevMouseState;

        Rectangle gameField;

        Texture2D pausedBackground;
        Rectangle pausedRectangle;
        KeyboardState prevKeyboardState;
        bool paused = false;


        int active = 0;
        bool newGame = true;

        //paus knappar
        const int pausButtons = 4,
            continueGameIndex = 0,
            resetGameIndex = 1,
            mainMenuIndex = 2,
            quitGameIndex = 3;
        int menuButtonHeight = 70,
            menuButtonWidth = 180;

        MenuButtonState[] pausButtonState = new MenuButtonState[pausButtons];

        bool currMouseStatePaus, prevMouseStatePaus = false;

        int mousePosX, mousePosY;

        Texture2D[] pausButtonTexture = new Texture2D[pausButtons];
        Rectangle[] pausButtonRectangle = new Rectangle[pausButtons];
        Color[] pausButtonColor = new Color[pausButtons];


        public Play(ContentManager content, EventHandler screenEvent, Game1 game1, Rectangle gameField)
        : base(screenEvent)
        {
            game = game1;

            this.gameField = gameField;

            playBackground = content.Load<Texture2D>("Images/Menu/Background/playS");
            playHud = content.Load<Texture2D>("Images/Game/gameHud");
            paddleTexture = content.Load<Texture2D>("Images/Game/paddle2");
            ballTexture = content.Load<Texture2D>("Images/Game/ball");
            brickTexture = content.Load<Texture2D>("Images/Game/brick");

            //paus
            pausedBackground = content.Load<Texture2D>("Images/Menu/Background/pausbackground");
            pausedRectangle = new Rectangle(0, 0, pausedBackground.Width, pausedBackground.Height);
            //paus knappar
            pausButtonTexture[continueGameIndex] = content.Load<Texture2D>(@"Images/Menu/startgame");
            pausButtonTexture[resetGameIndex] = content.Load<Texture2D>(@"Images/Menu/howtoplay");
            pausButtonTexture[mainMenuIndex] = content.Load<Texture2D>(@"Images/Menu/options");
            pausButtonTexture[quitGameIndex] = content.Load<Texture2D>(@"Images/Menu/quitgame");


            paddle = new Paddle(paddleTexture, gameField);
            ball = new Ball(ballTexture, gameField);

            MenuButtonsPrep();  
        }

        public void MenuButtonsPrep()
        {
            int x = (game.Window.ClientBounds.Width - menuButtonWidth) / 2;
            int y = game.Window.ClientBounds.Height / 2 - pausButtons / 2 *
                menuButtonHeight - (pausButtons % 2) * menuButtonHeight / 2
                + 50;
            for (int i = 0; i < pausButtons; i++)
            {
                pausButtonColor[i] = Color.White;
                pausButtonRectangle[i] = new Rectangle(x, y, menuButtonWidth, menuButtonHeight);
                y += menuButtonHeight + 10;
            }
        }

        void ButtonsUpdate()
        {
            for (int i = 0; i < pausButtons; i++)
            {
                if (new Rectangle(mousePosX, mousePosY, 1, 1).Intersects(pausButtonRectangle[i]))
                {
                    if (currMouseStatePaus)
                    {
                        pausButtonState[i] = MenuButtonState.MouseButtonDown;
                        pausButtonColor[i] = Color.Purple;
                    }
                    else if (!currMouseStatePaus && prevMouseStatePaus)
                    {
                        if (pausButtonState[i] == MenuButtonState.MouseButtonDown)
                        {
                            pausButtonState[i] = MenuButtonState.MouseButtonReleased;
                        }
                    }
                    else
                    {
                        pausButtonState[i] = MenuButtonState.Hover;
                        pausButtonColor[i] = Color.Pink;
                    }
                }
                else
                {
                    pausButtonState[i] = MenuButtonState.MouseButtonUp;
                    pausButtonColor[i] = Color.White;
                }

                if (pausButtonState[i] == MenuButtonState.MouseButtonReleased)
                {
                    ButtonChoice(i);
                }
            }
        }

        void ButtonChoice(int button)
        {
            switch (button)
            {
                case continueGameIndex:
                    paused = false;
                    break;
                case resetGameIndex:
                    active = 0;
                    NewGame();
                    paused = false;
                    break;
                case mainMenuIndex:
                    screenEvent.Invoke(this, new EventArgs());
                    paused = false;
                    newGame = true;
                    break;
                case quitGameIndex:
                    game.Exit();
                    break;
            }
        }

        public void NewGame()
        {
            bricks.Clear();
            for (int i = 0; i < brickRows; i++)
            {
                for (int j = 0; j < brickCols; j++)
                {
                    bricks.Add(new Brick(brickTexture, new Rectangle((j*80) + 251, (i * 30) + 100, brickTexture.Width, brickTexture.Height)));
                }
            }

            paddle.StartPosition();
            ball.Idle(paddle.Position);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState currMouseState = Mouse.GetState();
            KeyboardState currKeyboardState = Keyboard.GetState();

            //Rectangle ballRectangle = new Rectangle(
            //    (int)ball.Position.X,
            //    (int)ball.Position.Y,
            //    ballTexture.Width,
            //    ballTexture.Height);

            if (newGame)
            {
                active = 0;
                newGame = false;
                NewGame();
            }

            if (!paused)
            {
                game.IsMouseVisible = false;

                if (prevKeyboardState.IsKeyUp(Keys.Escape) && currKeyboardState.IsKeyDown(Keys.Escape))
                {
                    paused = true;
                }
                
                paddle.Update();

                if (active == 0 || ball.Position.Y > gameField.Height)
                {
                    active = 0;
                    
                    ball.Idle(paddle.Position);

                    if (currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        Random rnd = new Random();
                        //ballMotion = new Vector2(0, -8);
                        ballMotion = new Vector2(rnd.Next(-2, 3), -8);
                        ball.Motion = ballMotion;
                        active = 1;
                    }
                }

                if (active == 1)
                {
                    if (bricks.Count == 0)
                    {
                        active = 0;
                        NewGame();
                    }
                    ball.Update();
                    ball.PaddleCollision(new Rectangle((int)paddle.Position.X, (int)paddle.Position.Y, paddleTexture.Width, paddleTexture.Height));

                    for (int i = 0; i < bricks.Count; i++)
                    {
                        if (ball.BrickCollision(bricks[i].Position))
                        {
                            bricks.RemoveAt(i);
                        }
                    }
                }

                if (prevKeyboardState.IsKeyUp(Keys.Back) && currKeyboardState.IsKeyDown(Keys.Back))
                {
                    screenEvent.Invoke(this, new EventArgs());
                    paused = false;
                    newGame = true;
                }
            }
            else if (paused)
            {
                game.IsMouseVisible = true;

                MouseState mouseState = Mouse.GetState();
                mousePosX = mouseState.X;
                mousePosY = mouseState.Y;
                prevMouseStatePaus = currMouseStatePaus;
                currMouseStatePaus = mouseState.LeftButton == ButtonState.Pressed;

                ButtonsUpdate();



                if (prevKeyboardState.IsKeyUp(Keys.Escape) && currKeyboardState.IsKeyDown(Keys.Escape))
                {
                    paused = false;
                }
                if (prevKeyboardState.IsKeyUp(Keys.Enter) && currKeyboardState.IsKeyDown(Keys.Enter))
                {
                    active = 0;
                    NewGame();
                    paused = false;
                }
                if (prevKeyboardState.IsKeyUp(Keys.Back) && currKeyboardState.IsKeyDown(Keys.Back))
                {
                    screenEvent.Invoke(this, new EventArgs());
                    paused = false;
     
                }
            }

            prevMouseState = currMouseState;
            prevKeyboardState = currKeyboardState;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playBackground, Vector2.Zero, Color.White);
            spriteBatch.Draw(playHud, Vector2.Zero, Color.White);

            paddle.Draw(spriteBatch);
            foreach (Brick item in bricks)
            {
                item.Draw(spriteBatch);
            }
            ball.Draw(spriteBatch);            

            if (paused)
            {
                spriteBatch.Draw(pausedBackground, Vector2.Zero, Color.White);
                for (int i = 0; i < pausButtons; i++)
                {
                    spriteBatch.Draw(pausButtonTexture[i], pausButtonRectangle[i], pausButtonColor[i]);
                }
            }

            base.Draw(spriteBatch);
        }
    }
}
