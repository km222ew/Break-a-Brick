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

        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }

        public Brick(Texture2D texture, Rectangle position)
        {
            this.texture = texture;
            this.position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
