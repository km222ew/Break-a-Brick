using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick.GameComponents
{
    class Paddle
    {
        Vector2 position;
        Texture2D texture;

        MouseState mouseState;

        Rectangle gameField;

        public Paddle(Texture2D texture, Rectangle gameField )
        {
            this.texture = texture;
            this.gameField = gameField;

            position.X = (gameField.Width - texture.Width) / 2;
            position.Y = gameField.Height - texture.Height - 30;
        }

        public void Update()
        {
            mouseState = Mouse.GetState();

            if (mouseState.X < 151|| mouseState.X + texture.Width + 151 > gameField.Width)
            {
                if (mouseState.X < 151)
                {
                    position.X = 151;
                }
                if (mouseState.X + texture.Width > gameField.Width - 151)
                {
                    position.X = gameField.Width - texture.Width - 151;
                }
            }
            else
            {
                position.X = mouseState.X;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }
}
