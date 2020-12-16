namespace GTMY.Audio
{
    /// <summary>
    /// Factory to return the single instance of a simple 3D Audio source (Simple3DAudioSource).
    /// </summary>
    public class AudioFactoryBasic3D : IAudioFactory
    {
        private readonly IAudio audioSource;

        /// <summary>
        /// Get the singleton instance.
        /// </summary>
        public static AudioFactoryBasic3D Instance { get; private set; } = new AudioFactoryBasic3D();

        private AudioFactoryBasic3D()
        {
            audioSource = new Simple3DAudioSource();
        }

        /// <inheritdoc/>
        public IAudio CreateAudioSource()
        {
            return audioSource;
        }
    }
}