using UnityEngine;
using UnityEngine.Audio;

namespace Samarnggoon.GameDev3.Chapter11
{
    /// <summary>
    /// Chapter 11.7.2 - stores the volume settings so they persist between play sessions.
    /// Create via Assets > Create > GameDev3 > Chapter11 > SoundSettingsPreset.
    /// </summary>
    [CreateAssetMenu(menuName = "GameDev3/Chapter11/SoundSettingsPreset", fileName = "SoundSettingsPreset")]
    public class SoundSettings : ScriptableObject
    {
        public AudioMixer AudioMixer;

        [Header("MasterVolume")]
        public string MasterVolumeName = "MasterVolume";
        [Range(-80, 20)]
        public float MasterVolume;

        [Header("MusicVolume")]
        public string MusicVolumeName = "MusicVolume";
        [Range(-80, 20)]
        public float MusicVolume;

        [Header("MasterSFXVolume")]
        public string MasterSFXVolumeName = "MasterSFXVolume";
        [Range(-80, 20)]
        public float MasterSFXVolume;

        [Header("SFXVolume")]
        public string SFXVolumeName = "SFXVolume";
        [Range(-80, 20)]
        public float SFXVolume;

        [Header("UIVolume")]
        public string UIVolumeName = "UIVolume";
        [Range(-80, 20)]
        public float UIVolume;
    }
}