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
        SpriteFont font;

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
            font = content.Load<SpriteFont>("Font/SpriteFont1");
            difficultyBackground = content.Load<Texture2D>("Images/Menu/Background/difficultybackground");

            diffButtonTexture[casualIndex] = content.Load<Texture2D>(@"Images/Menu/casual");
            diffButtonTexture[normalIndex] = content.Load<Texture2D>(@"Images/Menu/normal");
            diffButtonTexture[hardIndex] = content.Load<Texture2D>(@"Images/Menu/hard");
            diffButtonTexture[backIndex] = content.Load<Texture2D>(@"Images/Menu/back");

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
            spriteBatch.DrawString(font, "Difficulty", new Vector2(550, 50), Color.HotPink);

            foreach (Button button in difficultyButtons)
            {
                button.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}
