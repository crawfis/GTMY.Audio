using System;
using System.Collections;
using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// Turns a Unity GameObject into an IAudio or creates and adds a GameObject with an AudioSource.
    /// </summary>
    public class AudioSourceGameObjectAdaptor : IAudio, IDisposable
    {
        private AudioSource audioSource;
        private IAudioFactory audioFactory;
        private GameObject gameObject;
        private MonoBehaviour coroutineHandler;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="audioFactory">Used to return the audio source when finished.</param>
        /// <param name="coroutineHandler">Used to handle any coroutine calls.</param>
        /// <param name="name">The name to give a new GameObject. Default is IAudioWrapper.</param>
        public AudioSourceGameObjectAdaptor(IAudioFactory audioFactory, MonoBehaviour coroutineHandler, string name = "IAudioWrapper")
            : this(audioFactory, coroutineHandler, new GameObject(name))
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="audioFactory">Used to return the audio source when finished.</param>
        /// <param name="coroutineHandler">Used to handle any coroutine calls.</param>
        /// <param name="gameObject">A UnityEngine.GameObject.</param>
        /// <remarks>If the game object does not contain an AudioSource, one will be added.</remarks>
        public AudioSourceGameObjectAdaptor(IAudioFactory audioFactory, MonoBehaviour coroutineHandler, GameObject gameObject)
        {
            this.audioFactory = audioFactory;
            this.gameObject = gameObject;
            audioSource = gameObject.GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            this.coroutineHandler = coroutineHandler;
        }

        /// <inheritdoc/>
        public void Play(AudioClip clip, float volumeScale)
        {
            audioSource.clip = clip;
            audioSource.volume = volumeScale;
            audioSource.Play();
            coroutineHandler.StartCoroutine(ReleaseAfterPlay());
        }

        private IEnumerator ReleaseAfterPlay()
        {
            yield return new WaitForSeconds(audioSource.clip.length);
            Stop();
        }

        /// <inheritdoc/>
        public void Stop()
        {
            audioSource.Stop();
            audioSource.clip = null;
            audioFactory.ReleaseAudioSource(this);
        }

        /// <inheritdoc/>
        public void SetAudioPosition(Vector3 position)
        {
            audioSource.transform.localPosition = position;
        }

        internal GameObject GetGameObject()
        {
            return gameObject;
        }

        public void Dispose()
        {
            this.gameObject = null;
            this.audioFactory = null;
            this.coroutineHandler = null;
            this.audioSource = null;
        }
    }
}
