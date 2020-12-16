namespace GTMY.Audio
{
    public class Simple3DAudioSource : IAudio
    {
        private UnityEngine.Vector3 position;
        public void Play(UnityEngine.AudioClip clip, float volumeScale)
        {
            UnityEngine.AudioSource.PlayClipAtPoint(clip, position, volumeScale);
        }

        public void SetAudioPosition(UnityEngine.Vector3 position)
        {
            this.position = position;
        }
    }
}