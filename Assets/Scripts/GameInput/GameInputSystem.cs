using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameInput
{
    public class GameInputSystem : MonoBehaviour
    {
        private const string KeyboardControlScheme = "KeyboardControlScheme";
        private const string MainControlScheme = "MainControlScheme";

        private CanonInput _canonInput;
        private Vector2 _composeInput;
        private Vector2 _zero;

        public event Action<Vector2> AimDeltaChanged;
        public event Action ShotClicked;

        private void Awake()
        {
            _zero = Vector2.zero;
            _canonInput = new CanonInput();

            if (Keyboard.current != null)
            {
                _canonInput.bindingMask = InputBinding.MaskByGroup(KeyboardControlScheme);
            }
            else if (Pointer.current != null)
            {
                Cursor.lockState = CursorLockMode.Locked;
                _canonInput.bindingMask = InputBinding.MaskByGroup(MainControlScheme);
            }
        }

        private void OnEnable()
        {
            _canonInput.Enable();
            _canonInput.Canon.AimDelta.performed += HandleAimInput;
            _canonInput.Canon.AimCompose.performed += HandleAimComposeInput;
            _canonInput.Canon.Shot.performed += HandleShotInput;
        }

        private void Update()
        {
            if (_composeInput == _zero) return;
            AimDeltaChanged?.Invoke(_composeInput);
        }

        private void OnDisable()
        {
            _canonInput.Disable();
            _canonInput.Canon.AimDelta.performed -= HandleAimInput;
            _canonInput.Canon.AimCompose.performed -= HandleAimComposeInput;
            _canonInput.Canon.Shot.performed -= HandleShotInput;
        }

        private void HandleAimInput(InputAction.CallbackContext context)
        {
            AimDeltaChanged?.Invoke(context.ReadValue<Vector2>());
        }

        private void HandleAimComposeInput(InputAction.CallbackContext context)
        {
            _composeInput = context.ReadValue<Vector2>();
        }

        private void HandleShotInput(InputAction.CallbackContext context)
        {
            ShotClicked?.Invoke();
        }
    }
}