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

        //Föregående mus-status
        MouseState prevMouseState;
        //Aktuell mus-status
        MouseState currMouseState;

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

        Button muteMusicButton;
        Rectangle muteMusicRectangle;
        bool muteMusic = false;

        Button muteSoundEffectsButton;
        Rectangle muteSoundEffectsRectangle;
        bool muteSoundEffects;

        Button fullscreenButton;
        Rectangle fullscreenRectangle;

        Button vsyncButton;
        Rectangle vsyncRectangle;

        SpriteFont font;
        SpriteFont font2;

        Texture2D musicVolumeTexture;
        Rectangle musicVolumeRectangle;
        Texture2D musicVolumeStaticTexture;
        Rectangle musicVolumeStaticRectangle;

        Texture2D soundEVolumeTexture;
        Rectangle soundEVolumeRectangle;
        Texture2D soundEVolumeStaticTexture;
        Rectangle soundEVolumeStaticRectangle;



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

            font2 = content.Load<SpriteFont>("Font/SpriteFont2");
            

            menuBox = content.Load<Texture2D>("Images/Menu/Background/box");
            backButtonRectangle = new Rectangle((game1.Window.ClientBounds.Width - menuButtonWidth) / 2, 600, menuButtonWidth, menuButtonHeight);
            fullscreenRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 + 345, 230, checkboxWidth, checkboxHeight);
            vsyncRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 + 345, 280, checkboxWidth, checkboxHeight);

            muteMusicRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 340, 230, checkboxWidth, checkboxHeight);
            musicVolumeTexture = content.Load<Texture2D>("Images/Game/paddle2");
            musicVolumeRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 340, 330, musicVolumeTexture.Width, musicVolumeTexture.Height);
            musicVolumeStaticTexture = content.Load<Texture2D>(@"Images/Menu/checkbox");
            musicVolumeStaticRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 340, 330, musicVolumeTexture.Width, musicVolumeTexture.Height);


            muteSoundEffectsRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 340, 280, checkboxWidth, checkboxHeight);
            soundEVolumeTexture = content.Load<Texture2D>("Images/Game/paddle2");
            soundEVolumeRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 340, 380, musicVolumeTexture.Width, musicVolumeTexture.Height);
            soundEVolumeStaticTexture = content.Load<Texture2D>(@"Images/Menu/checkbox");
            soundEVolumeStaticRectangle = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 340, 380, musicVolumeTexture.Width, musicVolumeTexture.Height);

            backButton = new Button(backButtonTexture, backButtonRectangle);
            muteMusicButton = new Button(checkboxTextureOff, muteMusicRectangle);
            muteSoundEffectsButton = new Button(checkboxTextureOff, muteSoundEffectsRectangle);
            fullscreenButton = new Button(checkboxTextureOff, fullscreenRectangle);
            vsyncButton = new Button(checkboxTextureOn, vsyncRectangle);            
        }

        public override void Update(GameTime gameTime)
        {
            currMouseState = Mouse.GetState();

            if (!muteMusic)
            {
                Audio.MusicVolume = (float)musicVolumeRectangle.Width / 100;
            }

            if (!muteSoundEffects)
            {
                Audio.SoundEffectsVolume = (float)soundEVolumeRectangle.Width / 100;
            }
            

            if (backButton.ButtonUpdate())
            {
                screenEvent.Invoke(this, new EventArgs());
            }
            if (muteMusicButton.ButtonUpdate())
            {
                if (!muteMusic)
                {
                    muteMusic = true;
                    Audio.MusicVolume = 0.0f;
                    muteMusicButton.Texture = checkboxTextureOn;

                }
                else
                {
                    muteMusic = false;
                    muteMusicButton.Texture = checkboxTextureOff;
                }
            }
            if (muteSoundEffectsButton.ButtonUpdate())
            {
                if (!muteSoundEffects)
                {
                    muteSoundEffects = true;
                    Audio.SoundEffectsVolume = 0.0f;
                    muteSoundEffectsButton.Texture = checkboxTextureOn;

                }
                else
                {
                    muteSoundEffects = false;
                    muteSoundEffectsButton.Texture = checkboxTextureOff;
                }
            }
            if (new Rectangle(currMouseState.X, currMouseState.Y, 1, 1).Intersects(new Rectangle(musicVolumeStaticRectangle.X, musicVolumeStaticRectangle.Y, musicVolumeStaticRectangle.Width + 1, musicVolumeStaticRectangle.Height)))
            {
                if (currMouseState.LeftButton == ButtonState.Pressed)
                {
                    musicVolumeRectangle.Width = currMouseState.X - musicVolumeRectangle.X;                   
                }
            }

            if (new Rectangle(currMouseState.X, currMouseState.Y, 1, 1).Intersects(new Rectangle(soundEVolumeStaticRectangle.X, soundEVolumeStaticRectangle.Y, soundEVolumeStaticRectangle.Width + 1, soundEVolumeStaticRectangle.Height)))
            {
                if (currMouseState.LeftButton == ButtonState.Pressed)
                {
                    soundEVolumeRectangle.Width = currMouseState.X - soundEVolumeRectangle.X;

                    if (currMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        Audio.SoundBank.PlayCue("ballhit");
                    }
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

            prevMouseState = currMouseState;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(optionsBackground, Vector2.Zero, Color.White);

            //Ljud
            spriteBatch.DrawString(font, "Sound", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width - 5 , 160), Color.HotPink);
            spriteBatch.Draw(menuBox, new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width - 5, 200), Color.MediumPurple);
            spriteBatch.DrawString(font2, "Mute Music", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 30, 230), Color.HotPink);
            muteMusicButton.Draw(spriteBatch);
            spriteBatch.DrawString(font2, "Mute Sound Effects", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 30, 280), Color.HotPink);
            muteSoundEffectsButton.Draw(spriteBatch);


            spriteBatch.DrawString(font2, "Music Volume", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 30, 330), Color.HotPink);
            spriteBatch.Draw(musicVolumeStaticTexture, musicVolumeStaticRectangle, Color.White);
            spriteBatch.DrawString(font2, musicVolumeRectangle.Width.ToString(), new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 290, 330), Color.HotPink);
            spriteBatch.Draw(musicVolumeTexture, musicVolumeRectangle, Color.White);

            spriteBatch.DrawString(font2, "Sound Effects Volume", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 30, 380), Color.HotPink);
            spriteBatch.Draw(soundEVolumeStaticTexture, soundEVolumeStaticRectangle, Color.White);
            spriteBatch.DrawString(font2, soundEVolumeRectangle.Width.ToString(), new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 290, 380), Color.HotPink);
            spriteBatch.Draw(soundEVolumeTexture, soundEVolumeRectangle, Color.White);

            //Video
            spriteBatch.DrawString(font, "Video", new Vector2((game.Window.ClientBounds.Width) / 2 + 5, 160), Color.HotPink);
            spriteBatch.Draw(menuBox, new Vector2((game.Window.ClientBounds.Width) / 2 + 5, 200), Color.MediumPurple);
            spriteBatch.DrawString(font2, "Fullscreen", new Vector2((game.Window.ClientBounds.Width) / 2 + 40, 230), Color.HotPink);
            spriteBatch.DrawString(font2, "Vertical Sync", new Vector2((game.Window.ClientBounds.Width) / 2 + 40, 280), Color.HotPink);            
            fullscreenButton.Draw(spriteBatch);
            vsyncButton.Draw(spriteBatch);

            backButton.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }
    }
}
