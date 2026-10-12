using UnityEngine;

namespace RavipoteP
{
    public class ThirdPersonFollowCamera : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Offset Settings")]
        [SerializeField] private Vector3 offset = new Vector3(0f, 4f, -7f);
        [SerializeField] private float lookAtHeight = 1.2f;

        [Header("Follow Smoothing")]
        [SerializeField] private float smoothSpeed = 10f;

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

            Vector3 lookTarget = target.position + Vector3.up * lookAtHeight;
            transform.LookAt(lookTarget);
        }
    }
}
