using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using BreakABrick.GameComponents;

namespace BreakABrick.Screens
{
    class Play : Screen
    {
        Texture2D playBackground;
        Texture2D playHud;

        Paddle paddle;
        Texture2D paddleTexture;
        Game1 game;

        public Play(ContentManager content, EventHandler screenEvent, Game1 game1, Rectangle gameField)
        : base(screenEvent)
        {
            game = game1;

            playBackground = content.Load<Texture2D>("Images/Menu/Background/playS");
            playHud = content.Load<Texture2D>("Images/Game/gameHud");
            paddleTexture = content.Load<Texture2D>("Images/Game/paddle");

            paddle = new Paddle(paddleTexture, gameField);
      
        }

        public override void Update(GameTime gameTime)
        {
            paddle.Update();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(playBackground, Vector2.Zero, Color.White);
            spriteBatch.Draw(playHud, Vector2.Zero, Color.White);

            paddle.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
