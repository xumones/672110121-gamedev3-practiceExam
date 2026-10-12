using UnityEngine;
using UnityEngine.InputSystem;

namespace RavipoteP
{
    public class HumanoidJumpSFX : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField] [Range(0f, 1f)] private float jumpVolume = 0.55f;

        private StarterAssets.ThirdPersonController _controller;
        private PlayerInput _playerInput;
        private InputAction _jumpAction;
        private float _jumpCooldownTimer = 0f;

        private void Awake()
        {
            _controller = GetComponent<StarterAssets.ThirdPersonController>();
            _playerInput = GetComponent<PlayerInput>();
            if (_playerInput != null && _playerInput.actions != null)
            {
                _jumpAction = _playerInput.actions["Jump"];
            }
        }

        private void Update()
        {
            if (_controller == null) return;

            if (_jumpCooldownTimer > 0f)
            {
                _jumpCooldownTimer -= Time.deltaTime;
            }

            // Check if jump was triggered this specific frame
            bool jumpPressed = (_jumpAction != null && _jumpAction.triggered) ||
                               (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame) ||
                               (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);

            // Play only once per jump takeoff when grounded and cooldown passed
            if (jumpPressed && _controller.Grounded && _jumpCooldownTimer <= 0f)
            {
                _jumpCooldownTimer = 0.45f;
                AudioManager.Instance?.PlayJumpSound(jumpVolume);
            }
        }
    }
}
