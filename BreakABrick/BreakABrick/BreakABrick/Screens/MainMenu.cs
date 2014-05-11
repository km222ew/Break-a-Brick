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

    class MainMenu : Screen
    {
        #region Variabler och samlingar
        const int nMenuButtons = 4,
            startGameIndex    = 0,
            howToPlayIndex    = 1,
            optionsIndex      = 2,
            quitGameIndex     = 3;
        int menuButtonHeight  = 70,
            menuButtonWidth   = 180;
        Game1 game;

        List<Button> menuButtons = new List<Button>();

        Texture2D[] menuButtonTexture = new Texture2D[nMenuButtons];
        Rectangle[] menuButtonRectangle = new Rectangle[nMenuButtons];
        Color[] menuButtonColor = new Color[nMenuButtons];
        #endregion

        public MainMenu(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            menuButtonTexture[startGameIndex] = content.Load<Texture2D>(@"Images/Menu/startgame");
            menuButtonTexture[howToPlayIndex] = content.Load<Texture2D>(@"Images/Menu/howtoplay");
            menuButtonTexture[optionsIndex] = content.Load<Texture2D>(@"Images/Menu/options");
            menuButtonTexture[quitGameIndex] = content.Load<Texture2D>(@"Images/Menu/quitgame");

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
                menuButtonColor[i] = Color.White;
                menuButtonRectangle[i] = new Rectangle(x, y, menuButtonWidth, menuButtonHeight);
                y += menuButtonHeight + 10;
            }
        }

        #region Update
        public override void Update(GameTime gameTime)
        {
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
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in menuButtons)
            {
                button.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
   
}
