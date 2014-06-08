using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BreakABrick.ApplicationComponents;

namespace BreakABrick.GameComponents
{
    class Paddle
    {
        Texture2D texture;
        Rectangle position;
        int life;

        int playFieldBorder = 151;

        Rectangle gameField;

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public int Life
        {
            get { return life; }
            set { life = value; }
        }

        public Paddle(Texture2D texture, Rectangle gameField, int life, Rectangle position)
        {
            this.texture    = texture;
            this.gameField  = gameField;
            this.life       = life;
            this.position   = position;

            StartPosition();
        }

        public void StartPosition()
        {
            position.X = (gameField.Width - texture.Width) / 2;
            position.Y = gameField.Height - texture.Height - 30;
        }

        public void RemoveLife()
        {
            Audio.SoundBank.PlayCue("lostlife");
            life -= 1;
        }

        public void Update(MouseState mouseState)
        {

            //Ser till så att plattan håller sig i spelfältet hur mycket användaren än rör med musen
            if (mouseState.X < playFieldBorder || mouseState.X + position.Width + playFieldBorder > gameField.Width)
            {
                if (mouseState.X < playFieldBorder)
                {
                    position.X = playFieldBorder;
                }
                if (mouseState.X + position.Width + playFieldBorder > gameField.Width)
                {
                    position.X = gameField.Width - position.Width - playFieldBorder;
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
