using System;
using System.Collections.Generic;

namespace GTMY.Audio
{
    /// <summary>
    /// List of IAudioFactory instances indexed by a string keyword. Singleton pattern.
    /// </summary>
    public class AudioFactoryRegistry
    {
        private Dictionary<string, IAudioFactory> audioFactories = new Dictionary<string, IAudioFactory>();
        private const string oneShotRegistry = "OneShot2D";
        private const string ThreeDRegistry = "3D";

        /// <summary>
        /// Get the single instance.
        /// </summary>
        public static AudioFactoryRegistry Instance { get; private set; } = new AudioFactoryRegistry();

        /// <summary>
        /// Get or set whether a different factory can be registered with an existing keyword, replacing
        /// the original factory.The default is false.
        /// </summary>
        public bool AllowFactoryReplacement { get; set; } = false;

        /// <summary>
        /// Register a new factory.
        /// </summary>
        /// <param name="audioDescriptor">A unique keyword (or phrase) to associate with this audio factory.</param>
        /// <param name="audioFactory">The audio factory to use according to the audioDescriptor.</param>
        /// <remarks>ArgumentException is thrown if the audioDescriptor is not unique and the property AllowFactoryReplacement is true.</remarks>
        public void RegisterAudioFactory(string audioDescriptor, IAudioFactory audioFactory)
        {
            if (!AllowFactoryReplacement && audioFactories.ContainsKey(audioDescriptor))
            {
                throw new ArgumentException(String.Format("An IAudioFactory is already registered with the audioDescriptor {0} in AudioFactoryRegistry", audioDescriptor));
            }

            audioFactories[audioDescriptor] = audioFactory;
        }

        /// <summary>
        /// Return the IAudioFactory associated with a string keyword or phrase.
        /// </summary>
        /// <param name="audioType">A string keyword or phrase.</param>
        /// <returns>An IAudioFactory.</returns>
        public IAudioFactory GetAudioFactory(string audioType)
        {
            if (!audioFactories.TryGetValue(audioType, out IAudioFactory factory))
            {
                throw new ArgumentException(String.Format("There is no factory of type {0} registered.", audioType));
            }
            return factory;
        }

        private AudioFactoryRegistry()
        {
            RegisterDefaultAudioFactories();
        }

        private void RegisterDefaultAudioFactories()
        {
            RegisterAudioFactory(oneShotRegistry, new AudioFactoryOneShot2D());
            RegisterAudioFactory(ThreeDRegistry, AudioFactory3DBuiltIn.Instance);
        }
    }
}