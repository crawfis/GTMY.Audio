using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// Turns a Unity GameObject into an IAudio or creates and adds a GameObject with an AudioSource.
    /// </summary>
    public class AudioGameObjectAdaptor : IAudio
    {
        private readonly AudioSource audioSource;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name to give the GameObject. Default is IAudioWrapper.</param>
        public AudioGameObjectAdaptor(string name = "IAudioWrapper")
        {
            var gameObject = new GameObject(name);
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="gameObject">A UnityEngine.GameObject.</param>
        /// <remarks>If the game object does not contain an AudioSource, one will be added.</remarks>
        public AudioGameObjectAdaptor(GameObject gameObject)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        /// <inheritdoc/>
        public void Play(AudioClip clip, float volumeScale)
        {
            audioSource.clip = clip;
            audioSource.volume = volumeScale;
            audioSource.Play();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            audioSource.Stop();
            audioSource.clip = null;
        }

        /// <inheritdoc/>
        public void SetAudioPosition(Vector3 position)
        {
            audioSource.transform.localPosition = position;
        }
    }
}
