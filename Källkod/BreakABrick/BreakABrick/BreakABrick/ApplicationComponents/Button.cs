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

    //Klass för att skapa en knapp, blev inte helt nöjd då jag fortfarande behöver göra dubbel kod för huvudmenyn och pausmenyn.
    class Button
    {
        #region Fält och variabler

        Texture2D texture;
        Rectangle rectangle;
        Color color;

        MouseState mouseState;

        bool currMouseState, prevMouseState = false;

        //Används för att ljudet som spelar vid hover inte ska upprepas
        bool mouseIsIntersecting;

        MenuButtonState state = new MenuButtonState();

        #endregion

        #region Egenskaper

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
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
            prevMouseState = currMouseState;
            currMouseState = mouseState.LeftButton == ButtonState.Pressed;

            if (new Rectangle(mouseState.X, mouseState.Y, 1, 1).Intersects(rectangle))
            {
                if (!mouseIsIntersecting)
                {
                    Audio.SoundBank.PlayCue("tap");
                    mouseIsIntersecting = true;
                }

                if (currMouseState)
                {
                    state = MenuButtonState.MouseButtonDown;
                    color = Color.LimeGreen;
                    return false;
                }
                else if (!currMouseState && prevMouseState)
                {
                    if (state == MenuButtonState.MouseButtonDown)
                    {
                        Audio.SoundBank.PlayCue("clunch");
                        state = MenuButtonState.MouseButtonReleased;
                        return true;
                    }
                    return false;
                }
                else
                {
                    state = MenuButtonState.MouseHover;
                    color = Color.Green;                    
                    return false;
                }
            }
            else
            {
                state = MenuButtonState.MouseButtonUp;
                mouseIsIntersecting = false;
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
