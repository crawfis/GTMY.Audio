namespace GTMY.Audio
{
    public interface IAudioFactory
    {
        IAudio Create3DAudioSource(string audioType);
        IAudio CreateOneShotAudioSource();
    }
}