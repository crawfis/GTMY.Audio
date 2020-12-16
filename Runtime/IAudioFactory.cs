namespace GTMY.Audio
{
    /// <summary>
    /// Factory to return instances of IAudio (audio sources).
    /// </summary>
    public interface IAudioFactory
    {
        /// <summary>
        /// Create and return an audio source.
        /// </summary>
        /// <returns>An instance of an IAudio.</returns>
        IAudio CreateAudioSource();
    }
}