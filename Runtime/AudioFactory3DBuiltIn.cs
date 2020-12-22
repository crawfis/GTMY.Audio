namespace GTMY.Audio
{
    /// <summary>
    /// Factory to return the single instance of a simple 3D Audio source (AudioSourceSimple3D).
    /// </summary>
    public class AudioFactory3DBuiltIn : IAudioFactory
    {
        private readonly IAudio audioSource;

        /// <summary>
        /// Get the singleton instance.
        /// </summary>
        public static AudioFactory3DBuiltIn Instance { get; private set; } = new AudioFactory3DBuiltIn();

        private AudioFactory3DBuiltIn()
        {
            audioSource = new AudioSource3DBuiltIn();
        }

        /// <inheritdoc/>
        public IAudio CreateAudioSource()
        {
            return audioSource;
        }

        /// <inheritdoc/>
        public void ReleaseAudioSource(IAudio audio)
        {
        }
    }
}