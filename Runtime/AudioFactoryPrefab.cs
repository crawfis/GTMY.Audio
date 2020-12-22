using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// An IAudio (Audio Source) factory that creates a new instance for each request.
    /// </summary>
    public class AudioFactoryPrefab : IAudioFactory
    {
        private readonly AudioSource prefabWithAudioSource;
        private readonly MonoBehaviour coroutineHandler;

        //private GameObject gameObjectWithAudioSource;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="prefab">The prefab to Instantiate when a new audio source is requested (CreateAusioSource).</param>
        public AudioFactoryPrefab(AudioSource prefab, MonoBehaviour coroutineHandler)
        {
            this.prefabWithAudioSource = prefab;
            this.coroutineHandler = coroutineHandler;
        }

        /// <inheritdoc/>
        public IAudio CreateAudioSource()
        {
            var gameObjectWithAudioSource = GameObject.Instantiate(prefabWithAudioSource.gameObject, prefabWithAudioSource.transform.parent);
            //gameObjectWithAudioSource.transform.parent = prefabWithAudioSource.transform.parent;
            IAudio audio = new AudioSourceGameObjectAdaptor(this, coroutineHandler, gameObjectWithAudioSource);
            audio.SetAudioPosition(gameObjectWithAudioSource.transform.localPosition);
            return audio;
        }

        /// <inheritdoc/>
        public void ReleaseAudioSource(IAudio audio)
        {
            var audioUnity = audio as AudioSourceGameObjectAdaptor;
            GameObject gameObjectWithAudioSource = audioUnity?.GetGameObject();
            GameObject.Destroy(gameObjectWithAudioSource);
        }
    }
}