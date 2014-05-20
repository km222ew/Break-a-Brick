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
        //Vector2 position;
        Texture2D texture;
        Rectangle position;
        int life;

        Rectangle gameField;

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        //public Rectangle CollisionBox
        //{
        //    get { return collisionBox; }
        //    set { collisionBox = value; }
        //}

        public int Life
        {
            get { return life; }
            set { life = value; }
        }

        public Paddle(Texture2D texture, Rectangle gameField, int life, Rectangle position)
        {
            this.texture = texture;
            this.gameField = gameField;
            this.life = life;
            this.position = position;

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

            if (mouseState.X < 151|| mouseState.X + position.Width + 151 > gameField.Width)
            {
                if (mouseState.X < 151)
                {
                    position.X = 151;
                }
                if (mouseState.X + position.Width + 151 > gameField.Width)
                {
                    position.X = gameField.Width - position.Width - 151;
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
