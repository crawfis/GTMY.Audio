Audio Manager ReadMe

This package has a simple music player and Sound Effects player that is controlled by a single manager for volume, mute, etc.

To get things working following these steps:
1) Install this package (which should also install Unity's Addressables package)
   a) Open Package Manager in Unity
   b) Select the + drop down in the top-right corner (Unity 2020.2)
   c) Select Add package from git URL ...
   d) Paste:  https://github.com/crawfis/GTMY.Audio.git 
   e) Click Add
2) Create an Empty Game Object and Add two scripts to it:
   a) Music Player
   b) Audio Manager Singleton
   c) Select the Music Player for the Audio Manager Singleton's field Music Player
3) Create your library of music as an addressable group.
   a) Add a label to your soundtracks of "music". You can also add additional labels for specific uses or genres. These can be controlled in the Music Player Genre field.
   b) SFX clips do not need to be in Addressables yet.

That is it, you can now write scripts to turn music on (play) and off (Stop), control the volume, mute and pause / unpause.  If you want more control over what music is
played or do not want to use Addressables, see the MusicController. Music Player is just a wrapper around MusicController.

Here is a sample test Script:

using GTMY.Audio;
using UnityEngine;

public class AudioManagerTest : MonoBehaviour
{
    public AudioClip sfxClip;

    private void Update()
    {
        const float volumeDelta = 0.1f;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AudioManagerSingleton.Instance.GlobalVolume -= volumeDelta;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            AudioManagerSingleton.Instance.GlobalVolume += volumeDelta;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            AudioManagerSingleton.Instance.Mute();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            AudioManagerSingleton.Instance.UnMute();
        if (Input.GetKeyDown(KeyCode.Alpha5))
            AudioManagerSingleton.Instance.PlayerSFX.Play(sfxClip, 1);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            AudioManagerSingleton.Instance.Music.Play();
        if (Input.GetKeyDown(KeyCode.Alpha7))
            AudioManagerSingleton.Instance.Music.Stop();
        if (Input.GetKeyDown(KeyCode.Alpha8))
            AudioManagerSingleton.Instance.Music.Pause();
        if (Input.GetKeyDown(KeyCode.Alpha9))
            AudioManagerSingleton.Instance.Music.UnPause();
    }
}
