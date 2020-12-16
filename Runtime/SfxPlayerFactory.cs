namespace GTMY.Audio
{
    /// <summary>
    /// A Singleton factory to create ISfxAudioPlayer's.
    /// </summary>
    public class SfxPlayerFactory
    {
        /// <summary>
        /// Get the single instance of this class.
        /// </summary>
        public static SfxPlayerFactory Instance { get; private set; } = new SfxPlayerFactory();

        /// <summary>
        /// Create a SFX audio player and associate it with the sfxType keyword.
        /// </summary>
        /// <param name="sfxType">A keyword or phrase used to request this player.</param>
        /// <param name="audioType">A string reference to an existing audio source factory.</param>
        /// <param name="clipProvider">A instance of IAudioClipProvider that determines which clip to play.</param>
        /// <returns>An audio player. Can be used to bypass the AUdioManagerSingleton or ignored.</returns>
        public ISfxAudioPlayer CreateSfxPlayer(string sfxType, string audioType, IAudioClipProvider clipProvider)
        {
            IAudioFactory factory = AudioFactoryRegistry.Instance.GetAudioFactory(audioType);
            IAudio audio = factory.CreateAudioSource();
            ISfxAudioPlayer sfxPlayer = new SfxAudioPlayer(clipProvider, audio);
            AudioManagerSingleton.Instance.RegisterPlayer(sfxType, sfxPlayer);
            return sfxPlayer;
        }

        private SfxPlayerFactory()
        {
        }
    }
}