using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// A concrete implemenation of ISfxAudioPlayer
    /// </summary>
    internal class SfxAudioPlayer : ISfxAudioPlayer
    {
        private readonly IAudioClipProvider clipProvider;
        private readonly IAudio audio;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="clipProvider">The clip provider to use.</param>
        /// <param name="audio">The audio source to use.</param>
        public SfxAudioPlayer(IAudioClipProvider clipProvider, IAudio audio)
        {
            this.clipProvider = clipProvider;
            this.audio = audio;
        }

        /// <inheritdoc/>
        public string SfxType { get; }

        /// <inheritdoc/>
        public float GlobalVolume { get; set; } = 1;

        /// <inheritdoc/>
        public float LocalVolume { get; set; } = 1;

        /// <inheritdoc/>
        public void Play(float localVolumeScale)
        {
            UnityEngine.AudioClip clip = clipProvider.GetNextClip();
            float volumeScale = localVolumeScale * LocalVolume * GlobalVolume;
            audio.Play(clip, volumeScale);
        }

        /// <inheritdoc/>
        public void Stop()
        {
            audio.Stop();
        }

        /// <inheritdoc/>
        public void PlayAt(Vector3 position, float localVolumeScale)
        {
            audio.SetAudioPosition(position);
            Play(localVolumeScale);
        }
    }
}