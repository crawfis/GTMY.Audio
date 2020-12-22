namespace GTMY.Audio
{
    /// <summary>
    /// Wrapper for a factory that returns the same IAudio.
    /// </summary>
    public class AudioFactoryOneShot2D : IAudioFactory
    {
        private readonly IAudio oneShotAudioSource;

        /// <summary>
        /// Constructor.
        /// </summary>
        public AudioFactoryOneShot2D()
        {
            oneShotAudioSource = AudioSource2DOneShotSingleton.Instance;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="audioSource">The IAudio to return on all CreateAudioSource calls.</param>
        public AudioFactoryOneShot2D(IAudio audioSource)
        {
            oneShotAudioSource = audioSource;
        }

        /// <inheritdoc/>
        public IAudio CreateAudioSource()
        {
            return oneShotAudioSource;
        }

        /// <inheritdoc/>
        public void ReleaseAudioSource(IAudio audio)
        {
        }
    }
}