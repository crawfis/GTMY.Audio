using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// A concrete implemenation of ISfxAudioPlayer
    /// </summary>
    internal class SfxAudioPlayer : ISfxAudioPlayer
    {
        private readonly IAudioClipProvider clipProvider;
        private readonly IAudioFactory audioFactory;
        private IAudio currentAudio;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sfxType">A keyword or phase associated with this audio player.</param>
        /// <param name="clipProvider">The clip provider to use.</param>
        /// <param name="audioFactory">The audio source factory to use each time a clip is played.</param>
        public SfxAudioPlayer(string sfxType, IAudioClipProvider clipProvider, IAudioFactory audioFactory)
        {
            SfxType = sfxType;
            this.clipProvider = clipProvider;
            this.audioFactory = audioFactory;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sfxType">A keyword or phase associated with this audio player.</param>
        /// <param name="clipProvider">The clip provider to use.</param>
        /// <param name="audio">The audio source to use each time a clip is played.</param>
        public SfxAudioPlayer(string sfxType, IAudioClipProvider clipProvider, IAudio audio)
        {
            SfxType = sfxType;
            this.clipProvider = clipProvider;
            this.currentAudio = audio;
        }

        /// <inheritdoc/>
        public string SfxType { get; private set; }

        /// <inheritdoc/>
        public float MasterVolume { get; set; } = 1;

        /// <inheritdoc/>
        public float LocalVolume { get; set; } = 1;

        /// <inheritdoc/>
        public void Play(float localVolumeScale)
        {
            UnityEngine.AudioClip clip = clipProvider.GetNextClip();
            float volumeScale = localVolumeScale * LocalVolume * MasterVolume;
            GetAudio();
            currentAudio?.Play(clip, volumeScale);
        }

        /// <inheritdoc/>
        public void Stop()
        {
            currentAudio?.Stop();
            currentAudio = null;
        }

        /// <inheritdoc/>
        public void PlayAt(Vector3 position, float localVolumeScale)
        {
            GetAudio();
            currentAudio?.SetAudioPosition(position);
            Play(localVolumeScale);
        }

        private void GetAudio()
        {
            currentAudio = audioFactory?.CreateAudioSource();
        }
    }
}