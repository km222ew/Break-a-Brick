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
using BreakABrick.GameComponents;

namespace BreakABrick.Screens
{

    class MainMenu : Screen
    {
        #region Variabler/Fält och samlingar

        Texture2D background;

        const int nMenuButtons = 4,
            startGameIndex    = 0,
            howToPlayIndex    = 1,
            optionsIndex      = 2,
            quitGameIndex     = 3;
        int menuButtonHeight  = 70,
            menuButtonWidth   = 180;

        List<Button> menuButtons = new List<Button>();

        Texture2D[] menuButtonTexture = new Texture2D[nMenuButtons];
        Rectangle[] menuButtonRectangle = new Rectangle[nMenuButtons];

        SpriteFont font4;
        SpriteFont font;

        Game1 game;

        #endregion

        public MainMenu(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            background = content.Load<Texture2D>("Images/Menu/Background/background5");

            menuButtonTexture[startGameIndex]   = content.Load<Texture2D>(@"Images/Menu/startgamebutton");
            menuButtonTexture[howToPlayIndex]   = content.Load<Texture2D>(@"Images/Menu/howtoplaybutton");
            menuButtonTexture[optionsIndex]     = content.Load<Texture2D>(@"Images/Menu/optionsbutton");
            menuButtonTexture[quitGameIndex]    = content.Load<Texture2D>(@"Images/Menu/quitbutton");

            font4   = content.Load<SpriteFont>("Font/SpriteFont4");
            font    = content.Load<SpriteFont>("Font/SpriteFont1");

            game = game1;

            MenuButtonsPrep();

            for (int i = 0; i < nMenuButtons; i++)
            {
                menuButtons.Add(new Button(menuButtonTexture[i], menuButtonRectangle[i]));               
            }
        }

        public void MenuButtonsPrep()
        {
            int x = (game.Window.ClientBounds.Width - menuButtonWidth) / 2;
            int y = game.Window.ClientBounds.Height / 2 - nMenuButtons / 2 *
                menuButtonHeight - (nMenuButtons % 2) * menuButtonHeight / 2
                +50;
            for (int i = 0; i < nMenuButtons; i++)
            {
                menuButtonRectangle[i] = new Rectangle(x, y, menuButtonWidth, menuButtonHeight);
                y += menuButtonHeight + 10;
            }
        }

        public override void Update(GameTime gameTime)
        {
            //Kollar efter vilket menyval som görs
            for (int i = 0; i < menuButtons.Count; i++)
            {
                if (menuButtons[i].ButtonUpdate())
                {
                    if (i == startGameIndex )
                    {
                        screenEvent.Invoke(this, new ScreenChoice(0));
                    }
                    if (i == howToPlayIndex)
                    {
                        screenEvent.Invoke(this, new ScreenChoice(1));
                    }
                    if (i == optionsIndex)
                    {
                        screenEvent.Invoke(this, new ScreenChoice(2));
                    }
                    if (i == quitGameIndex)
                    {
                        game.Exit();
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            spriteBatch.DrawString(font4, "Break A Brick", new Vector2((game.Window.ClientBounds.Width - font4.MeasureString("Break A Brick").X) / 2, 50), Color.Lime);

            foreach (Button button in menuButtons)
            {
                button.Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, "Created by Kevin Madsen", new Vector2((game.Window.ClientBounds.Width - font.MeasureString("Created by Kevin Madsen").X) / 2, 650), Color.Lime);

            base.Draw(spriteBatch);
        }
    }
   
}
