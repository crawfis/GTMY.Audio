using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// An IAudio (Audio Source) factory that creates a new instance for each request.
    /// </summary>
    public class AudioFactoryPrefab : IAudioFactory
    {
        private readonly AudioSource prefabWithAudioSource;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prefab">The prefab to Instantiate when a new audio source is requested (CreateAusioSource).</param>
        public AudioFactoryPrefab(AudioSource prefab)
        {
            this.prefabWithAudioSource = prefab;
        }

        /// <inheritdoc/>
        public IAudio CreateAudioSource()
        {
            GameObject gameObjectWithAudioSource = GameObject.Instantiate(prefabWithAudioSource.gameObject);
            IAudio audio = new AudioGameObjectAdaptor(gameObjectWithAudioSource);
            return audio;
        }
    }
}