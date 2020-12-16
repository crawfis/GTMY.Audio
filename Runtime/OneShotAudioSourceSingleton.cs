using UnityEngine;

namespace GTMY.Audio
{
    internal class OneShotAudioSourceSingleton : IAudio
    {
        public static OneShotAudioSourceSingleton Instance { get; } = new OneShotAudioSourceSingleton();
        private readonly AudioSource audioSource;

        private OneShotAudioSourceSingleton()
        {
            var gameObject = new UnityEngine.GameObject("OneShotAudioSourceSingleton");
            audioSource = gameObject.AddComponent<UnityEngine.AudioSource>();
        }

        public void Play(AudioClip clip, float volumeScale)
        {
            audioSource.PlayOneShot(clip, volumeScale);
        }

        public void SetAudioPosition(Vector3 position)
        {
            audioSource.transform.localPosition = position;
        }
    }
}
