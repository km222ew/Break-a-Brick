using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick.GameComponents
{
    class Brick
    {
        Texture2D texture;
        Rectangle position;
        Color color;
        int life;
        

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public int Life
        {
            get { return life; }
            set { life = value; }
        }

        public Brick(Texture2D texture, Rectangle position, int life)
        {
            this.texture = texture;
            this.position = position;
            this.life = life;
        }

        public void RemoveLife()
        {
            life -= 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, color);
        }
    }
}
