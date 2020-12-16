using UnityEngine;

namespace GTMY.Audio
{
    internal class SfxOneShotPlayer2D : ISfxPlayer
    {
        private readonly IAudioClipProvider clipProvider;
        private readonly IAudio audio;

        public SfxOneShotPlayer2D(IAudioClipProvider clipProvider, IAudio audio)
        {
            this.clipProvider = clipProvider;
            this.audio = audio;
        }

        public string SfxType { get; }
        public float GlobalVolume { get; set; } = 1;
        public float LocalVolume { get; set; } = 1;

        public void Play(float localVolumeScale)
        {
            UnityEngine.AudioClip clip = clipProvider.GetNextClip();
            float volumeScale = localVolumeScale * LocalVolume * GlobalVolume;
            audio.Play(clip, volumeScale);
        }

        public void Stop()
        {
            // No-op. Oneshot's are assumed to be short enough.
        }

        public void PlayAt(Vector3 position, float localVolumeScale)
        {
            audio.SetAudioPosition(position);
            Play(localVolumeScale);
        }
    }
}