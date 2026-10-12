using UnityEngine;
using UnityEngine.InputSystem;

namespace RavipoteP
{
    public class PlayerDanceEmote : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_animator == null) return;

            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            if (keyboard.digit1Key.wasPressedThisFrame || keyboard.tKey.wasPressedThisFrame)
            {
                _animator.CrossFadeInFixedTime("TwistDance", 0.2f);
            }
            else if (keyboard.digit2Key.wasPressedThisFrame || keyboard.gKey.wasPressedThisFrame)
            {
                _animator.CrossFadeInFixedTime("ChickenDance", 0.2f);
            }
            else if (keyboard.digit3Key.wasPressedThisFrame || keyboard.bKey.wasPressedThisFrame)
            {
                _animator.CrossFadeInFixedTime("BootyDance", 0.2f);
            }
            else if (keyboard.digit4Key.wasPressedThisFrame || keyboard.hKey.wasPressedThisFrame)
            {
                _animator.CrossFadeInFixedTime("HappyIdle", 0.2f);
            }
        }
    }
}
