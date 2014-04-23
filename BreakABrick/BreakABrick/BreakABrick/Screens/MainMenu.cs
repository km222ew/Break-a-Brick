using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace BreakABrick
{
    enum MenuButtonState
    {
        Hover,
        MouseButtonUp,
        MouseButtonReleased,
        MouseButtonDown
    }

    class MainMenu : Screen
    {
        const int menuButtons = 4,
            startGameIndex = 0,
            howToPlayIndex = 1,
            optionsIndex = 2,
            quitGameIndex = 3;
        int menuButtonHeight = 70,
            menuButtonWidth = 180;
        Game1 game;

        Texture2D[] menuButtonTexture = new Texture2D[menuButtons];
        Rectangle[] menuButtonRectangle = new Rectangle[menuButtons];
        Color[] menuButtonColor = new Color[menuButtons];

        MenuButtonState[] menuButtonState = new MenuButtonState[menuButtons];

        bool currMouseState, prevMouseState = false;

        int mousePosX, mousePosY;

        //double frame_time;

        // determine state and color of button
        void update_buttons()
        {
            for (int i = 0; i < menuButtons; i++)
            {
                if (new Rectangle(mousePosX, mousePosY, 1, 1).Intersects(menuButtonRectangle[i]))
	            {		 	
                    if (currMouseState)
                    {
                        // mouse is currently down
                        menuButtonState[i] = MenuButtonState.MouseButtonDown;
                        menuButtonColor[i] = Color.Purple;
                    }
                    else if (!currMouseState && prevMouseState)
                    {
                        // mouse was just released
                        if (menuButtonState[i] == MenuButtonState.MouseButtonDown)
                        {
                            // button i was just down
                            menuButtonState[i] = MenuButtonState.MouseButtonReleased;
                        }
                    }
                    else
                    {
                        menuButtonState[i] = MenuButtonState.Hover;
                        menuButtonColor[i] = Color.Pink;
                    }
                }
                else
                {
                    menuButtonState[i] = MenuButtonState.MouseButtonUp;
                    menuButtonColor[i] = Color.White;
                }

                if (menuButtonState[i] == MenuButtonState.MouseButtonReleased)
                {
                    ButtonChoice(i);
                }
            }
        }

        void ButtonChoice(int i)
        {
            switch (i)
            {
                case startGameIndex:

                    break;
                case howToPlayIndex:

                    break;
                case optionsIndex:

                    break;
                case quitGameIndex:
                    game.Exit();
                    break;
                default:
                    break;
            }
        }

        public MainMenu(ContentManager content, EventHandler screenEvent, Game1 game1)
            : base(screenEvent)
        {
            menuButtonTexture[startGameIndex] = content.Load<Texture2D>(@"Images/Menu/startgame");
            menuButtonTexture[howToPlayIndex] = content.Load<Texture2D>(@"Images/Menu/howtoplay");
            menuButtonTexture[optionsIndex] = content.Load<Texture2D>(@"Images/Menu/options");
            menuButtonTexture[quitGameIndex] = content.Load<Texture2D>(@"Images/Menu/quitgame");

            game = game1;

            MenuButtonsPrep();

        }

        public void MenuButtonsPrep()
        {
            int x = (game.Window.ClientBounds.Width - menuButtonWidth) / 2;
            int y = game.Window.ClientBounds.Height / 2 - menuButtons / 2 *
                menuButtonHeight - (menuButtons % 2) * menuButtonHeight / 2;
            for (int i = 0; i < menuButtons; i++)
            {
                menuButtonColor[i] = Color.White;
                menuButtonRectangle[i] = new Rectangle(x, y, menuButtonWidth, menuButtonHeight);
                y += menuButtonHeight + 10;
            }
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            mousePosX = mouseState.X;
            mousePosY = mouseState.Y;
            prevMouseState = currMouseState;
            currMouseState = mouseState.LeftButton == ButtonState.Pressed;

            update_buttons();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menuButtons; i++)
            {
                spriteBatch.Draw(menuButtonTexture[i], menuButtonRectangle[i], menuButtonColor[i]);
            }
            base.Draw(spriteBatch);
        }
    }
   
}
