namespace GTMY.Audio
{
    /// <summary>
    /// Uses UnityEngine.AudiOClip.PlayClipAtPoint to play a 3D sound.
    /// </summary>
    /// <remarks>Stop() is not supported.</remarks>
    public class Simple3DAudioSource : IAudio
    {
        private UnityEngine.Vector3 position;

        /// <inheritdoc/>
        public void Play(UnityEngine.AudioClip clip, float volumeScale)
        {
            UnityEngine.AudioSource.PlayClipAtPoint(clip, position, volumeScale);
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public void Stop()
        {
            // No-op
        }

        /// <inheritdoc/>
        public void SetAudioPosition(UnityEngine.Vector3 position)
        {
            this.position = position;
        }
    }
}