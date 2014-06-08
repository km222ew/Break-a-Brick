using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick.GameComponents
{
    //Abstract basklass som powerups ärver ifrån
    abstract class PowerUp
    {
        Texture2D texture;
        Rectangle position;
        Vector2 motion;

        public virtual Rectangle Position
        {
            get { return position; }
        }

        public PowerUp(Texture2D texture, Rectangle position, Vector2 motion)
        {
            this.texture = texture;
            this.position = position;
            this.motion = motion;
        }

        public virtual void Update()
        {
            position.Y = position.Y + (int)motion.Y;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
