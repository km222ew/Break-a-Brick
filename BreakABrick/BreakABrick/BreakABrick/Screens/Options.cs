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
        GraphicsDeviceManager graphics;
        Game1 game;

        Texture2D optionsBackground;
        Texture2D menuBox;

        int menuButtonHeight = 70,
            menuButtonWidth = 180;

        Button backButton;
        Texture2D backButtonTexture;
        Rectangle backButtonRectangle;

        int checkboxHeight = 20,
            checkboxWidth = 20;
        Texture2D checkboxTextureOn;
        Texture2D checkboxTextureOff;

        Button muteSoundButton;
        Rectangle muteSoundRectangle;
        bool muteSound = false;

        Button fullscreenButton;
        Rectangle fullscreenRectangle;

        Button vsyncButton;
        Rectangle vsyncRectangle;

        SpriteFont font;

        public Options(ContentManager content, EventHandler screenEvent, Game1 game1, GraphicsDeviceManager graphics)
            : base(screenEvent)
        {
            this.graphics = graphics;
            this.game = game1;

            optionsBackground = content.Load<Texture2D>("Images/Menu/Background/optionsS");
            backButtonTexture = content.Load<Texture2D>(@"Images/Menu/back");

            checkboxTextureOn = content.Load<Texture2D>(@"Images/Menu/checkboxon2");
            checkboxTextureOff = content.Load<Texture2D>(@"Images/Menu/checkbox");

            font = content.Load<SpriteFont>("Font/SpriteFont1");

            menuBox = content.Load<Texture2D>("Images/Menu/Background/box");
            backButtonRectangle = new Rectangle((game1.Window.ClientBounds.Width - menuButtonWidth) / 2, 600, menuButtonWidth, menuButtonHeight);
            muteSoundRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 340, 238, checkboxWidth, checkboxHeight);
            fullscreenRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 + 345, 238, checkboxWidth, checkboxHeight);
            vsyncRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 + 345, 288, checkboxWidth, checkboxHeight);
            

            backButton = new Button(backButtonTexture, backButtonRectangle);
            muteSoundButton = new Button(checkboxTextureOff, muteSoundRectangle);
            fullscreenButton = new Button(checkboxTextureOff, fullscreenRectangle);
            vsyncButton = new Button(checkboxTextureOn, vsyncRectangle);            
        }

        public override void Update(GameTime gameTime)
        {
            if (backButton.ButtonUpdate())
            {
                screenEvent.Invoke(this, new EventArgs());
            }
            if (muteSoundButton.ButtonUpdate())
            {
                if (!muteSound)
                {
                    muteSound = true;
                    Audio.AudioVolume = 0.0f;
                    muteSoundButton.Texture = checkboxTextureOn;

                }
                else
                {
                    muteSound = false;
                    Audio.AudioVolume = 1.0f;
                    muteSoundButton.Texture = checkboxTextureOff;
                }
            }
            if (fullscreenButton.ButtonUpdate())
            {
                if (!graphics.IsFullScreen)
                {
                    graphics.IsFullScreen = true;
                    graphics.ApplyChanges();
                    fullscreenButton.Texture = checkboxTextureOn;

                }
                else
                {
                    graphics.IsFullScreen = false;
                    graphics.ApplyChanges();
                    fullscreenButton.Texture = checkboxTextureOff;
                }
            }

            if (vsyncButton.ButtonUpdate())
            {
                if (!graphics.SynchronizeWithVerticalRetrace)
                {
                    graphics.SynchronizeWithVerticalRetrace = true;
                    graphics.ApplyChanges();
                    vsyncButton.Texture = checkboxTextureOn;
                }
                else
                {
                    graphics.SynchronizeWithVerticalRetrace = false;
                    graphics.ApplyChanges();
                    vsyncButton.Texture = checkboxTextureOff;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(optionsBackground, Vector2.Zero, Color.White);

            //Ljud
            spriteBatch.DrawString(font, "Sound", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width - 5 , 160), Color.HotPink);
            spriteBatch.Draw(menuBox, new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width - 5, 200), Color.MediumPurple);
            spriteBatch.DrawString(font, "Mute sound", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 30, 230), Color.HotPink);
            muteSoundButton.Draw(spriteBatch);

            //Video
            spriteBatch.DrawString(font, "Video", new Vector2((game.Window.ClientBounds.Width) / 2 + 5, 160), Color.HotPink);
            spriteBatch.Draw(menuBox, new Vector2((game.Window.ClientBounds.Width) / 2 + 5, 200), Color.MediumPurple);
            spriteBatch.DrawString(font, "Fullscreen", new Vector2((game.Window.ClientBounds.Width) / 2 + 40, 230), Color.HotPink);
            spriteBatch.DrawString(font, "Vertical Sync", new Vector2((game.Window.ClientBounds.Width) / 2 + 40, 280), Color.HotPink);            
            fullscreenButton.Draw(spriteBatch);
            vsyncButton.Draw(spriteBatch);

            backButton.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
