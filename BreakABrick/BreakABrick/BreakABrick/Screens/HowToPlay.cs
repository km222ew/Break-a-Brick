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
    class HowToPlay : Screen
    {
        Texture2D howToPlayBackground;

        public HowToPlay(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            howToPlayBackground = content.Load<Texture2D>("Images/Menu/Background/howtoplayS");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(howToPlayBackground, Vector2.Zero, Color.White);
            base.Draw(spriteBatch);
        }
    }
}
