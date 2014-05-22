using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick.GameComponents.PowerUps
{
    class Laser : PowerUp
    {
        Texture2D texture;
        Rectangle position;
        Vector2 motion;
        bool hit = false;


        public override Rectangle Position
        {
            get { return position; }
        }

        public bool Hit
        {
            get { return hit; }
            set { hit = value; }
        }

        public Laser(Texture2D texture, Rectangle position, Vector2 motion)
            : base(texture, position, motion)
        {
            this.texture = texture;
            this.position = position;
            this.motion = motion;
        }

        public bool BrickCollision(Rectangle brick)
        {
            if (position.Intersects(brick))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Update()
        {
            position.Y = position.Y + (int)motion.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
