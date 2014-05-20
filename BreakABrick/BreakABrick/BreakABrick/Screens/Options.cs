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

        Texture2D optionsBackground;

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
        bool fullscreen = false;

        Button vsyncButton;
        Rectangle vsyncRectangle;
        bool vsync = true;

        SpriteFont font;

        public Options(ContentManager content, EventHandler screenEvent, Game1 game1, GraphicsDeviceManager graphics)
            : base(screenEvent)
        {
            this.graphics = graphics;

            optionsBackground = content.Load<Texture2D>("Images/Menu/Background/optionsS");
            backButtonTexture = content.Load<Texture2D>(@"Images/Menu/back");

            checkboxTextureOn = content.Load<Texture2D>(@"Images/Menu/checkboxon");
            checkboxTextureOff = content.Load<Texture2D>(@"Images/Menu/checkbox");

            font = content.Load<SpriteFont>("Font/SpriteFont1");

            backButtonRectangle = new Rectangle((game1.Window.ClientBounds.Width - menuButtonWidth) / 2, 600, menuButtonWidth, menuButtonHeight);
            muteSoundRectangle = new Rectangle(((game1.Window.ClientBounds.Width) / 2) -50, 255, checkboxWidth, checkboxHeight);
            fullscreenRectangle = new Rectangle(((game1.Window.ClientBounds.Width) / 2) - 50, 325, checkboxWidth, checkboxHeight);
            vsyncRectangle = new Rectangle(((game1.Window.ClientBounds.Width) / 2) - 50, 395, checkboxWidth, checkboxHeight);

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
                if (!fullscreen)
                {
                    fullscreen = true;
                    graphics.IsFullScreen = true;
                    graphics.ApplyChanges();
                    fullscreenButton.Texture = checkboxTextureOn;

                }
                else
                {
                    fullscreen = false;
                    graphics.IsFullScreen = false;
                    graphics.ApplyChanges();
                    fullscreenButton.Texture = checkboxTextureOff;
                }
            }

            if (vsyncButton.ButtonUpdate())
            {
                if (!vsync)
                {
                    vsync = true;
                    graphics.SynchronizeWithVerticalRetrace = true;
                    graphics.ApplyChanges();
                    vsyncButton.Texture = checkboxTextureOn;
                }
                else
                {
                    vsync = false;
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

            spriteBatch.DrawString(font, "Mute sound", new Vector2(640, 250), Color.HotPink);
            spriteBatch.DrawString(font, "Fullscreen", new Vector2(640, 320), Color.HotPink);
            spriteBatch.DrawString(font, "Vertical Sync", new Vector2(640, 390), Color.HotPink);

            backButton.Draw(spriteBatch);
            muteSoundButton.Draw(spriteBatch);
            fullscreenButton.Draw(spriteBatch);
            vsyncButton.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
