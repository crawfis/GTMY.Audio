namespace GTMY.Audio
{
    /// <summary>
    /// Wrapper for a factory that returns the same IAudio.
    /// </summary>
    public class AudioFactoryOneShot2D : IAudioFactory
    {
        private readonly IAudio oneShotAudioSource;

        /// <inheritdoc/>
        public IAudio CreateAudioSource()
        {
            return oneShotAudioSource;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AudioFactoryOneShot2D()
        {
            oneShotAudioSource = OneShotAudioSourceSingleton.Instance;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="audioSource">The IAudio to return on all CreateAudioSource calls.</param>
        public AudioFactoryOneShot2D(IAudio audioSource)
        {
            oneShotAudioSource = audioSource;
        }
    }
}