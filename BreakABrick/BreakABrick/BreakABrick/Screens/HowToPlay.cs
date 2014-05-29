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
        Texture2D howToPlayBackground;
        Texture2D menuBox;
        Rectangle menuBoxRectangle;

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

        SpriteFont font;
        SpriteFont font2;

        int menuButtonHeight = 70,
            menuButtonWidth = 180;

        Button backButton;
        Texture2D backButtonTexture;
        Rectangle backButtonRectangle;

        public HowToPlay(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            this.game = game1;

            howToPlayBackground = content.Load<Texture2D>("Images/Menu/Background/howtoplayS");
            backButtonTexture = content.Load<Texture2D>(@"Images/Menu/back");
            menuBox = content.Load<Texture2D>("Images/Menu/Background/howtoplaybox");

            paddle = content.Load<Texture2D>("Images/Game/paddle5");
            ball = content.Load<Texture2D>("Images/Game/ball5");
            brick = content.Load<Texture2D>("Images/Game/brick3");
            points = content.Load<Texture2D>("Images/Game/Powerups/points2");
            life = content.Load<Texture2D>("Images/Game/Powerups/life2");
            paddleUp = content.Load<Texture2D>("Images/Game/Powerups/paddleup3");
            paddleDown = content.Load<Texture2D>("Images/Game/Powerups/paddledown2");
            multiBall = content.Load<Texture2D>("Images/Game/Powerups/multiball2");
            paddleShoot = content.Load<Texture2D>("Images/Game/Powerups/paddleshoot2");


            font = content.Load<SpriteFont>("Font/SpriteFont1");

            font2 = content.Load<SpriteFont>("Font/SpriteFont2");

            backButtonRectangle = new Rectangle((game1.Window.ClientBounds.Width - menuButtonWidth) / 2, 600, menuButtonWidth, menuButtonHeight);
            menuBoxRectangle = new Rectangle((game1.Window.ClientBounds.Width - menuButtonWidth) / 2, 600, menuButtonWidth, menuButtonHeight);


            backButton = new Button(backButtonTexture, backButtonRectangle);
        }

        public override void Update(GameTime gameTime)
        {
            if (backButton.ButtonUpdate())
            {
                screenEvent.Invoke(this, new EventArgs());
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(howToPlayBackground, Vector2.Zero, Color.White);

            

            spriteBatch.Draw(menuBox, new Vector2(20, 200), Color.MediumPurple);
            spriteBatch.Draw(menuBox, new Vector2(440, 200), Color.MediumPurple);
            spriteBatch.Draw(menuBox, new Vector2(860, 200), Color.MediumPurple);

            //Basic info
            spriteBatch.Draw(paddle, new Vector2(170, 450), Color.White);
            spriteBatch.Draw(ball, new Vector2(190, 430), Color.White);
            spriteBatch.DrawString(font2, "1. Steer the paddle with your mouse", new Vector2(30, 220), Color.HotPink);
            spriteBatch.DrawString(font2, "2. Click to play the ball", new Vector2(30, 250), Color.HotPink);
            spriteBatch.DrawString(font2, "3. Hit the ball with the paddle", new Vector2(30, 280), Color.HotPink);
            spriteBatch.DrawString(font2, "4. Use momentum to flick the ball in \n the desired direction", new Vector2(30, 310), Color.HotPink);

            //Spelets mål
            spriteBatch.DrawString(font2, "The goal of the game is to destroy all \nthe bricks and collect points. There \nare different kind of bricks that have \nmore health. There is also the \nindestructible brick that can not be \ndestroyed.", 
                                    new Vector2(450, 220), Color.HotPink);

            spriteBatch.DrawString(font2, "1 hit", new Vector2(450, 420), Color.HotPink);
            spriteBatch.Draw(brick, new Vector2(450, 450), Color.Violet);
            spriteBatch.DrawString(font2, "2 hits", new Vector2(550, 420), Color.HotPink);
            spriteBatch.Draw(brick, new Vector2(550, 450), Color.HotPink);
            spriteBatch.DrawString(font2, "3 hits", new Vector2(650, 420), Color.HotPink);
            spriteBatch.Draw(brick, new Vector2(650, 450), Color.Purple);
            spriteBatch.DrawString(font2, "Infinite", new Vector2(750, 420), Color.HotPink);
            spriteBatch.Draw(brick, new Vector2(750, 450), Color.Gray);


            //Powerup info
            spriteBatch.Draw(points, new Vector2(880, 220), Color.White);
            spriteBatch.Draw(life, new Vector2(880, 265), Color.White);
            spriteBatch.Draw(paddleDown, new Vector2(880, 310), Color.White);
            spriteBatch.Draw(paddleUp, new Vector2(880, 355), Color.White);
            spriteBatch.Draw(multiBall, new Vector2(880, 400), Color.White);
            spriteBatch.Draw(paddleShoot, new Vector2(880, 445), Color.White);

            spriteBatch.DrawString(font2, "Gives 500 extra points", new Vector2(930, 225), Color.HotPink);
            spriteBatch.DrawString(font2, "Gives 1 extra life", new Vector2(930, 270), Color.HotPink);
            spriteBatch.DrawString(font2, "Smaller paddle", new Vector2(930, 315), Color.HotPink);
            spriteBatch.DrawString(font2, "Larger paddle", new Vector2(930, 360), Color.HotPink);
            spriteBatch.DrawString(font2, "Spawns more balls", new Vector2(930, 405), Color.HotPink);
            spriteBatch.DrawString(font2, "Click to shoot", new Vector2(930, 450), Color.HotPink);


            backButton.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
