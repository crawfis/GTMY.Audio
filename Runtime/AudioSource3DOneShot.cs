using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// An audio source that plays clips using the PlayOneShot method.
    /// </summary>
    public class AudioSource3DOneShot : IAudio
    {
        private readonly AudioSource audioSource;

        public AudioSource3DOneShot(AudioSource audioSource)
        {
            this.audioSource = audioSource;
        }

        /// <inheritdoc/>
        public void Play(AudioClip clip, float volumeScale)
        {
            audioSource.PlayOneShot(clip, volumeScale);
        }

        /// <inheritdoc/>
        public void Stop()
        {
            // No-op. OneShot clips are assumed to be short.
        }

        /// <inheritdoc/>
        public void SetAudioPosition(Vector3 position)
        {
            audioSource.transform.localPosition = position;
        }
    }
}