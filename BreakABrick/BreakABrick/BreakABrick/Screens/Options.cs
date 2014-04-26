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
    class Options : Screen
    {
        Texture2D optionsBackground;

        public Options(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            optionsBackground = content.Load<Texture2D>("Images/Menu/Background/optionsS");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(optionsBackground, Vector2.Zero, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
