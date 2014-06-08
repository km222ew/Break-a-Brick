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
        #region Variabler/Fält

        GraphicsDeviceManager graphics;
        Game1 game;

        MouseState prevMouseState;
        MouseState currMouseState;

        Texture2D background;
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
        bool muteMusic;

        Button muteSoundEffectsButton;
        Rectangle muteSoundEffectsRectangle;
        bool muteSoundEffects;

        Button fullscreenButton;
        Rectangle fullscreenRectangle;

        Button vsyncButton;
        Rectangle vsyncRectangle;

        SpriteFont font,
                   font2,
                   font3;

        Color fontColor = Color.Lime;

        Texture2D musicVolumeTexture;
        Rectangle musicVolumeRectangle;
        Texture2D musicVolumeBackgroundTexture;
        Rectangle musicVolumeInputRectangle;

        Texture2D soundEVolumeTexture;
        Rectangle soundEVolumeRectangle;
        Texture2D soundEVolumeBackgroundTexture;
        Rectangle soundEVolumeInputRectangle;

        #endregion

        #region Konstruktor

        public Options(ContentManager content, EventHandler screenEvent, Game1 game1, GraphicsDeviceManager graphics)
            : base(screenEvent)
        {
            this.graphics = graphics;
            this.game = game1;

            background          = content.Load<Texture2D>("Images/Menu/Background/background");
            backButtonTexture   = content.Load<Texture2D>(@"Images/Menu/backbutton");
            menuBox             = content.Load<Texture2D>("Images/Menu/Background/box4");
            checkboxTextureOn   = content.Load<Texture2D>(@"Images/Menu/checkboxon5");
            checkboxTextureOff  = content.Load<Texture2D>(@"Images/Menu/checkbox");

            musicVolumeTexture              = content.Load<Texture2D>("Images/Game/paddle2");
            musicVolumeBackgroundTexture    = content.Load<Texture2D>(@"Images/Menu/checkbox");
            soundEVolumeTexture             = content.Load<Texture2D>("Images/Game/paddle2");
            soundEVolumeBackgroundTexture   = content.Load<Texture2D>(@"Images/Menu/checkbox");

            font    = content.Load<SpriteFont>("Font/SpriteFont1");
            font2   = content.Load<SpriteFont>("Font/SpriteFont2");
            font3   = content.Load<SpriteFont>("Font/SpriteFont3");
             
            backButtonRectangle         = new Rectangle((game1.Window.ClientBounds.Width - menuButtonWidth) / 2, 600, menuButtonWidth, menuButtonHeight);
            fullscreenRectangle         = new Rectangle((game.Window.ClientBounds.Width) / 2 + 335, 230, checkboxWidth, checkboxHeight);
            vsyncRectangle              = new Rectangle((game.Window.ClientBounds.Width) / 2 + 335, 280, checkboxWidth, checkboxHeight);
            muteMusicRectangle          = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 330, 230, checkboxWidth, checkboxHeight);
            musicVolumeRectangle        = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 330, 330, musicVolumeTexture.Width, musicVolumeTexture.Height);            
            musicVolumeInputRectangle   = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 330, 330, musicVolumeTexture.Width, musicVolumeTexture.Height);
            muteSoundEffectsRectangle   = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 330, 280, checkboxWidth, checkboxHeight);            
            soundEVolumeRectangle       = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 330, 380, musicVolumeTexture.Width, musicVolumeTexture.Height);           
            soundEVolumeInputRectangle  = new Rectangle((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 330, 380, musicVolumeTexture.Width, musicVolumeTexture.Height);

            //Knappar
            backButton              = new Button(backButtonTexture, backButtonRectangle);
            muteMusicButton         = new Button(checkboxTextureOff, muteMusicRectangle);
            muteSoundEffectsButton  = new Button(checkboxTextureOff, muteSoundEffectsRectangle);
            fullscreenButton        = new Button(checkboxTextureOff, fullscreenRectangle);
            vsyncButton             = new Button(checkboxTextureOn, vsyncRectangle);            
        }
        #endregion

        #region Update

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

            if (new Rectangle(currMouseState.X, currMouseState.Y, 1, 1).Intersects(new Rectangle(
                musicVolumeInputRectangle.X, 
                musicVolumeInputRectangle.Y, 
                musicVolumeInputRectangle.Width + 1, 
                musicVolumeInputRectangle.Height)))
            {
                if (currMouseState.LeftButton == ButtonState.Pressed)
                {
                    musicVolumeRectangle.Width = currMouseState.X - musicVolumeRectangle.X;                   
                }
            }

            if (new Rectangle(currMouseState.X, currMouseState.Y, 1, 1).Intersects(new Rectangle(
                soundEVolumeInputRectangle.X, 
                soundEVolumeInputRectangle.Y, 
                soundEVolumeInputRectangle.Width + 1, 
                soundEVolumeInputRectangle.Height)))
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

        #endregion

        #region Draw

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            spriteBatch.DrawString(font3, "Options", new Vector2((game.Window.ClientBounds.Width - font3.MeasureString("Options").X) / 2, 30), fontColor);

            //Ljud
            spriteBatch.DrawString(font, "Sound", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width - 5, 160), fontColor);
            spriteBatch.Draw(menuBox, new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width - 5, 200), Color.SteelBlue);
            spriteBatch.DrawString(font2, "Mute Music", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 30, 230), fontColor);
            muteMusicButton.Draw(spriteBatch);
            spriteBatch.DrawString(font2, "Mute Sound Effects", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 30, 280), fontColor);
            muteSoundEffectsButton.Draw(spriteBatch);

            spriteBatch.DrawString(font2, "Music Volume", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 30, 330), fontColor);
            spriteBatch.Draw(musicVolumeBackgroundTexture, musicVolumeInputRectangle, Color.White);
            spriteBatch.DrawString(font2, musicVolumeRectangle.Width.ToString(), new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 280, 330), Color.White);
            spriteBatch.Draw(musicVolumeTexture, musicVolumeRectangle, Color.White);

            spriteBatch.DrawString(font2, "Sound Effects Volume", new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 30, 380), fontColor);
            spriteBatch.Draw(soundEVolumeBackgroundTexture, soundEVolumeInputRectangle, Color.White);
            spriteBatch.DrawString(font2, soundEVolumeRectangle.Width.ToString(), new Vector2((game.Window.ClientBounds.Width) / 2 - menuBox.Width + 280, 380), Color.White);
            spriteBatch.Draw(soundEVolumeTexture, soundEVolumeRectangle, Color.White);

            //Video
            spriteBatch.DrawString(font, "Video", new Vector2((game.Window.ClientBounds.Width) / 2 + 5, 160), fontColor);
            spriteBatch.Draw(menuBox, new Vector2((game.Window.ClientBounds.Width) / 2 + 5, 200), Color.SteelBlue);
            spriteBatch.DrawString(font2, "Fullscreen", new Vector2((game.Window.ClientBounds.Width) / 2 + 40, 230), fontColor);
            spriteBatch.DrawString(font2, "Vertical Sync", new Vector2((game.Window.ClientBounds.Width) / 2 + 40, 280), fontColor);            
            fullscreenButton.Draw(spriteBatch);
            vsyncButton.Draw(spriteBatch);

            backButton.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        #endregion
    }
}
