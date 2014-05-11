using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using BreakABrick.ApplicationComponents;

namespace BreakABrick.Screens
{
    class Options : Screen
    {
        Texture2D optionsBackground;

        int menuButtonHeight = 70,
            menuButtonWidth = 180;

        Button backButton;
        Texture2D backButtonTexture;
        Rectangle backButtonRectangle;

        public Options(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            optionsBackground = content.Load<Texture2D>("Images/Menu/Background/optionsS");
            backButtonTexture = content.Load<Texture2D>(@"Images/Menu/back");

            int x = (game1.Window.ClientBounds.Width - menuButtonWidth) / 2;
            int y = (game1.Window.ClientBounds.Height - menuButtonHeight) / 2;               

            backButtonRectangle = new Rectangle(x, y, menuButtonWidth, menuButtonHeight);

            backButton = new Button(backButtonTexture, backButtonRectangle);
        }

        public override void Update(GameTime gameTime)
        {
            if (backButton.ButtonUpdate())
            {
                screenEvent.Invoke(this, new EventArgs());
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(optionsBackground, Vector2.Zero, Color.White);

            backButton.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
