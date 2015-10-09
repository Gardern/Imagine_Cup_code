using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace ImagineCup2012
{
    public class Sound
    {
        private Cue cue;
        private string name;

        public Sound(string name)
            : this(null, name)
        {

        }

        public Sound(Cue cue, string name)
        {
            this.cue = cue;
            this.name = name;
        }

        public Cue getCue()
        {
            return cue;
        }

        public string getName()
        {
            return name;
        }

        public void play()
        {
            cue.Play();
        }

        public void resume()
        {
            if (cue.IsPaused)
            {
                cue.Resume();
            }
        }

        public void pause()
        {
            if (!cue.IsPaused)
            {
                cue.Pause();
            }
        }

        public void stop(AudioStopOptions options)
        {
            cue.Stop(options);
        }

        public bool isStopped()
        {
            return cue.IsStopped;
        }

        public bool isPlaying()
        {
            return cue.IsPlaying;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null) return false;

            Sound sound = obj as Sound;
            if ((Object)sound == null) return false;

            if (sound == this) return true;

            return name.Equals(sound.name);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}