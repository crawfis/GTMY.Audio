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
        public SfxController2D PlayerSFX { get; private set; }

        private float globalVolume = 1;
        private float volumeBeforeMuteCalled;
        private bool isMuted = false;

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

        private void AdjustControllerVolumes()
        {
            Music.GlobalVolume = globalVolume;
            PlayerSFX.GlobalVolume = globalVolume;
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

        private async Task Awake()
        {
            // Keep this instance alive
            DontDestroyOnLoad(this.gameObject);
            // Note: Could expose this to the Unity Editor for customization.
            AudioSource sfxAudioSource = gameObject.AddComponent<AudioSource>();
            PlayerSFX = new SfxController2D(sfxAudioSource);

            await Addressables.Initialize().Task;
        }
    }
}
