using UnityEngine;
using UnityEngine.InputSystem;

namespace RavipoteP
{
    public class InteractableItem : MonoBehaviour
    {
        [Header("Item Configuration")]
        [SerializeField] private ItemType itemType = ItemType.ScoreBonus;
        [SerializeField] private AudioClip collectSFX;

        [Header("Animation Settings")]
        [SerializeField] private float rotateSpeed = 90f;
        [SerializeField] private float bobSpeed = 3f;
        [SerializeField] private float bobHeight = 0.25f;

        private Vector3 startPosition;
        private bool isPlayerInRange = false;

        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            // Rotation & Bobbing
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // Interaction input via New Input System
            if (isPlayerInRange)
            {
                var keyboard = Keyboard.current;
                var gamepad = Gamepad.current;

                bool interactPressed = (keyboard != null && keyboard.eKey.wasPressedThisFrame) ||
                                       (gamepad != null && gamepad.buttonWest.wasPressedThisFrame);

                if (interactPressed)
                {
                    Collect();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.GetComponent<CharacterController>() != null)
            {
                isPlayerInRange = true;
                string promptLabel = GetPromptText();
                Stage2HUD.Instance?.ShowInteractionPrompt($"Press [E] to Collect {promptLabel}");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") || other.GetComponent<CharacterController>() != null)
            {
                isPlayerInRange = false;
                Stage2HUD.Instance?.HideInteractionPrompt();
            }
        }

        private void OnDisable()
        {
            if (isPlayerInRange)
            {
                Stage2HUD.Instance?.HideInteractionPrompt();
            }
        }

        public void Collect()
        {
            if (collectSFX != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(collectSFX);
            }

            var stageMgr = Stage2GameManager.Instance;
            if (stageMgr != null)
            {
                switch (itemType)
                {
                    case ItemType.TimeBonus:
                        stageMgr.AddTime(3f);
                        break;
                    case ItemType.TimePenalty:
                        stageMgr.AddTime(-5f);
                        break;
                    case ItemType.ScoreBonus:
                        stageMgr.AddScore(500);
                        break;
                    case ItemType.ScorePenalty:
                        stageMgr.AddScore(-500);
                        break;
                }
            }

            Stage2HUD.Instance?.HideInteractionPrompt();
            Destroy(gameObject);
        }

        private string GetPromptText()
        {
            switch (itemType)
            {
                case ItemType.TimeBonus: return "(+3s Time)";
                case ItemType.TimePenalty: return "(-5s Penalty)";
                case ItemType.ScoreBonus: return "(+500 Pts)";
                case ItemType.ScorePenalty: return "(-500 Pts)";
                default: return "";
            }
        }

        public void SetItemType(ItemType type, AudioClip sfx)
        {
            itemType = type;
            collectSFX = sfx;
        }
    }
}
