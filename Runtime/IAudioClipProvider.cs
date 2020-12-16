namespace GTMY.Audio
{
    public interface IAudioClipProvider
    {
        UnityEngine.AudioClip GetNextClip();
        void Shuffle();
    }
}