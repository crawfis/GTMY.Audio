using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// A singleton audio source that plays clips using the PlayOneShot method.
    /// </summary>
    public class AudioSource2DOneShotSingleton : IAudio
    {
        private readonly AudioSource audioSource;

        /// <summary>
        /// Get the single instance of this class.
        /// </summary>
        public static AudioSource2DOneShotSingleton Instance { get; } = new AudioSource2DOneShotSingleton();

        private AudioSource2DOneShotSingleton()
        {
            var gameObject = new UnityEngine.GameObject("OneShotAudioSourceSingleton");
            audioSource = gameObject.AddComponent<UnityEngine.AudioSource>();
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
        }
    }
}
