using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick.Screens
{
    class Options : Screen
    {
        Texture2D optionsBackground;

        int menuButtonHeight = 70,
            menuButtonWidth = 180;
        Game1 game;

        MenuButtonState menuButtonState = new MenuButtonState();

        Texture2D menuButtonTexture;
        Rectangle menuButtonRectangle;
        Color menuButtonColor;

        bool currMouseState, prevMouseState = false;

        int mousePosX, mousePosY;

        public Options(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            optionsBackground = content.Load<Texture2D>("Images/Menu/Background/optionsS");
            menuButtonTexture = content.Load<Texture2D>(@"Images/Menu/back");

            game = game1;

            int x = (game.Window.ClientBounds.Width - menuButtonWidth) / 2;
            int y = (game.Window.ClientBounds.Height - menuButtonHeight) / 2;               

            menuButtonColor = Color.White;
            menuButtonRectangle = new Rectangle(x, y, menuButtonWidth, menuButtonHeight);
        }

        public void ButtonsUpdate()
        {
                if (new Rectangle(mousePosX, mousePosY, 1, 1).Intersects(menuButtonRectangle))
                {
                    if (currMouseState)
                    {
                        menuButtonState = MenuButtonState.MouseButtonDown;
                        menuButtonColor = Color.Purple;
                    }
                    else if (!currMouseState && prevMouseState)
                    {
                        if (menuButtonState == MenuButtonState.MouseButtonDown)
                        {
                            menuButtonState = MenuButtonState.MouseButtonReleased;
                        }
                    }
                    else
                    {
                        menuButtonState = MenuButtonState.Hover;
                        menuButtonColor = Color.Pink;
                    }
                }
                else
                {
                    menuButtonState = MenuButtonState.MouseButtonUp;
                    menuButtonColor = Color.White;
                }

                if (menuButtonState == MenuButtonState.MouseButtonReleased)
                {
                    screenEvent.Invoke(this, new EventArgs());
                }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            mousePosX = mouseState.X;
            mousePosY = mouseState.Y;
            prevMouseState = currMouseState;
            currMouseState = mouseState.LeftButton == ButtonState.Pressed;

            ButtonsUpdate();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(optionsBackground, Vector2.Zero, Color.White);
            spriteBatch.Draw(menuButtonTexture, menuButtonRectangle, menuButtonColor);
            base.Draw(spriteBatch);
        }
    }
}
