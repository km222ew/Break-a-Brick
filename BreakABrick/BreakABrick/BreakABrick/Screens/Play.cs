using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick.Screens
{
    class Play : Screen
    {
         Texture2D playBackground;

         public Play(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            playBackground = content.Load<Texture2D>("Images/Menu/Background/playS");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(playBackground, Vector2.Zero, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
