using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GTMY.Audio
{
    public static class SfxPlayerFactory
    {
        public static ISfxPlayer CreateAndRegisterOneShot2D(string sfxType, IAudioClipProvider clipProvider, IAudioFactory audioSourceFactory)
        {
            IAudio audio = audioSourceFactory.CreateOneShotAudioSource();
            ISfxPlayer sfxPlayer = new SfxOneShotPlayer2D(clipProvider, audio);
            AudioManagerSingleton.Instance.RegisterPlayer(sfxType, sfxPlayer);
            return sfxPlayer;
        }
    }
}