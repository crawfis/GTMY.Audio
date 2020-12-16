using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace GTMY.Audio
{
    /// <summary>
    /// Provide audio clips based on addressable labels or keys.
    /// </summary>
    public class AudioClipProviderAddressablesPreLoaded : AudioClipProvider, System.IDisposable
    {
        private readonly List<string> labels;
        private readonly List<AsyncOperationHandle<AudioClip>> assetHandles = new List<AsyncOperationHandle<AudioClip>>();
        private AsyncOperationHandle<IList<IResourceLocation>> addressableAssets;
        private bool disposedValue;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="labels">A list of keywords which all must be present on the Addressable. For instance {"sfx","fire"}</param>
        /// <param name="shuffleOnLoadAndReplay">If true, the list of clips is shuffled up front as
        /// well as everytime through the playlist.</param>
        public AudioClipProviderAddressablesPreLoaded(IList<string> labels, bool shuffleOnLoadAndReplay = true)
            : this(labels, new System.Random(), shuffleOnLoadAndReplay)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="labels">A list of keywords which all must be present on the Addressable. For instance {"sfx","fire"}</param>
        /// <param name="randomGenerator">A System.Random instance.</param>
        /// <param name="shuffleOnLoadAndReplay">If true, the list of clips is shuffled up front as
        /// well as everytime through the playlist.</param>
        public AudioClipProviderAddressablesPreLoaded(IList<string> labels, System.Random randomGenerator, bool shuffleOnLoadAndReplay = true)
            : base(randomGenerator, shuffleOnLoadAndReplay)
        {
            this.labels = new List<string>(labels);
            // Load Clips from Addressables. Is it safe to call this in the Constructor.
            // Allow flexibility (and perhaps some errors) by letting the user decide when to load.
        }

        /// <summary>
        /// Determines the clips and loads them into memory. This must be called first.
        /// </summary>
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
                        assetHandles.Add(addressHandle);
                    };

                }
            }
            // I do not think I need the AsyncOperationHandle anymore
            //Addressables.Release(addressHandles);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var handle in this.assetHandles)
                    {
                        Addressables.Release<AudioClip>(handle);
                    }
                    Addressables.Release(addressableAssets);
                }
                base.Dispose(disposing);

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~AudioClipProvider()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void System.IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}