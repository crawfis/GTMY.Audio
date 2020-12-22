namespace GTMY.Audio
{
    /// <summary>
    /// A concrete implemenation of ISfxAudioPlayer using an audio factory
    /// obtained from the AudioFactoryRegistry class (singleton instance).
    /// </summary>
    internal class SfxAudioPlayerUsingAudioFactoryRegistry : SfxAudioPlayer
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sfxType">A keyword or phase associated with this audio player.</param>
        /// <param name="clipProvider">The clip provider to use.</param>
        /// <param name="audioType">The name of the registered audio source factory.</param>
        public SfxAudioPlayerUsingAudioFactoryRegistry(string sfxType, IAudioClipProvider clipProvider, string audioType)
            : base(sfxType, clipProvider, AudioFactoryRegistry.Instance.GetAudioFactory(audioType))
        {
        }
    }
}