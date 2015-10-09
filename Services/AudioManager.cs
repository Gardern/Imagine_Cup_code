using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ImagineCup2012
{
    public class AudioManager
    {
        private static readonly AudioManager instance = new AudioManager();

        public const string BUY_TECH = "buy_tech";
        public const string CONVERS_END = "convers_end";
        public const string CONVERS_START = "convers_start";
        public const string FETCH_APPLES_AND_CORN = "fetch_apples_and_corn";
        public const string LAB_SOUNDS = "lab_sounds";
        public const string LEVEL1_INGAME = "level1_ingame";
        public const string MAINMENU = "mainmenu";
        public const string MAINMENU_CLICK = "mainmenu_click";
        public const string PICKUP = "pickup";
        public const string PUT_FOOD_GRANARY = "put_food_granary";
        public const string QUEST_GAINED = "quest_gained";
        public const string USE_TOOL_FANTASY = "use_tool_fantasy";
        public const string USE_TOOL_PICKAXE = "use_tool_pickaxe";
        public const string WALK_GRASS = "walk_grass";
        public const string WALK_ROAD = "walk_road";

        private AudioEngine audioEngine;
        private WaveBank wavebank;
        private SoundBank soundBank;

        private List<Sound> sounds;

        private AudioManager()
        {
            sounds = new List<Sound>();
        }

        public static AudioManager getInstance
        {
            get
            {
                return instance;
            }
        }

        public SoundBank getSoundBank()
        {
            return soundBank;
        }

        public void start()
        {
            audioEngine = new AudioEngine("Content/Audio/Imagine Cup 2012 Sound.xgs");
            wavebank = new WaveBank(audioEngine, "Content/Audio/waveBank.xwb");
            soundBank = new SoundBank(audioEngine, "Content/Audio/soundBank.xsb");
        }

        public void update()
        {
            for (int i = 0; i < sounds.Count; )
            {
                Sound sound = sounds.ElementAt(i);
                if (sound.isStopped())
                {
                    sound.stop(AudioStopOptions.Immediate);
                    sounds.RemoveAt(i);
                }
                else
                {
                    ++i;
                }
            }

            audioEngine.Update();
        }

        public void playSound(Sound input, string path)
        {
            sounds.Add(new Sound(soundBank.GetCue(path), input.getName()));
            sounds.ElementAt(sounds.Count - 1).play();
        }

        public void resumeSound(Sound input)
        {
            foreach (Sound sound in sounds)
            {
                if (input.Equals(sound))
                {
                    sound.resume();
                }
            }
        }

        public void pauseSound(Sound input)
        {
            foreach (Sound sound in sounds)
            {
                if (input.Equals(sound))
                {
                    sound.pause();
                }
            }
        }

        public void stopSound(Sound input)
        {
            for (int i = 0; i < sounds.Count; i++)
            {
                if (input.Equals(sounds.ElementAt(i)))
                {
                    sounds.ElementAt(i).stop(AudioStopOptions.Immediate);
                    sounds.RemoveAt(i);
                }
            }
        }

        public bool isStopped(Sound input)
        {
            foreach (Sound sound in sounds)
            {
                if (input.Equals(sound))
                {
                    return sound.isStopped();
                }
            }
            return true;
        }

        public bool isPlaying(Sound input)
        {
            foreach (Sound sound in sounds)
            {
                if (input.Equals(sound))
                {
                    return sound.isPlaying();
                }
            }

            return false;
        }
    }
}