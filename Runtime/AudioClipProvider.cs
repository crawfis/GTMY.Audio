using System.Collections.Generic;
using UnityEngine;

namespace GTMY.Audio
{
    public class AudioClipProvider : IAudioClipProvider
    {
        private readonly List<AudioClip> clips = new List<AudioClip>();
        private int clipIndex = 0;
        private IList<int> permutation;
        private readonly System.Random randomGenerator;
        private readonly bool shuffleOnLoadAndReplay;

        public AudioClipProvider(System.Random randomGenerator, bool shuffleOnLoadAndReplay = true)
        {
            this.randomGenerator = randomGenerator;
            this.shuffleOnLoadAndReplay = shuffleOnLoadAndReplay;
        }

        public void AddClip(AudioClip clip)
        {
            clips.Add(clip);
        }

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

        public void Shuffle()
        {
            permutation = GTMY.Utility.Shuffle.CreateRandomPermutation(clips.Count, randomGenerator);
        }
    }
}