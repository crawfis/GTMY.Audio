using System.Collections.Generic;
using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// Contains a list of UnityEngine.AudioClip's and supports the IAudioClipProvider interface.
    /// </summary>
    public class AudioClipProvider : IAudioClipProvider, System.IDisposable
    {
        private readonly List<AudioClip> clips = new List<AudioClip>();
        private int clipIndex = 0;
        private IList<int> permutation;
        private bool disposedValue;
        private readonly System.Random randomGenerator;
        private readonly bool shuffleOnLoadAndReplay;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="randomGenerator">A random generator used in the Shuffle method.</param>
        /// <param name="shuffleOnLoadAndReplay">If true, the list of clips is shuffled up front as
        /// well as everytime through the playlist.</param>
        public AudioClipProvider(System.Random randomGenerator, bool shuffleOnLoadAndReplay = true)
        {
            this.randomGenerator = randomGenerator;
            this.shuffleOnLoadAndReplay = shuffleOnLoadAndReplay;
        }

        /// <summary>
        /// Add a clip to the list.
        /// </summary>
        /// <param name="clip">A UnityEngine.AudioClip.</param>
        public void AddClip(AudioClip clip)
        {
            clips.Add(clip);
        }

        /// <inheritdoc/>
        public AudioClip GetNextClip()
        {
            if (clipIndex >= clips.Count)
            {
                clipIndex = 0;
            }
            if (clipIndex == 0 && shuffleOnLoadAndReplay)
            {
                Shuffle();
            }
            return clips[permutation[clipIndex++]];
        }

        /// <inheritdoc/>
        public void Shuffle()
        {
            permutation = GTMY.Utility.Shuffle.CreateRandomPermutation(clips.Count, randomGenerator);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    clips.Clear();
                }

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