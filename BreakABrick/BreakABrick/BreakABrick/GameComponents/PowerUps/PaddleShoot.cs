using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick.GameComponents.PowerUps
{
    class PaddleShoot : PowerUp
    {
        Texture2D texture;
        Rectangle position;
        Vector2 motion;

        public override Rectangle Position
        {
            get { return position; }
        }

        public PaddleShoot(Texture2D texture, Rectangle position, Vector2 motion)
            : base(texture, position, motion)
        {
            this.texture = texture;
            this.position = position;
            this.motion = motion;
        }

        //public PowerUpAction()
        //{

        //}

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
