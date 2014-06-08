using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using BreakABrick.ApplicationComponents;

namespace BreakABrick.Screens
{
    class HowToPlay : Screen
    {
        #region Variabler/fält

        Texture2D background;
        Texture2D menuBox;

        Texture2D paddle,
                  ball,
                  brick,
                  points,
                  life,
                  paddleUp,
                  paddleDown,
                  multiBall,
                  paddleShoot;

        Game1 game;

        SpriteFont font,
                   font2,
                   font3;

        Color fontColor = Color.Lime;

        int menuButtonHeight = 70,
            menuButtonWidth = 180;

        Button backButton;
        Texture2D backButtonTexture;
        Rectangle backButtonRectangle;

        #endregion

        #region Konstruktor

        public HowToPlay(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            this.game = game1;

            background          = content.Load<Texture2D>("Images/Menu/Background/background");
            backButtonTexture   = content.Load<Texture2D>(@"Images/Menu/backbutton");
            menuBox             = content.Load<Texture2D>("Images/Menu/Background/howtoplaybox2");

            paddle      = content.Load<Texture2D>("Images/Game/paddle5");
            ball        = content.Load<Texture2D>("Images/Game/ball5");
            brick       = content.Load<Texture2D>("Images/Game/brick3");
            points      = content.Load<Texture2D>("Images/Game/Powerups/points2");
            life        = content.Load<Texture2D>("Images/Game/Powerups/life2");
            paddleUp    = content.Load<Texture2D>("Images/Game/Powerups/paddleup3");
            paddleDown  = content.Load<Texture2D>("Images/Game/Powerups/paddledown2");
            multiBall   = content.Load<Texture2D>("Images/Game/Powerups/multiball2");
            paddleShoot = content.Load<Texture2D>("Images/Game/Powerups/paddleshoot2");

            font    = content.Load<SpriteFont>("Font/SpriteFont1");
            font2   = content.Load<SpriteFont>("Font/SpriteFont2");
            font3   = content.Load<SpriteFont>("Font/SpriteFont3");

            backButtonRectangle = new Rectangle((game1.Window.ClientBounds.Width - menuButtonWidth) / 2, 600, menuButtonWidth, menuButtonHeight);

            backButton = new Button(backButtonTexture, backButtonRectangle);
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            if (backButton.ButtonUpdate())
            {
                screenEvent.Invoke(this, new EventArgs());
            }

            base.Update(gameTime);
        }

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            spriteBatch.DrawString(font3, "How to play", new Vector2((game.Window.ClientBounds.Width - font3.MeasureString("How to play").X) / 2, 50), fontColor);            

            //Basic info
            spriteBatch.DrawString(font, "Gameplay", new Vector2(20, 160), fontColor);
            spriteBatch.Draw(menuBox, new Vector2(20, 200), Color.SteelBlue);
            spriteBatch.Draw(paddle, new Vector2(170, 450), Color.White);
            spriteBatch.Draw(ball, new Vector2(190, 430), Color.White);
            spriteBatch.DrawString(font2, "1. Steer the paddle with your mouse", new Vector2(35, 220), fontColor);
            spriteBatch.DrawString(font2, "2. Click to play the ball", new Vector2(35, 250), fontColor);
            spriteBatch.DrawString(font2, "3. Hit the ball with the paddle", new Vector2(35, 280), fontColor);
            spriteBatch.DrawString(font2, "4. Use momentum to flick the ball in \n the desired direction", new Vector2(35, 310), fontColor);
            spriteBatch.DrawString(font2, "5. How hard you hit the ball with the \n paddle determines the balls direction", new Vector2(35, 360), fontColor);

            //Spelets mål
            spriteBatch.DrawString(font, "Goal", new Vector2(440, 160), fontColor);
            spriteBatch.Draw(menuBox, new Vector2(440, 200), Color.SteelBlue);
            spriteBatch.DrawString(font2, "The goal of the game is to destroy all \nthe bricks and collect points. There \nare different kind of bricks that have \nmore health. There is also the \nindestructible brick that can not be \ndestroyed.",
                                    new Vector2(455, 220), fontColor);
            spriteBatch.DrawString(font2, "  1 hit", new Vector2(455, 420), fontColor);
            spriteBatch.Draw(brick, new Vector2(455, 450), Color.Lime);
            spriteBatch.DrawString(font2, " 2 hits", new Vector2(555, 420), fontColor);
            spriteBatch.Draw(brick, new Vector2(555, 450), Color.Green);
            spriteBatch.DrawString(font2, " 3 hits", new Vector2(655, 420), fontColor);
            spriteBatch.Draw(brick, new Vector2(655, 450), Color.DarkGreen);
            spriteBatch.DrawString(font2, "Infinite", new Vector2(755, 420), fontColor);
            spriteBatch.Draw(brick, new Vector2(755, 450), Color.Gray);


            //Powerup info
            spriteBatch.DrawString(font, "Powerups", new Vector2(860, 160), fontColor);
            spriteBatch.Draw(menuBox, new Vector2(860, 200), Color.SteelBlue);
            spriteBatch.Draw(points, new Vector2(880, 220), Color.White);
            spriteBatch.Draw(life, new Vector2(880, 265), Color.White);
            spriteBatch.Draw(paddleDown, new Vector2(880, 310), Color.White);
            spriteBatch.Draw(paddleUp, new Vector2(880, 355), Color.White);
            spriteBatch.Draw(multiBall, new Vector2(880, 400), Color.White);
            spriteBatch.Draw(paddleShoot, new Vector2(880, 445), Color.White);

            spriteBatch.DrawString(font2, "Gives 500 extra points", new Vector2(930, 225), fontColor);
            spriteBatch.DrawString(font2, "Gives 1 extra life", new Vector2(930, 270), fontColor);
            spriteBatch.DrawString(font2, "Smaller paddle", new Vector2(930, 315), fontColor);
            spriteBatch.DrawString(font2, "Larger paddle", new Vector2(930, 360), fontColor);
            spriteBatch.DrawString(font2, "Spawns more balls", new Vector2(930, 405), fontColor);
            spriteBatch.DrawString(font2, "Click to shoot", new Vector2(930, 450), fontColor);


            backButton.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        #endregion
    }
}
