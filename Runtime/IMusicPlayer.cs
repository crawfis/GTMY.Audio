namespace GTMY.Audio
{
    /// <summary>
    /// Interface for playing music.
    /// </summary>
    public interface IMusicPlayer
    {
        /// <summary>
        /// Get or set the time for the cross-fase between two songs.
        /// </summary>
        float FadeTime { get; set; }

        /// <summary>
        /// Get or set the relative max volume of this music player. Scaled by MasterVolume
        /// </summary>
        float Volume { get; set; }

        /// <summary>
        /// Set the overall max volume of the device.
        /// </summary>
        float MasterVolume { set; }

        /// <summary>
        /// Start playing music.
        /// </summary>
        void Play();

        /// <summary>
        /// Stop playing music.
        /// </summary>
        void Stop();

        /// <summary>
        /// Change the order that songs are played.
        /// </summary>
        void Shuffle();

        /// <summary>
        /// Pause the music.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resume playing if the music is paused.
        /// </summary>
        void UnPause();
    }
}