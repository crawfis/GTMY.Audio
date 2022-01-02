using UnityEngine;

namespace GTMY.Audio
{
    public class AudioFactoryPooled : IAudioFactory
    {
        private readonly AudioSourcePooler pool;
        private MonoBehaviour coroutineHandler;

        public AudioFactoryPooled(MonoBehaviour coroutineHandle, AudioSourceGameObjectAdaptor gameAdaptorWithAudioSource)
        {
            this.coroutineHandler = coroutineHandle;
            pool = new AudioSourcePooler(gameAdaptorWithAudioSource, this, coroutineHandler);
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