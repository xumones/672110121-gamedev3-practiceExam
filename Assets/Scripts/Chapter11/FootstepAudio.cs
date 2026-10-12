using UnityEngine;

namespace Samarnggoon.GameDev3.Chapter11
{
    /// <summary>
    /// Footstep SFX driven by actual ground distance travelled, so the cadence matches the
    /// character's speed instead of a fixed timer. Plays through a single AudioSource with
    /// PlayOneShot and varies pitch/volume per step so repeats do not sound mechanical.
    ///
    /// Attach to the character that has a CharacterController (PlayerArmature).
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class FootstepAudio : MonoBehaviour
    {
        [Header("Clips")]
        [Tooltip("One or more footstep clips; a random, non-repeating one is used per step.")]
        public AudioClip[] footstepClips;

        [Header("Source")]
        public AudioSource footstepSource;
        [Tooltip("Name of the AudioMixer group this should play into.")]
        public string outputGroup = "SFX";

        [Header("Cadence")]
        [Tooltip("Metres of ground travel between steps.")]
        public float stepDistance = 2.1f;
        [Tooltip("Horizontal speed below which the character counts as standing still.")]
        public float moveThreshold = 0.15f;

        [Header("Variation")]
        public float minVolume = 0.25f;
        public float maxVolume = 0.55f;
        public float minPitch = 0.92f;
        public float maxPitch = 1.09f;

        private CharacterController m_Controller;
        private float m_DistanceAccumulated;
        private int m_LastClip = -1;

        private void Awake()
        {
            m_Controller = GetComponent<CharacterController>();

            if (footstepSource == null)
                footstepSource = GetComponent<AudioSource>();
            if (footstepSource == null)
                footstepSource = gameObject.AddComponent<AudioSource>();

            footstepSource.playOnAwake = false;
            footstepSource.loop = false;
            footstepSource.spatialBlend = 0f;      // the listener is the character itself
            if (!string.IsNullOrEmpty(outputGroup))
                footstepSource.outputAudioMixerGroup = AudioMixerLookup.Find(outputGroup);
        }

        private void Update()
        {
            if (m_Controller == null || footstepClips == null || footstepClips.Length == 0)
                return;

            Vector3 velocity = m_Controller.velocity;
            float planarSpeed = new Vector2(velocity.x, velocity.z).magnitude;

            if (!m_Controller.isGrounded || planarSpeed < moveThreshold)
            {
                m_DistanceAccumulated = 0f;
                return;
            }

            m_DistanceAccumulated += planarSpeed * Time.deltaTime;
            if (m_DistanceAccumulated < stepDistance)
                return;

            m_DistanceAccumulated -= stepDistance;
            PlayStep();
        }

        private void PlayStep()
        {
            int index = Random.Range(0, footstepClips.Length);
            if (footstepClips.Length > 1 && index == m_LastClip)
                index = (index + 1) % footstepClips.Length;
            m_LastClip = index;

            footstepSource.pitch = Random.Range(minPitch, maxPitch);
            footstepSource.PlayOneShot(footstepClips[index], Random.Range(minVolume, maxVolume));
        }
    }
}