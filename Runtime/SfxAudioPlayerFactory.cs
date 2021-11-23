namespace GTMY.Audio
{
    /// <summary>
    /// A Singleton factory to create ISfxAudioPlayer's.
    /// </summary>
    public class SfxAudioPlayerFactory
    {
        /// <summary>
        /// Get the single instance of this class.
        /// </summary>
        public static SfxAudioPlayerFactory Instance { get; private set; } = new SfxAudioPlayerFactory();

        /// <summary>
        /// Create a SFX audio player and associate it with the sfxType keyword.
        /// </summary>
        /// <param name="sfxType">A keyword or phrase used to request this player.</param>
        /// <param name="audioFactory">An audio source factory.</param>
        /// <param name="clipProvider">A instance of IAudioClipProvider that determines which clip to play.</param>
        /// <returns>An audio player. Can be used to bypass the AudioManagerSingleton or ignored.</returns>
        public ISfxAudioPlayer CreateSfxAudioPlayer(string sfxType, IAudioFactory audioFactory, IAudioClipProvider clipProvider)
        {
            ISfxAudioPlayer sfxAudioPlayer = new SfxAudioPlayer(sfxType, clipProvider, audioFactory);
            AudioManagerSingleton.Instance.RegisterAudioPlayer(sfxAudioPlayer);
            return sfxAudioPlayer;
        }

        /// <summary>
        /// Create a SFX audio player and associate it with the sfxType keyword.
        /// </summary>
        /// <param name="sfxType">A keyword or phrase used to request this player.</param>
        /// <param name="audioType">A string reference to an existing audio source factory.</param>
        /// <param name="clipProvider">A instance of IAudioClipProvider that determines which clip to play.</param>
        /// <returns>An audio player. Can be used to bypass the AUdioManagerSingleton or ignored.</returns>
        public ISfxAudioPlayer CreateSfxAudioPlayer(string sfxType, string audioType, IAudioClipProvider clipProvider)
        {
            ISfxAudioPlayer sfxAudioPlayer = new SfxAudioPlayerUsingAudioFactoryRegistry(sfxType, clipProvider, audioType);
            AudioManagerSingleton.Instance.RegisterAudioPlayer(sfxAudioPlayer);
            return sfxAudioPlayer;
        }

        private SfxAudioPlayerFactory()
        {
        }
    }
}