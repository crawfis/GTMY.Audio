using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// Simple wrapper that handles global and sfx volume controls. This
    /// audio player is intended for 2D sounds on the camera or player position.
    /// The SFX clips are assumed to be short, so no support for stop, pause, ...
    /// </summary>
    public class SfxController2D
    {
        private readonly AudioSource audioSource;
        private float localVolume = 1;
        private float globalVolume = 1;

        internal float GlobalVolume
        {
            get
            {
                return globalVolume;
            }
            set
            {
                globalVolume = Mathf.Clamp(value, 0, 1);
                audioSource.volume = localVolume * globalVolume;
            }
        }

        /// <summary>
        /// Get or set the volume for sound effects (sfx) played through this.
        /// </summary>
        public float Volume
        {
            get
            {
                return localVolume;
            }
            set
            {
                localVolume = Mathf.Clamp(value, 0, 1);
                audioSource.volume = localVolume * globalVolume;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="audioSource">An audio source to use for the sound effects.</param>
        public SfxController2D(AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }

        /// <summary>
        /// Play a sound effect.
        /// </summary>
        /// <param name="sfxClip">The sound effect clip.</param>
        /// <param name="volume">A local volume adjustment.</param>
        public void Play(AudioClip sfxClip, float volume = 1)
        {
            audioSource.PlayOneShot(sfxClip, volume * Volume * GlobalVolume);
        }
    }
}
