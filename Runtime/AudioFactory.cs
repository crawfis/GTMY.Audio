using System;
using System.Collections.Generic;
using UnityEngine;

namespace GTMY.Audio
{
    public class AudioFactory : IAudioFactory
    {
        private IAudio oneShotAudioSource;
        private IAudio default3DAudioSource;
        private Dictionary<string, GameObject> prefabsToInstantiate = new Dictionary<string, GameObject>();

        public static AudioFactory Instance { get; private set; } = new AudioFactory();

        public void SetOneShotAudioSource(IAudio audioSource)
        {
            this.oneShotAudioSource = audioSource;
        }

        public void RegisterPreferredAudioSourcePrefab(string audioDescriptor, GameObject prefabWithAudioSource)
        {
            AudioSource audioSource = prefabWithAudioSource.GetComponent<AudioSource>();
            if (audioSource == null) return;

            if(prefabsToInstantiate.ContainsKey(audioDescriptor))
            {
                //Debug.Log(String.Format("A prefab is already registered with the audioDescriptor {0} in AudioSourceFactory", audioDescriptor));
                throw new ArgumentException(String.Format("A prefab is already registered with the audioDescriptor {0} in AudioSourceFactory", audioDescriptor));
            }

            prefabsToInstantiate[audioDescriptor] = prefabWithAudioSource;
        }

        public AudioFactory()
        {
            oneShotAudioSource = OneShotAudioSourceSingleton.Instance;
            default3DAudioSource = new Simple3DAudioSource();

            
        }
        public IAudio CreateOneShotAudioSource()
        {
            return oneShotAudioSource;
        }
        public IAudio Create3DAudioSource(string audioType)
        {
            if (audioType == "3D")
            {
                return default3DAudioSource;
            }
            else
            {
                var prefab = prefabsToInstantiate[audioType];
                GameObject prefabWithAudioSource = GameObject.Instantiate(prefab);
                IAudio audio = new AudioGameObjectAdaptor(prefabWithAudioSource);
                return audio;
            }
        }
    }
}
