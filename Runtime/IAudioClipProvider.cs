namespace GTMY.Audio
{
    /// <summary>
    /// Provides one or more AudioClip instances on request.
    /// </summary>
    public interface IAudioClipProvider
    {
        /// <summary>
        /// Return the next audio clip according to the classes implementation.
        /// </summary>
        /// <returns>a UnityEngine.AudioClip.</returns>
        UnityEngine.AudioClip GetNextClip();

        /// <summary>
        /// Shuffle the existing audio clips (if supported).
        /// </summary>
        void Shuffle();
    }
}