namespace GTMY.Audio
{
    /// <summary>
    /// Encapsulates one or more UnityEngine.AudioSource's
    /// </summary>
    public interface IAudio
    {
        /// <summary>
        /// Play the specified clip on this audio source.
        /// </summary>
        /// <param name="clip">The audio clip to Play.</param>
        /// <param name="volumeScale">The volume of the clip.</param>
        void Play(UnityEngine.AudioClip clip, float volumeScale);

        /// <summary>
        /// Stop any clip playing.
        /// </summary>
        void Stop();

        /// <summary>
        /// Specify the position of the the audio source.
        /// </summary>
        /// <param name="position">A local position of the audio source.</param>
        void SetAudioPosition(UnityEngine.Vector3 position);
    }
}