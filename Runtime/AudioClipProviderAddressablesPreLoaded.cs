using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace GTMY.Audio
{
    public class AudioClipProviderAddressablesPreLoaded : AudioClipProvider
    {
        private readonly List<string> labels;
        private AsyncOperationHandle<IList<IResourceLocation>> addressableAssets;

        public AudioClipProviderAddressablesPreLoaded(IList<string> labels, bool shuffleOnLoadAndReplay = true)
            : this(labels, new System.Random(), shuffleOnLoadAndReplay)
        {
        }
        public AudioClipProviderAddressablesPreLoaded(IList<string> labels, System.Random randomGenerator, bool shuffleOnLoadAndReplay = true)
            : base(randomGenerator, shuffleOnLoadAndReplay)
        {
            this.labels = new List<string>(labels);
            // Load Clips from Addressables. Is it safe to call this in the Constructor.
            // Allow flexibility (and perhaps some errors) by letting the user decide when to load.
        }

        public void LoadAllClips()
        {
            addressableAssets = Addressables.LoadResourceLocationsAsync(labels, Addressables.MergeMode.Intersection);
            addressableAssets.Completed += AddressablesLoading_Completed;
        }

        private void AddressablesLoading_Completed(AsyncOperationHandle<IList<IResourceLocation>> addressHandles)
        {
            if (addressHandles.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var resourceLocation in addressHandles.Result)
                {
                    Addressables.LoadAssetAsync<AudioClip>(resourceLocation).Completed += addressHandle =>
                    {
                        // Todo: Keep track of the handles so we can clean up.
                        // Todo: If we are going to pre-load, then use this as a decorator over
                        // a IClipProvider that contains a list of clips.
                        AudioClip clip = addressHandle.Result;
                        this.AddClip(clip);
                    };
                }
            }
            // I do not think I need the AsyncOperationHandle anymore
            //Addressables.Release(addressHandles);
        }
    }
}