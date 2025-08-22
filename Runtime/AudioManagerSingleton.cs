using System;
using System.Collections.Generic;

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
    public class AudioManagerSingleton
    {
        public IMusicPlayer Music { get; private set; }

        private float masterVolume = 1;
        private float volumeBeforeMuteCalled;
        private bool isMuted = false;
        private readonly Dictionary<string, ISfxAudioPlayer> sfxAudioPlayers = new();

        /// <summary>
        /// Get the singleton instance.
        /// </summary>
        public static AudioManagerSingleton Instance { get; } = new AudioManagerSingleton();

        public float MasterVolume
        {
            get
            {
                return masterVolume;
            }
            set
            {
                masterVolume = UnityEngine.Mathf.Clamp(value, 0, 1);
                if (masterVolume > 0) isMuted = false;
                AdjustControllerVolumes();
            }
        }

        public void SetMusicPlayer(IMusicPlayer musicPlayer)
        {
            Music = musicPlayer;
        }

        /// <summary>
        /// Play a sound effect of the specified type.
        /// </summary>
        /// <param name="soundType">A string corresponding to a registered sfx player.</param>
        /// <param name="volumeScale">Value between zero and 1 to scale the relative volume of the player.</param>
        public void PlaySfx(string soundType, float volumeScale = 1)
        {
            if (sfxAudioPlayers.TryGetValue(soundType, out ISfxAudioPlayer sfxAudioPlayer))
            {
                sfxAudioPlayer.Play(volumeScale);
            }
        }

        /// <summary>
        /// Set the global volume to zero and remember the old value for UnMute().
        /// </summary>
        public void Mute()
        {
            if (!isMuted)
            {
                volumeBeforeMuteCalled = masterVolume;
                masterVolume = 0;
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
                masterVolume = volumeBeforeMuteCalled;
                AdjustControllerVolumes();
                isMuted = false;
            }
        }

        /// <summary>
        /// Register an audio player with the specified keyword or phrase.
        /// </summary>
        /// <param name="soundType">A string to associate with this audio player.</param>
        /// <param name="sfxAudioPlayer">An ISfxAudioPlayer.</param>
        public void RegisterAudioPlayer(ISfxAudioPlayer sfxAudioPlayer)
        {
            if (sfxAudioPlayer == null) return;
            string soundType = sfxAudioPlayer.SfxType;
            if (sfxAudioPlayers.ContainsKey(soundType))
            {
                // Trying to re-register. Ignore and forgive.
                if (sfxAudioPlayer == sfxAudioPlayers[soundType]) return;
                // Trying to add a different AudioPlayer associated with the same soundType. Not allowed.
                // Todo: research whether this should replace the existing player for the soundType.
                else
                {
                    //throw new ArgumentException(String.Format("A different Sfx Player of type {0} is already registered.", soundType), "sfxAudioPlayer");
                    sfxAudioPlayers[soundType] = sfxAudioPlayer;
                    return;
                }
            }

            sfxAudioPlayers.Add(soundType, sfxAudioPlayer);
        }

        /// <summary>
        /// Unregister an audio player.
        /// </summary>
        /// <param name="sfxAudioPlayer">The audio player to unregister.</param>
        public void UnregisterAudioPlayer(ISfxAudioPlayer sfxAudioPlayer)
        {
            if (sfxAudioPlayer == null) return;
            string soundType = sfxAudioPlayer.SfxType;
            if (sfxAudioPlayers.ContainsKey(soundType))
            {
                sfxAudioPlayers.Remove(soundType);
            }
        }

        /// <summary>
        /// Clears all audio players from the collection.
        /// </summary>
        /// <remarks>This method removes all sound effect audio players from the internal collection, 
        /// leaving it empty. Use this method to reset or release resources associated with  the audio
        /// players.</remarks>
        public void ClearAudioPlayers()
        {
            sfxAudioPlayers.Clear();
        }

        /// <summary>
        /// Find the audio player associated with the keyword or phrase.
        /// </summary>
        /// <param name="soundType">A keyword string.</param>
        /// <returns>An instance of ISfxAudioPlayer or throws an exception if none is registered with
        /// the passed in string.</returns>
        public ISfxAudioPlayer GetAudioPlayer(string soundType)
        {
            return sfxAudioPlayers[soundType];
        }

        /// <summary>
        /// Removes all registered ISfxAudioPlayers.
        /// </summary>
        public void ClearAll()
        {
            sfxAudioPlayers.Clear();
        }

        private void AdjustControllerVolumes()
        {
            if (Music != null)
                Music.MasterVolume = masterVolume;
            foreach (ISfxAudioPlayer player in sfxAudioPlayers.Values)
            {
                player.MasterVolume = masterVolume;
            }
        }
    }
}