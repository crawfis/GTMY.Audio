using UnityEngine;

namespace GTMY.Audio
{
    public class AudioFactoryPooled : IAudioFactory
    {
        private readonly AudioSourcePooler pool;
        private MonoBehaviour coroutineHandler;

        public AudioFactoryPooled(MonoBehaviour coroutineHandle, GameObject audioPrefab)
        {
            this.coroutineHandler = coroutineHandle;
            var gameObjectWithAudio = new AudioSourceGameObjectAdaptor(this, coroutineHandler);
            pool = new AudioSourcePooler(gameObjectWithAudio, this, coroutineHandler);
        }
        public IAudio CreateAudioSource()
        {
            return pool.Get();
        }

        public void ReleaseAudioSource(IAudio audio)
        {
            pool.Release(audio as AudioSourceGameObjectAdaptor);
        }
    }
}