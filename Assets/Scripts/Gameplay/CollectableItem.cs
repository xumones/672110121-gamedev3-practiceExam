using UnityEngine;

namespace RavipoteP
{
    public enum ItemType
    {
        TimeBonus,    // +3 seconds
        TimePenalty,  // -5 seconds
        ScoreBonus,   // +500 points
        ScorePenalty  // -500 points
    }

    public class CollectableItem : MonoBehaviour
    {
        [Header("Item Configuration")]
        [SerializeField] private ItemType itemType = ItemType.ScoreBonus;
        [SerializeField] private AudioClip collectSFX;

        [Header("Animation Settings")]
        [SerializeField] private float rotateSpeed = 90f;
        [SerializeField] private float bobSpeed = 3f;
        [SerializeField] private float bobHeight = 0.25f;

        private Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.position;
        }

        private void Update()
        {
            // Continuous rotation
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

            // Smooth vertical floating
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.GetComponent<CapsulePlayerController>() != null)
            {
                Collect();
            }
        }

        public void Collect()
        {
            // Play SFX
            if (collectSFX != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(collectSFX);
            }

            // Apply effect to Stage Manager
            var stageMgr = Stage1GameManager.Instance;
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

            Destroy(gameObject);
        }

        public void SetItemType(ItemType type, AudioClip sfx)
        {
            itemType = type;
            collectSFX = sfx;
        }
    }
}
