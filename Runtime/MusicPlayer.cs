using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace GTMY.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private List<IResourceLocation> soundtracks = new List<IResourceLocation>();
        [SerializeField] private string musicGenre = string.Empty;
        private int soundtrackIndex = 0;
        private MusicController musicController;
        AsyncOperationHandle<AudioClip> currentOperationHandle;
        AsyncOperationHandle<AudioClip> oldOperationHandle;

        public float GlobalVolume
        {
            get
            {
                return musicController.GlobalVolume;
            }
            set
            {
                if(musicController != null)
                    musicController.GlobalVolume = value;
            }
        }

        public float FadeTime { get; set; } = 1f;

        public void Awake()
        {
            // Create audio sources to be used in the MusicController.
            // This could be replaced with AudioSources with mixers and other
            // changes to the default AudioSource.
            AudioSource musicSource1 = gameObject.AddComponent<AudioSource>();
            AudioSource musicSource2 = gameObject.AddComponent<AudioSource>();

            // Make sure to enable loop on music sources
            musicSource1.loop = true;
            musicSource2.loop = true;

            musicController = new MusicController(musicSource1, musicSource2, this);

        }
        private void Start()
        {
            LoadMusicAddressesAsync(musicGenre);
        }

        private AsyncOperationHandle<IList<IResourceLocation>> addressableAssets;
        private void LoadMusicAddressesAsync(string musicGenre)
        {
            var addressableLabels = new List<string>() { "music" };
            if (musicGenre != null && musicGenre != String.Empty)
                addressableLabels.Add(musicGenre);
            addressableAssets = Addressables.LoadResourceLocationsAsync(addressableLabels.ToArray(), Addressables.MergeMode.Intersection);
            addressableAssets.Completed += SaveClipAddresses;
        }

        private void SaveClipAddresses(AsyncOperationHandle<IList<IResourceLocation>> addressHandles)
        {
            if (addressHandles.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var resourceLocation in addressHandles.Result)
                {
                    soundtracks.Add(resourceLocation);
                }
            }
            // I do not think I need the AsyncOperationHandle anymore
            //Addressables.Release(addressHandles);
        }

        public void Stop()
        {
            StopAllCoroutines();
            musicController.Stop();
            // Bug: IsValid is false if loading I think. Or release does not work.
            if (currentOperationHandle.IsValid())
            {
                Addressables.Release<AudioClip>(currentOperationHandle);
            }
            // Bug: IsValid is false if loading I think.
            if (oldOperationHandle.IsValid())
            {
                Addressables.Release<AudioClip>(oldOperationHandle);
            }
            isPlaying = false;
        }

        public void Pause()
        {
            musicController.Pause();
        }

        public void UnPause()
        {
            musicController.UnPause();
        }

        private bool isPlaying = false;
        public void Play()
        {
            if(!isPlaying)
                StartCoroutine(PlayAll());
        }

        //public void PlayNext()
        //{

        //}

        IEnumerator PlayAll()
        {
            if (soundtracks.Count <= 0) yield break;

            if (soundtracks.Count == 1)
            {
                isPlaying = true;
                yield return PlaySingleSoundtrack();
            }
            else if(soundtracks.Count == 2)
            {
                isPlaying = true;
                yield return PlayTwoSoundtracks();
            }
            else if(soundtracks.Count > 2)
            {
                isPlaying = true;
                yield return PlayManySoundtracks();
            }
        }

        private IEnumerator PlaySingleSoundtrack()
        {
            currentOperationHandle = Addressables.LoadAssetAsync<AudioClip>(soundtracks[0]);
            yield return currentOperationHandle;
            AudioClip newAudioClip = currentOperationHandle.Result;
            musicController.PlayMusicWithCrossFade(newAudioClip, FadeTime);
        }

        private IEnumerator PlayTwoSoundtracks()
        {
            var currentOperationHandle = Addressables.LoadAssetAsync<AudioClip>(soundtracks[0]);
            yield return currentOperationHandle;
            AudioClip audioClip1 = currentOperationHandle.Result;
            oldOperationHandle = Addressables.LoadAssetAsync<AudioClip>(soundtracks[1]);
            yield return oldOperationHandle;
            AudioClip audioClip2 = oldOperationHandle.Result;
            var newAudioClip = audioClip1;
            while (true)
            {
                // Once loaded, play the current sound track
                musicController.PlayMusicWithCrossFade(newAudioClip, FadeTime);
                float clipLength = newAudioClip.length;
                // Pre-load the next sound track.
                newAudioClip = newAudioClip == audioClip1 ? audioClip2 : audioClip1;

                // Wait until the current sound track is almost finished.
                yield return new WaitForSeconds(clipLength - FadeTime);
            }
        }

        private IEnumerator PlayManySoundtracks()
        {
            currentOperationHandle = Addressables.LoadAssetAsync<AudioClip>(soundtracks[soundtrackIndex]);
            oldOperationHandle = currentOperationHandle;
            yield return currentOperationHandle;
            AudioClip newAudioClip = currentOperationHandle.Result;
            bool firstTrack = true;
            while (true)
            {
                // Once loaded, play the current sound track
                musicController.PlayMusicWithCrossFade(newAudioClip, FadeTime);
                float clipLength = newAudioClip.length;
                float elapsedTime = Time.deltaTime;

                // Free up the memory for the fading out sound track (once finished)
                if (!firstTrack && soundtracks.Count > 2)
                {
                    yield return new WaitForSeconds(FadeTime);
                    Addressables.Release(oldOperationHandle);
                }
                // Pre-load the next sound track.
                if (firstTrack || soundtracks.Count > 2)
                {
                    soundtrackIndex = (soundtrackIndex + 1) % soundtracks.Count;
                    oldOperationHandle = currentOperationHandle;
                    currentOperationHandle = Addressables.LoadAssetAsync<AudioClip>(soundtracks[soundtrackIndex]);
                    yield return currentOperationHandle;
                    newAudioClip = currentOperationHandle.Result;
                }
                firstTrack = false;

                // Wait until the current sound track is almost finished.
                elapsedTime = Time.deltaTime - elapsedTime;
                yield return new WaitForSeconds(clipLength - FadeTime - elapsedTime);
            }
        }
    }
}