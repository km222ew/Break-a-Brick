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
    class Difficulty : Screen
    {
        Texture2D difficultyBackground;
        SpriteFont font3;

        const int nDifficultyButtons = 4,
            casualIndex = 0,
            normalIndex = 1,
            hardIndex = 2,
            backIndex = 3;
        int menuButtonHeight = 70,
            menuButtonWidth = 180;

        Game1 game;

        List<Button> difficultyButtons = new List<Button>();

        Texture2D[] diffButtonTexture = new Texture2D[nDifficultyButtons];
        Rectangle[] diffButtonRectangle = new Rectangle[nDifficultyButtons];
        Color[] diffButtonColor = new Color[nDifficultyButtons];

        public Difficulty(ContentManager content, EventHandler screenEvent, Game1 game1) 
            :base(screenEvent)
        {
            game = game1;
            
            difficultyBackground = content.Load<Texture2D>("Images/Menu/Background/background5");

            diffButtonTexture[casualIndex]  = content.Load<Texture2D>(@"Images/Menu/casualbutton");
            diffButtonTexture[normalIndex]  = content.Load<Texture2D>(@"Images/Menu/normalbutton");
            diffButtonTexture[hardIndex]    = content.Load<Texture2D>(@"Images/Menu/hardbutton");
            diffButtonTexture[backIndex]    = content.Load<Texture2D>(@"Images/Menu/backbutton");

            font3 = content.Load<SpriteFont>("Font/SpriteFont3");

            MenuButtonsPrep();

            for (int i = 0; i < nDifficultyButtons; i++)
            {
                difficultyButtons.Add(new Button(diffButtonTexture[i], diffButtonRectangle[i]));
            }
        }

        public void MenuButtonsPrep()
        {
            int x = (game.Window.ClientBounds.Width - menuButtonWidth) / 2;
            int y = game.Window.ClientBounds.Height / 2 - nDifficultyButtons / 2 *
                menuButtonHeight - (nDifficultyButtons % 2) * menuButtonHeight / 2
                + 50;
            for (int i = 0; i < nDifficultyButtons; i++)
            {
                diffButtonColor[i] = Color.White;
                diffButtonRectangle[i] = new Rectangle(x, y, menuButtonWidth, menuButtonHeight);
                y += menuButtonHeight + 10;
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < difficultyButtons.Count; i++)
            {
                if (difficultyButtons[i].ButtonUpdate())
                {
                    if (i == casualIndex)
                    {
                        screenEvent.Invoke(this, new ScreenChoice(0));
                    }
                    if (i == normalIndex)
                    {
                        screenEvent.Invoke(this, new ScreenChoice(1));
                    }
                    if (i == hardIndex)
                    {
                        screenEvent.Invoke(this, new ScreenChoice(2));
                    }
                    if (i == backIndex)
                    {
                        screenEvent.Invoke(this, new ScreenChoice(3));
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(difficultyBackground, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font3, "Difficulty", new Vector2((game.Window.ClientBounds.Width - font3.MeasureString("Difficulty").X) / 2, 50), Color.Lime);

            foreach (Button button in difficultyButtons)
            {
                button.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}
