using UnityEngine;
using UnityEngine.InputSystem;

namespace RavipoteP
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class CapsulePlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 8.5f;
        [SerializeField] private float rotationSpeed = 12f;
        [SerializeField] private float jumpHeight = 2.0f;
        [SerializeField] private float gravity = -20f;

        [Header("Audio")]
        [SerializeField] private AudioClip jumpSFX;

        private CharacterController controller;
        private PlayerInput playerInput;
        private InputAction moveAction;
        private InputAction jumpAction;

        private Vector3 velocity;
        private bool isGrounded;
        private Transform cameraTransform;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            playerInput = GetComponent<PlayerInput>();

            // Setup Actions from New Input System
            if (playerInput != null && playerInput.actions != null)
            {
                moveAction = playerInput.actions["Move"];
                jumpAction = playerInput.actions["Jump"];
            }

            if (Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void OnEnable()
        {
            moveAction?.Enable();
            jumpAction?.Enable();
        }

        private void OnDisable()
        {
            moveAction?.Disable();
            jumpAction?.Disable();
        }

        private void Update()
        {
            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Read Vector2 from New Input System Move Action
            Vector2 inputDir = Vector2.zero;
            if (moveAction != null)
            {
                inputDir = moveAction.ReadValue<Vector2>();
            }

            // Calculate movement relative to Camera
            Vector3 moveDir = Vector3.zero;
            if (cameraTransform != null)
            {
                Vector3 forward = cameraTransform.forward;
                Vector3 right = cameraTransform.right;
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();
                moveDir = (forward * inputDir.y + right * inputDir.x).normalized;
            }
            else
            {
                moveDir = new Vector3(inputDir.x, 0f, inputDir.y).normalized;
            }

            // Move character
            controller.Move(moveDir * moveSpeed * Time.deltaTime);

            // Smooth rotation towards movement direction
            if (moveDir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Check Jump from New Input System Jump Action
            bool jumpTriggered = jumpAction != null && jumpAction.triggered;
            if (jumpTriggered && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                if (jumpSFX != null && AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(jumpSFX);
                }
            }

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
