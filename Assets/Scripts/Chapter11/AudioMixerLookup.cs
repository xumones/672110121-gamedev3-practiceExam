using UnityEngine;
using UnityEngine.Audio;

namespace Samarnggoon.GameDev3.Chapter11
{
    /// <summary>
    /// Resolves an AudioMixer group by name at runtime. Resources.Load is the only way to
    /// reach a mixer from a build, so the mixer asset has to sit under a Resources folder.
    /// </summary>
    public static class AudioMixerLookup
    {
        public static AudioMixer Mixer;

        public static AudioMixerGroup Find(string groupName)
        {
            AudioMixer mixer = Mixer;
            if (mixer == null) mixer = Mixer = Resources.Load<AudioMixer>("MainMixer");
            if (mixer == null)
            {
                Debug.LogWarning("[Audio] MainMixer not found. Put MainMixer.mixer inside a Resources folder.");
                return null;
            }

            // FindMatchingGroups does substring matching, so "SFX" also matches "MasterSFX"
            // and the wrong group silently wins. Pull all of them and compare exactly.
            AudioMixerGroup[] all = mixer.FindMatchingGroups(string.Empty);
            foreach (AudioMixerGroup g in all)
                if (g.name == groupName)
                    return g;

            Debug.LogWarning("[Audio] No mixer group named '" + groupName + "'. Available: "
                             + string.Join(", ", System.Array.ConvertAll(all, x => x.name)));
            return null;
        }
    }
}