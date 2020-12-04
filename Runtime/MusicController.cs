using System;
using System.Collections;
using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// This is a minimilistic approach to playing multiple music clips that fade in
    /// and out as we switch from one to the other.
    /// </summary>
    public class MusicController
    {
        private AudioSource musicSource1;
        private AudioSource musicSource2;
        private MonoBehaviour coroutineHandler;
        private bool source1Playing = false;
        private bool crossFading = false;
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
                AdjustAudioVolumes();
            }
        }

        /// <summary>
        /// The constructor takes in two audio sources. This allows for greater
        /// configuration and avoids making this a MonoBehavior. Unfortunately, it
        /// thus requires a MonoBehavior object for Coroutine calls. The use of two audio 
        /// sources is strictly for cross-fading purposes.
        /// </summary>
        /// <param name="musicSource1">An AudioSource</param>
        /// <param name="musicSource2">An AudioSource</param>
        /// <param name="coroutineHandler">A MonoBehaviour instance used for Coroutines.</param>
        public MusicController(AudioSource musicSource1, AudioSource musicSource2, MonoBehaviour coroutineHandler)
        {
            this.musicSource1 = musicSource1;
            this.musicSource2 = musicSource2;
            this.coroutineHandler = coroutineHandler;
        }

        /// <summary>
        /// Get or set the volume for music. It will be adjusted based on 
        /// a global volume control.
        /// </summary>
        public float Volume { 
            get
            {
                return localVolume;
            }
            set
            {
                localVolume = Mathf.Clamp(value,0,1);
                AdjustAudioVolumes();
            }
        }

        /// <summary>
        /// Stop playing music.
        /// </summary>
        public void Stop()
        {
            coroutineHandler.StopAllCoroutines();
            musicSource1.Stop();
            musicSource1.clip = null;
            musicSource2.Stop();
            musicSource2.clip = null;
            source1Playing = false;
        }

        /// <summary>
        /// Pause the music.
        /// </summary>
        public void Pause()
        {
            musicSource1.Pause();
            musicSource2.Pause();
        }

        /// <summary>
        /// Resume playing music.
        /// </summary>
        public void UnPause()
        {
            musicSource1.UnPause();
            musicSource2.UnPause();
        }

        /// <summary>
        /// Play music that can fade in. If currently fading in, this call is ignored.
        /// </summary>
        /// <param name="musicClip">An AudioClip to start playing. Change any parameters
        /// before calling this function.</param>
        /// <param name="fadeTime">The time before the new clip reaches it's maximum volume 
        /// and the old music clip's volume reaches zero.</param>
        /// <remarks>I see no reason for any other functions.</remarks>
        public void PlayMusicWithCrossFade(AudioClip musicClip, float fadeTime = 1.0f)
        {
            if (crossFading) return;
            crossFading = true;
            // Determine which source is active
            AudioSource activeSource = (source1Playing) ? musicSource1 : musicSource2;
            AudioSource newSource = (source1Playing) ? musicSource2 : musicSource1;

            // Swap the source
            source1Playing = !source1Playing;

            // Set the fields of the audio source, then start the coroutine to crossfade
            newSource.clip = musicClip;
            newSource.Play();
            coroutineHandler.StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, fadeTime));
        }

        private void AdjustAudioVolumes()
        {
            musicSource1.volume = Volume * GlobalVolume;
            musicSource2.volume = Volume * GlobalVolume;
        }

        private IEnumerator UpdateMusicWithCrossFade(AudioSource originalSource, AudioSource newSource, float transitionTime)
        {
            for (float t = 0; t <= transitionTime; t+= Time.deltaTime)
            {
                // Note: This is inside the for loop in case Volume or GlobalVolume changes while fading.
                float volumeScale = Volume * GlobalVolume;
                originalSource.volume = (1 - (t / transitionTime)) * volumeScale;
                newSource.volume = (t / transitionTime) * volumeScale;
                yield return null;
            }
            // Correct any over shooting
            originalSource.volume = 0;
            newSource.volume = Volume * GlobalVolume;
            originalSource.Stop();
            crossFading = false;
        }
    }
}
