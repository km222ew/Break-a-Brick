using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick.GameComponents
{
    class Ball
    {
        Vector2 position;
        Vector2 motion;
        Texture2D texture;

        Rectangle gameField;

        public Vector2 Motion
        {
            get { return motion; }
            set { motion = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Ball(Texture2D texture, Rectangle gameField)
        {
            this.texture = texture;
            this.gameField = gameField;
        }

        public void Update()
        {

            position = position + motion;

            if (position.X < 151)
            {
                position.X = 151;
                motion.X = -motion.X;
            }
            if (position.X + texture.Width + 151 > gameField.Width)
            {
                position.X = gameField.Width - texture.Width - 151;
                motion.X = -motion.X;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
                motion.Y = -motion.Y;
            }
        }

        public void PaddleCollision(Rectangle paddle)
        {
            Rectangle ball = new Rectangle(
                (int)Position.X, 
                (int)Position.Y, 
                texture.Width, 
                texture.Height);

            Vector2 motionTemp = Motion;

            if (paddle.Intersects(ball) && motion.Y > 0)
            {
                //motion.X = motion.X + (paddle.X * 0.003f);
                //motion.Y *= -1;

                //if (ball.X + (ball.Width / 2) > paddle.X + paddle.Width / 2)
                //{
                //    motion.X = (paddle.X * 0.008f);
                //    //motion.X = Math.Abs(paddle.X * 0.005f);
                //}
                //else
                //{
                //    motion.X = (paddle.X * 0.008f) * -1;
                //    //motion.X = Math.Abs(paddle.X * 0.005f) * -1;
                //}

                if (ball.X + 20 >= paddle.X && ball.X + 20 <= (paddle.X + 33))
                {
                    //motion.X = (paddle.X * 0.008f) * -1;

                    motion.X = Math.Abs(paddle.X * 0.008f) * -1;
                    motion.Y *= -1;
                }
                else if (ball.X + 20 >= paddle.X + 34 && ball.X + 20 <= paddle.X + 66)
                {
                    motion.X = (motion.X * 0.5f);
                    motion.Y *= -1;
                }
                else if (ball.X + 20 >= paddle.X + 67 && ball.X <= paddle.X + 100)
                {
                    //motion.X = (paddle.X * 0.008f);

                    motion.X = Math.Abs(paddle.X * 0.008f);
                    motion.Y *= -1;
                }
                else
                {
                    motion.X *= -1;
                }

                //if (ball.X + 10 >= paddle.X && ball.X + 10 <= (paddle.X + 19))
                //{
                //    Motion = new Vector2(-5.5F, -4.5F);
                //} 
                //if (ball.X + 10 >= paddle.X + 20 && ball.X + 6 <= (paddle.X + 38))
                //{
                //    if (Motion.X > 0)
                //    {
                //        motionTemp.X = -Motion.X;
                //        Motion = motionTemp;
                //    }
                //    motionTemp.Y = -Motion.Y;
                //    Motion = motionTemp;
                //}
                //if (ball.X + 10 >= paddle.X + 39 && ball.X + 6 <= paddle.X + 62)
                //{
                //    motionTemp.Y = -Motion.Y;
                //    Motion = motionTemp;
                //}
                //if (ball.X + 10 >= paddle.X + 63 && ball.X + 6 <= paddle.X + 81)
                //{
                //    if (Motion.X < 0)
                //    {
                //        motionTemp.X = -Motion.X;
                //        Motion = motionTemp;
                //    }
                //    motionTemp.Y = -Motion.Y;
                //    Motion = motionTemp;
                //}                                
                //if (ball.X + 10 >= paddle.X + 82 && ball.X + 6 <= paddle.X + 100)
                //{
                //    Motion = new Vector2(5.5F, -4.5F);
                //}
            }
        }

        public bool BrickCollision(Rectangle brick)
        {

            Point collisionPoint1;
            Point collisionPoint2;
            Point collisionPoint3;
            Point collisionPoint4;
            // top 
            collisionPoint1 = new Point((int)position.X + 10, (int)position.Y);
            // left 
            collisionPoint2 = new Point((int)position.X, (int)position.Y + 10);
            // bottom 
            collisionPoint3 = new Point((int)position.X + 10, (int)position.Y + 20);
            // right 
            collisionPoint4 = new Point((int)position.X + 20, (int)position.Y + 10);

            if (brick.Contains(collisionPoint1))
            {
                motion.Y = motion.Y * -1;
                position.Y = position.Y + 10;
                return true;
            }

            if (brick.Contains(collisionPoint2))
            {
                motion.X = motion.X * -1;
                position.X = position.X + 10;
                return true;
            }
            if (brick.Contains(collisionPoint3))
            {
                motion.Y = motion.Y * -1;
                position.Y = position.Y - 10;
                return true;
            }
            if (brick.Contains(collisionPoint4))
            {
                motion.X = motion.X * -1;
                position.X = position.X - 10;
                return true;
            }

            return false;

            //Rectangle ball = new Rectangle(
            //    (int)Position.X, 
            //    (int)Position.Y, 
            //    texture.Width, 
            //    texture.Height);

            //if (brick.Intersects(ball))
            //{
            //    position -= motion;

                
            //    if (ball.X + 20 >= brick.X && ball.X <= brick.X + 75)
            //    {
            //        motion.Y *= -1;
            //    }
            //    else
            //    {
            //        motion.X *= -1;
            //    }
            //}
        }

        public void Idle(Vector2 paddle)
        {
                position.X = paddle.X + 40;
                position.Y = paddle.Y - 20;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
