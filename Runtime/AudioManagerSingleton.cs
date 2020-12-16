using System;
using System.Collections.Generic;
using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// This is a singleton pattern to control "some" of the audio in
    /// a game. One should ask why do they want / need a manager for audio.
    /// For our purposes, this will handle all of the player interactions
    /// and music. 
    /// Pausing the game and turning sounds off, controlling the volume levels 
    /// are other great uses of a manager.
    /// </summary>
    public class AudioManagerSingleton : MonoBehaviour
    {
        [SerializeField]
        private MusicPlayerAddressables Music;

        private float globalVolume = 1;
        private float volumeBeforeMuteCalled;
        private bool isMuted = false;
        private Dictionary<string, ISfxAudioPlayer> sfxPlayers = new Dictionary<string, ISfxAudioPlayer>();

        #region Instance
        private static AudioManagerSingleton instance;

        /// <summary>
        /// Get the singleton instance.
        /// </summary>
        public static AudioManagerSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AudioManagerSingleton>();
                    if (instance == null)
                    {
                        instance = new GameObject("AudioManager-Spawned", typeof(AudioManagerSingleton)).GetComponent<AudioManagerSingleton>();
                    }
                }

                return instance;
            }
        }
        #endregion

        public float GlobalVolume
        {
            get
            {
                return globalVolume;
            }
            set
            {
                globalVolume = Mathf.Clamp(value, 0, 1);
                if (globalVolume > 0) isMuted = false;
                AdjustControllerVolumes();
            }
        }

        /// <summary>
        /// Play a sound effect of the specified type.
        /// </summary>
        /// <param name="soundType">A string corresponding to a registered sfx player.</param>
        /// <param name="volumeScale">Value between zero and 1 to scale the relative volume of the player.</param>
        public void PlaySfx(string soundType, float volumeScale = 1)
        {
            if (sfxPlayers.TryGetValue(soundType, out ISfxAudioPlayer sfxPlayer))
            {
                sfxPlayer.Play(volumeScale);
            }
        }

        /// <summary>
        /// Set the global volume to zero and remember the old value for UnMute().
        /// </summary>
        public void Mute()
        {
            if (!isMuted)
            {
                volumeBeforeMuteCalled = globalVolume;
                globalVolume = 0;
                AdjustControllerVolumes();
                isMuted = true;
            }
        }

        /// <summary>
        /// Reset the global volume to what it was when Mute() was called.
        /// </summary>
        public void UnMute()
        {
            if (isMuted)
            {
                globalVolume = volumeBeforeMuteCalled;
                AdjustControllerVolumes();
                isMuted = false;
            }
        }

        private async System.Threading.Tasks.Task Awake()
        {
            // Keep this instance alive
            DontDestroyOnLoad(this.gameObject);

            await UnityEngine.AddressableAssets.Addressables.Initialize().Task;
        }

        private void AdjustControllerVolumes()
        {
            Music.Volume = globalVolume;
            foreach(ISfxAudioPlayer player in sfxPlayers.Values)
            {
                player.GlobalVolume = globalVolume;
            }
        }

        internal void RegisterPlayer(string soundType, ISfxAudioPlayer sfxPlayer)
        {
            if(sfxPlayers.ContainsKey(soundType))
            {
                if (sfxPlayer == null || sfxPlayer == sfxPlayers[soundType]) return;
                else
                {
                    throw new ArgumentException(String.Format("A Sfx Player of type {0} is already registered.", soundType), "soundType");
                }
            }

            sfxPlayers.Add(soundType, sfxPlayer);
        }
    }
}
