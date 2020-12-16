using UnityEngine;

namespace GTMY.Audio
{
    /// <summary>
    /// Simple implementation of IAudioClipProvider that encapsulates a single audio clip
    /// </summary>
    public class AudioClipProviderSingleInstance : AudioClipProvider
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="audioClip">The audio clip to return on all GetNextClip calls.</param>
        public AudioClipProviderSingleInstance(AudioClip audioClip) : base(null)
        {
            this.AddClip(audioClip);
        }
    }
}