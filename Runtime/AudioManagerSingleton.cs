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
        #region Instance
        private static AudioManagerSingleton instance;
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
            set
            {
                instance = value;
            }
        }
        #endregion

        [SerializeField]
        public MusicPlayer Music;

        private float globalVolume = 1;
        private float volumeBeforeMuteCalled;
        private bool isMuted = false;
        private Dictionary<string, ISfxPlayer> sfxPlayers = new Dictionary<string, ISfxPlayer>();

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
            Music.GlobalVolume = globalVolume;
            foreach(ISfxPlayer player in sfxPlayers.Values)
            {
                player.GlobalVolume = globalVolume;
            }
        }

        internal void RegisterPlayer(string soundType, ISfxPlayer sfxPlayer)
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

        public void PlaySfx(string soundType, float volumeScale = 1)
        {
            if (sfxPlayers.TryGetValue(soundType, out ISfxPlayer sfxPlayer))
            {
                sfxPlayer.Play(volumeScale);
            }
        }
    }
}
