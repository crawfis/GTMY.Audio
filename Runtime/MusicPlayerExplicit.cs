using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GTMY.Audio
{
    public class MusicPlayerExplicit : MonoBehaviour, IMusicPlayer
    {
        [SerializeField]
        private List<AudioClip> soundtracks = new List<AudioClip>();
        [SerializeField]
        private bool shuffleOnLoadAndReplay = true;

        private int soundtrackIndex = 0;
        private MusicController musicController;
        private bool isPlaying = false;
        private IList<int> permutation;
        private readonly System.Random randomGenerator = new System.Random();

        /// <inheritdoc/>
        public float MasterVolume
        {
            get
            {
                return musicController.GlobalVolume;
            }
            set
            {
                if (musicController != null)
                    musicController.GlobalVolume = value;
            }
        }

        /// <inheritdoc/>
        public float Volume
        {
            get
            {
                return musicController.Volume;
            }
            set
            {
                if (musicController != null)
                    musicController.Volume = value;
            }
        }

        /// <inheritdoc/>
        public float FadeTime { get; set; } = 1f;

        private void Awake()
        {
            // Create audio sources to be used in the MusicController.
            // This could be replaced with AudioSources with mixers and other
            // changes to the default AudioSource.
            AudioSource musicSource1 = gameObject.AddComponent<AudioSource>();
            AudioSource musicSource2 = gameObject.AddComponent<AudioSource>();

            // Make sure to enable loop on music sources
            musicSource1.loop = true;
            musicSource2.loop = true;

            musicController = new MusicController(musicSource1, musicSource2, this);

        }

        /// <summary>
        /// Stop all music.
        /// </summary>
        public void Stop()
        {
            musicController.Stop();
            isPlaying = false;
        }

        /// <summary>
        /// Pause the music
        /// </summary>
        public void Pause()
        {
            musicController.Pause();
        }

        /// <inheritdoc/>
        public void UnPause()
        {
            musicController.UnPause();
        }

        /// <inheritdoc/>
        public void Play()
        {
            if (!isPlaying)
                StartCoroutine(PlayAll());
        }

        /// <inheritdoc/>
        public void Shuffle()
        {
            permutation = GTMY.Utility.Shuffle.CreateRandomPermutation(soundtracks.Count, randomGenerator);
        }

        private IEnumerator PlayAll()
        {
            if (!shuffleOnLoadAndReplay && permutation == null)
            {
                permutation = new List<int>(soundtracks.Count);
                for (int i = 0; i < soundtracks.Count; i++)
                {
                    permutation.Add(i);
                }
            }
            while (true)
            {
                if (soundtrackIndex == 0 && shuffleOnLoadAndReplay)
                {
                    Shuffle();
                }
                var newAudioClip = soundtracks[permutation[soundtrackIndex]];
                // Once loaded, play the current sound track
                musicController.PlayMusicWithCrossFade(newAudioClip, FadeTime);
                float clipLength = newAudioClip.length;
                // Wait until the current sound track is almost finished.
                yield return new WaitForSeconds(clipLength - FadeTime);
                soundtrackIndex = (soundtrackIndex + 1) % soundtracks.Count;
            }
        }
    }
}