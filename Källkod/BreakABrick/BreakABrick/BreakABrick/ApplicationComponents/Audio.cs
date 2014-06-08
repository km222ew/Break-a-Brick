using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace BreakABrick.ApplicationComponents
{
    class Audio
    {
        private static AudioEngine audioEngine;
        private static WaveBank waveBank;
        private static SoundBank soundBank;
        private static AudioCategory music;
        private static AudioCategory soundEffects;

        //Volym, 1 = orginal, 0.5 = halv etc
        private static float musicVolume = 1.0f;
        private static float soundEffectsVolume = 1.0f;

        //Tillgång till alla ljud
        public static SoundBank SoundBank
        {
            get { return soundBank; }
        }

        //För att kunna köra audioEngine.Update();
        public static AudioEngine AudioEngine
        {
            get { return audioEngine; }
        }

        //Ändrar volymen på musiken
        public static float MusicVolume
        {
            get { return musicVolume; }
            set 
            {
                musicVolume = value;
                music.SetVolume(musicVolume);
            }
        }

        //Ändrar volymen på ljudeffekter
        public static float SoundEffectsVolume
        {
            get { return musicVolume; }
            set
            {
                soundEffectsVolume = value;
                soundEffects.SetVolume(soundEffectsVolume);
            }
        }

        static Audio()
        {
            //Ladda in ljud-resurser
            audioEngine = new AudioEngine("Content/Audio/sounds.xgs");
            waveBank = new WaveBank(audioEngine, "Content/Audio/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/Audio/Sound Bank.xsb");

            //Kategori på ljud i xact-projektet
            music = audioEngine.GetCategory("Music");
            soundEffects = audioEngine.GetCategory("Sound Effects");
        }               
    }
}
