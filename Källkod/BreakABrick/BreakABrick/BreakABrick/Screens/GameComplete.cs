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
    class GameComplete : Screen
    {
        SpriteFont font3;

        Texture2D gameCompleteBackground;

        Button mainMenuButton;
        Texture2D mainMenuButtonTexture;
        Rectangle mainMenuButtonRectangle;

        int menuButtonHeight = 70,
            menuButtonWidth = 180;

        Game1 game;

        public GameComplete(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            this.game = game1;

            gameCompleteBackground  = content.Load<Texture2D>("Images/Menu/Background/gamecompletebackground");
            mainMenuButtonTexture   = content.Load<Texture2D>(@"Images/Menu/mainmenubutton");

            font3 = content.Load<SpriteFont>("Font/SpriteFont3");     

            mainMenuButtonRectangle = new Rectangle((game1.Window.ClientBounds.Width - menuButtonWidth) / 2, 600, menuButtonWidth, menuButtonHeight);

            mainMenuButton = new Button(mainMenuButtonTexture, mainMenuButtonRectangle);
        }

        public override void Update(GameTime gameTime)
        {
            if (mainMenuButton.ButtonUpdate())
            {
                screenEvent.Invoke(this, new EventArgs());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameCompleteBackground, Vector2.Zero, Color.White);

            mainMenuButton.Draw(spriteBatch);
        }
    }
}
