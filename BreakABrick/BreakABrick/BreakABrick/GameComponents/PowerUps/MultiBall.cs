using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick.GameComponents.PowerUps
{
    class MultiBall : PowerUp
    {
        Texture2D texture;
        Rectangle position;
        Vector2 motion;

        public override Rectangle Position
        {
            get { return position; }
        }

        public MultiBall(Texture2D texture, Rectangle position, Vector2 motion) 
            :base(texture, position, motion)
        {
            this.texture = texture;
            this.position = position;
            this.motion = motion;
        }

        //public List<Ball> PowerUpAction(List<Ball> balls)
        //{
        //    //int newBalls = 2;
        //    //for (int i = 0; i < newBalls; i++)
        //    //{
        //    //    balls.Add(new Ball(balls[0].
        //    //}
            
        //    //return balls;
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
