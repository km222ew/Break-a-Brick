using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using BreakABrick.ApplicationComponents;


namespace BreakABrick.GameComponents
{
    class Ball
    {
        #region Variabler/Fält

        Texture2D texture;
        Vector2 position;
        Vector2 motion;      

        //Används för att ta en "snapshot" av plattans position innan kollision
        Vector2 paddleBeforeCollision = new Vector2(0, 0);

        //Maxhastighet på X-axeln för bollen
        int maxSpeed = 10;

        int playFieldBorder = 151;

        Rectangle gameField;
        #endregion

        #region Egenskaper
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

        #endregion

        #region Konstruktorer

        public Ball(Texture2D texture, Rectangle gameField)
        {
            this.texture    = texture;
            this.gameField  = gameField;
        }

        public Ball(Texture2D texture, Rectangle gameField, Vector2 position, Vector2 motion)
        {
            this.texture    = texture;
            this.gameField  = gameField;
            this.position   = position;
            this.motion     = motion;
        }

        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
            position = position + motion;

            //Sänker hastigheten på bollen gradvis om maxhastigheten överskrids
            if (motion.X > maxSpeed || motion.X < maxSpeed * -1)
            {
                motion.X *= 0.985f;
            }

            //Ser till så att bollen inte åker utanför spelplanen
            if (position.X < playFieldBorder || position.X + texture.Width + playFieldBorder > gameField.Width || position.Y < 0)
            {
                Audio.SoundBank.PlayCue("hit");

                if (position.X < playFieldBorder)
                {
                    position.X = playFieldBorder;
                    motion.X = -motion.X;
                }
                if (position.X + texture.Width + playFieldBorder > gameField.Width)
                {
                    position.X = gameField.Width - texture.Width - playFieldBorder;
                    motion.X = -motion.X;
                }
                if (position.Y < 0)
                {
                    position.Y = 0;
                    motion.Y = -motion.Y;
                }
            }
            
        }
        #endregion

        #region PaddleCollision
        public void PaddleCollision(Rectangle paddle, MouseState mouseState)
        {
            //Skapar en rektangel runt bollen för att kunna kolla kollision med plattan
            Rectangle ball = new Rectangle(
                (int)Position.X, 
                (int)Position.Y, 
                texture.Width, 
                texture.Height);

            Vector2 motionTemp = Motion;

            Random randSpeed = new Random();

            //Tar "snapshot" av plattans position innan bollen träffar för att kunna avgöra var plattan var innan kollisionen
            if (position.Y >= paddle.Y - 60 && position.Y <= paddle.Y - 40 && motion.Y > 0)
            {
                paddleBeforeCollision.X = paddle.X;
            }

            #region Rules for collision
            if (paddle.Intersects(ball) && motion.Y > 0)
            {
                //Ska se till så att en andra kollision inte detekteras direkt efter den första genom att backa bollen en uppdatering
                position -= motion;

                Audio.SoundBank.PlayCue("ballhit");

                //Om bollen åker åt höger...
                if (motion.X > 0)
                {
                    //..och plattan flyttas åt vänster
                    if (paddleBeforeCollision.X > paddle.X)
                    {
                        motion.X = motion.X + ((paddleBeforeCollision.X - paddle.X) / 10) * -1;

                        motion.Y *= -1;
                    }
                    //..och plattan flyttas åt höger
                    else if (paddleBeforeCollision.X < paddle.X)
                    {
                        motion.X = (paddle.X - paddleBeforeCollision.X) / 10;

                        motion.Y *= -1;
                    }
                    //.. och plattan är på samma ställe
                    else
                    {
                        motion.Y *= -1;
                    }
                }
                //Om bollen åker åt vänster...
                else if (motion.X < 0)
                {
                    //..och plattan flyttas åt vänster
                    if (paddleBeforeCollision.X > paddle.X)
                    {
                        motion.X = ((paddleBeforeCollision.X - paddle.X) / 10) * -1;

                        motion.Y *= -1;
                    }
                    //..och plattan flyttas åt höger
                    else if (paddleBeforeCollision.X < paddle.X)
                    {
                        motion.X = motion.X + (paddle.X - paddleBeforeCollision.X) / 10;

                        motion.Y *= -1;
                    }
                    //.. och plattan är på samma ställe
                    else
                    {
                        motion.Y *= -1;
                    }
                }
                //Om bollen åker rakt ner...
                else if (motion.X == 0)
                {
                    if (paddleBeforeCollision.X > paddle.X)
                    {
                        if (paddleBeforeCollision.X - paddle.X > 6)
                        {
                            motion.X = (6 + randSpeed.Next(-3, 4)) * -1;
                            motion.Y *= -1;
                        }
                        else
                        {
                            motion.X = paddleBeforeCollision.X - paddle.X;
                            motion.Y *= -1;
                        }
                    }
                    else if (paddleBeforeCollision.X < paddle.X)
                    {
                        if (paddle.X - paddleBeforeCollision.X > 6)
                        {
                            motion.X = (6 + randSpeed.Next(-3, 4));
                            motion.Y *= -1;
                        }
                        else
                        {
                            motion.X = paddleBeforeCollision.X - paddle.X;
                            motion.Y *= -1;
                        }
                    }
                    else
                    {
                        motion.Y *= -1;
                    }
                }

                #region Gammal kollisionskod
                //}
                //else
                //{
                //    if (ball.X + 20 >= paddle.X && ball.X + 20 <= (paddle.X + 33))
                //    {
                //        motion.X = (6 + randSpeed.Next(-3, 4)) * -1;
                //        motion.Y *= -1;
                //    }
                //    else if (ball.X + 20 >= paddle.X + 34 && ball.X + 20 <= paddle.X + 66)
                //    {
                //        motion.X = (motion.X * 0.1f);
                //        motion.Y *= -1;
                //    }
                //    else if (ball.X + 20 >= paddle.X + 67 && ball.X <= paddle.X + 100)
                //    {
                //        motion.X = (6 + randSpeed.Next(-3, 4));
                //        motion.Y *= -1;
                //    }
                //    else
                //    {
                //        motion.X *= -1;
                //    }
                //}
        

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

                //if (paddleVelocity.X < paddle.X)
                //{
                //    motion.X = motion.X + (8 * 0.05f);
                //    motion.Y *= -1;
                //}
                //if (paddleVelocity.X > paddle.X)
                //{
                //    motion.X = motion.X + (100 * 0.05f) * -1;
                //    motion.Y *= -1;
                //}

                //if (ball.X + 20 >= paddle.X && ball.X + 20 <= (paddle.X + 33))
                //{
                //    motion.X = motion.X + (100 * 0.05f) * -1;
                //    motion.Y *= -1;

                //    //motion.X = (paddle.X * 0.008f) * -1;
                //    //motion.Y *= -1;
                //}
                //else if (ball.X + 20 >= paddle.X + 34 && ball.X + 20 <= paddle.X + 66)
                //{
                //    motion.X = (motion.X * 0.5f);
                //    motion.Y *= -1;
                //}
                //else if (ball.X + 20 >= paddle.X + 67 && ball.X <= paddle.X + 100)
                //{
                //    motion.X = motion.X + (100 * 0.05f);
                //    motion.Y *= -1;

                //    //motion.X = (paddle.X * 0.008f);
                //    //motion.Y *= -1;
                //}
                //else
                //{
                //    motion.X *= -1;
                //}

                ////Om bollen åker åt höger...
                //if (motion.X > 0)
                //{
                //    //..och plattan flyttas åt vänster
                //    if (paddlePreCol.X > paddle.X)
                //    {
                //        if (paddlePreCol.X - paddle.X > 40)
                //        {
                //            motion.X = 6 * -1;
                //            motion.Y *= -1;
                //        }
                //        else
                //        {
                //            motion.X = motion.X * 0.15f;
                //            motion.Y *= -1;
                //        }
                //    }
                //    //..och plattan flyttas åt höger
                //    else if (paddlePreCol.X < paddle.X)
                //    {
                //        if (paddle.X - paddlePreCol.X > 6)
                //        {
                //            motion.X = 6;
                //            motion.Y *= -1;
                //        }
                //        else
                //        {
                //            motion.X = paddle.X - paddlePreCol.X;
                //            motion.Y *= -1;
                //        }

                //    }
                //    //.. och plattan är på samma ställe
                //    else
                //    {
                //        motion.X = motion.X * 0.30f;
                //        motion.Y *= -1;
                //    }
                //}
                ////Om bollen åker åt vänster...
                //else if (motion.X < 0)
                //{
                //    //..och plattan flyttas åt vänster
                //    if (paddlePreCol.X > paddle.X)
                //    {
                //        if (paddlePreCol.X - paddle.X > 6)
                //        {
                //            motion.X = 6 * -1;
                //            motion.Y *= -1;
                //        }
                //        else
                //        {
                //            motion.X = paddlePreCol.X - paddle.X;
                //            motion.Y *= -1;
                //        }
                //    }
                //    //..och plattan flyttas åt höger
                //    else if (paddlePreCol.X < paddle.X)
                //    {
                //        if (paddle.X - paddlePreCol.X > 40)
                //        {
                //            motion.X = 6;
                //            motion.Y *= -1;
                //        }
                //        else
                //        {
                //            motion.X = motion.X * 0.15f;
                //            motion.Y *= -1;
                //        }
                //    }
                //    //.. och plattan är på samma ställe
                //    else
                //    {
                //        motion.X = motion.X * 0.30f;
                //        motion.Y *= -1;
                //    }
                //}
                //else if (motion.X == 0)
                //{
                //    if (paddlePreCol.X > paddle.X)
                //    {
                //        if (paddlePreCol.X - paddle.X > 6)
                //        {
                //            motion.X = 6 * -1;
                //            motion.Y *= -1;
                //        }
                //        else
                //        {
                //            motion.X = paddlePreCol.X - paddle.X;
                //            motion.Y *= -1;
                //        }
                //    }
                //    else if (paddlePreCol.X < paddle.X)
                //    {
                //        if (paddle.X - paddlePreCol.X > 6)
                //        {
                //            motion.X = 6;
                //            motion.Y *= -1;
                //        }
                //        else
                //        {
                //            motion.X = paddlePreCol.X - paddle.X;
                //            motion.Y *= -1;
                //        }
                //    }
                //    else
                //    {
                //        motion.Y *= -1;
                //    }
                //}

                //motion.X = motion.X + (8 * 0.05f);
                //motion.Y *= -1;

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
                #endregion
            }
            #endregion
        }
        #endregion

        #region BrickCollision
        public bool BrickCollision(Rectangle brick)
        {

            //Skapar fyra punkter runt bollen för att kolla kollision med brickor (är inte perfekt, men hjälper för att bollen inte ska fastna i brickor) 
            Point[] collisionPoints = new Point[4];

            // toppen 
            collisionPoints[0] = new Point((int)position.X + 10, (int)position.Y);

            // vänster
            collisionPoints[1] = new Point((int)position.X, (int)position.Y + 10);

            // botten
            collisionPoints[2] = new Point((int)position.X + 10, (int)position.Y + 20);

            // höger
            collisionPoints[3] = new Point((int)position.X + 20, (int)position.Y + 10);

            if (brick.Contains(collisionPoints[0]))
            {
                position.Y = position.Y + 10;
                motion.Y = motion.Y * -1;

                //motion.X = motion.X * 0.9f; 
                return true;
            }
            else if (brick.Contains(collisionPoints[1]))
            {
                position.X = position.X + 10;
                motion.X = motion.X * -1;

                //motion.X = motion.X * 0.9f * -1; 
                return true;
            }
            else if (brick.Contains(collisionPoints[2]))
            {
                position.Y = position.Y - 10;
                motion.Y = motion.Y * -1;

                //motion.X = motion.X * 0.9f; 
                return true;
            }
            else if (brick.Contains(collisionPoints[3]))
            {
                position.X = position.X - 10;
                motion.X = motion.X * -1;

                //motion.X = motion.X * 0.9f * -1; 
                return true;
            }

            return false;
        }
        #endregion

        public void Idle(Rectangle paddle)
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
