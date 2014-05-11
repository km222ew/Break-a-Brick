using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick.ApplicationComponents
{
    enum MenuButtonState
    {
        MouseHover,
        MouseButtonUp,
        MouseButtonReleased,
        MouseButtonDown
    }

    class Button
    {
        #region Fält och variabler
        Texture2D texture;
        Rectangle rectangle;
        Color color;

        MouseState mouseState;

        bool currMouseState, prevMouseState = false;

        int mousePosX, mousePosY;

        MenuButtonState state = new MenuButtonState();
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get { return texture; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        #endregion

        public Button(Texture2D texture, Rectangle rectangle)
        {
            this.texture = texture;
            this.rectangle = rectangle;
        }

        #region ButtonUpdate
        public bool ButtonUpdate()
        {
            mouseState = Mouse.GetState();

            mousePosX = mouseState.X;
            mousePosY = mouseState.Y;
            prevMouseState = currMouseState;
            currMouseState = mouseState.LeftButton == ButtonState.Pressed;

            if (new Rectangle(mousePosX, mousePosY, 1, 1).Intersects(rectangle))
            {
                if (currMouseState)
                {
                    state = MenuButtonState.MouseButtonDown;
                    color = Color.Purple;
                    return false;
                }
                else if (!currMouseState && prevMouseState)
                {
                    if (state == MenuButtonState.MouseButtonDown)
                    {
                        Audio.SoundBank.PlayCue("plop");
                        state = MenuButtonState.MouseButtonReleased;
                        return true;
                    }
                    return false;
                }
                else
                {
                    state = MenuButtonState.MouseHover;
                    color = Color.HotPink;
                    return false;
                }
            }
            else
            {
                state = MenuButtonState.MouseButtonUp;
                color = Color.White;
            }

            return false;
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}
