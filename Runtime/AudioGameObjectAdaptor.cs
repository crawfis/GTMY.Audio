using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// Turns a Unity GameObject into an IAudio or creates and adds a GameObject with an AudioSource.
    /// </summary>
    internal class AudioGameObjectAdaptor : IAudio
    {
        private readonly AudioSource audioSource;

        public AudioGameObjectAdaptor()
        {
            var gameObject = new GameObject("IAudioWrapper");
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        public AudioGameObjectAdaptor(GameObject gameObject)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if(audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public void Play(AudioClip clip, float volumeScale)
        {
            audioSource.clip = clip;
            audioSource.volume = volumeScale;
            audioSource.Play();
        }

        public void SetAudioPosition(Vector3 position)
        {
            audioSource.transform.localPosition = position;
        }
    }
}
