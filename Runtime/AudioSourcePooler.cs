using CrawfisSoftware;
using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// IPooler implementation for AudioSource's
    /// </summary>
    public class AudioSourcePooler : PoolerBase<AudioSourceGameObjectAdaptor>
    {
        private readonly IAudioFactory factory;
        private readonly MonoBehaviour coroutineHandler;

        public AudioSourcePooler(AudioSourceGameObjectAdaptor prefab, IAudioFactory factory, MonoBehaviour coroutineHandler, int initialSize = 32, int maxPersistentSize = 1024, bool collectionChecks = false)
            : base(prefab, initialSize, maxPersistentSize, collectionChecks)
        {
            this.factory = factory;
            this.coroutineHandler = coroutineHandler;
        }

        protected override AudioSourceGameObjectAdaptor CreateNewPoolInstance()
        {
            var gameObjectWithAudio = _prefab.GetGameObject();
            var instance = GameObject.Instantiate(gameObjectWithAudio);
            return new AudioSourceGameObjectAdaptor(factory, coroutineHandler, instance);
        }
        protected override void GetPoolInstance(AudioSourceGameObjectAdaptor poolObject) => poolObject.GetGameObject().SetActive(true);
        protected override void ReleasePoolInstance(AudioSourceGameObjectAdaptor poolObject) => poolObject.GetGameObject().SetActive(false);
        protected override void DestroyPoolInstance(AudioSourceGameObjectAdaptor poolObject) => Object.Destroy(poolObject.GetGameObject());
    }
}