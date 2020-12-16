namespace GTMY.Audio
{
    /// <summary>
    /// Play and control a specific type of Sfx (collision, gunshot, ...)
    /// </summary>
    public interface ISfxPlayer
    {
        /// <summary>
        /// An indicator of the type of Sfx this player plays
        /// </summary>
        string SfxType { get; }
        /// <summary>
        /// A control for the overall max volume of the device.
        /// </summary>
        float GlobalVolume { get; set;  }

        /// <summary>
        /// The relative max volume of this Sfx. Scaled by GlobalVolume
        /// </summary>
        float LocalVolume { get; set; }

        /// <summary>
        /// Play a Sfx clip associated with this Sfx type.
        /// </summary>
        /// <param name="localVolumeScale">A scale for this instance. Multiplied by both GlobalVolume and LocalVolume.</param>
        void Play(float localVolumeScale);

        /// <summary>
        /// Play a Sfx clip associated with this Sfx type at the specified position.
        /// </summary>
        /// <param name="position">The position of the eminating sound.</param>
        /// <param name="localVolumeScale">A scale for this instance. Multiplied by both GlobalVolume and LocalVolume.</param>
        void PlayAt(UnityEngine.Vector3 position, float localVolumeScale);

        /// <summary>
        /// Stop any clip playing on this player.
        /// </summary>
        void Stop();
    }
}