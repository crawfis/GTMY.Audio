using System.Collections.Generic;

namespace GTMY.Utility
{
    /// <summary>
    /// Static class with methods to produce a permutation.
    /// </summary>
    public static class Shuffle
    {
        /// <summary>
        /// Create a permuation of the integers from 0 to N-1.
        /// </summary>
        /// <param name="length">The size of the permutation.</param>
        /// <param name="random">A System.Random instance.</param>
        /// <returns>A list of integers containing the set of numbers from zero to N-1 in a random order.</returns>
        public static IList<int> CreateRandomPermutation(int length, System.Random random)
        {
            // This is the "inside-out" algorithm of Fisher and Yates' shuffle algorithm (Knuth Algorithm P)
            // https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
            int[] permutation = new int[length];
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(i + 1);
                if (index != i)
                    permutation[i] = permutation[index];
                permutation[index] = i;
            }
            return permutation;
        }
    }
}