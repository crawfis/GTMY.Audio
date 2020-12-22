using System.Collections.Generic;
using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// A collection of IAudio's that can be randomly selected.
    /// </summary>
    internal class AudioSourceComposite : IAudio
    {
        private readonly IList<IAudio> audioSources;
        private readonly System.Random random;
        IAudio currentAudio;
        private Vector3 position;

        public AudioSourceComposite(IList<IAudio> audioSources, System.Random random = null)
        {
            this.audioSources = audioSources;
            this.random = random;
            if (random == null)
                this.random = new System.Random();
        }

        /// <inheritdoc/>
        public void Play(AudioClip clip, float volumeScale)
        {
            IAudio currentAudio = audioSources[random.Next(audioSources.Count)];
            currentAudio.SetAudioPosition (position);
            currentAudio.Play(clip, volumeScale);
        }

        /// <inheritdoc/>
        public void Stop()
        {
            if (currentAudio != null)
                currentAudio.Stop();
            currentAudio = null;
        }

        /// <inheritdoc/>
        public void SetAudioPosition(Vector3 position)
        {
            this.position = position;
        }
    }
}