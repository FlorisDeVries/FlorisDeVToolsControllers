using FlorisDeVToolsControllers.Input.Generated;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FlorisDeVToolsControllers.Input
{
    [CreateAssetMenu(fileName = "InputHandler", menuName = "FlorisDeVTools/GameSetup/Input Handler")]
    public class InputHandler : ScriptableObject, FlorisDeVToolsInput.IGameplayActions
    {
        public event UnityAction<Vector2> OnMoveEvent = delegate { };
        public event UnityAction<Vector2> OnMouseScreenMoveEvent = delegate { };

        public event UnityAction<float> OnZoomEvent = delegate { };
        public event UnityAction<float> OnRotateCameraEvent = delegate { };

        public event UnityAction<bool> OnEnableCameraRotationEvent = delegate { };
        public event UnityAction<bool> OnJumpEvent = delegate { };
        public event UnityAction<bool> OnDashEvent = delegate { };
        public event UnityAction<bool> OnAttackEvent = delegate { };

        private FlorisDeVToolsInput _gameInput;
        
        private void OnEnable()
        {
            if (_gameInput == null)
            {
                _gameInput = new FlorisDeVToolsInput();
                _gameInput.Gameplay.SetCallbacks(this);
            }

            // Enable desire input scheme
            _gameInput.Gameplay.Enable();
        }

        private void OnDisable()
        {
            // Disable all input schemes
            _gameInput.Gameplay.Disable();
        }

        #region EventInvokers
        public void OnMove(InputAction.CallbackContext context)
        {
            OnMoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnMouseScreenMove(InputAction.CallbackContext context)
        {
            OnMouseScreenMoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            OnZoomEvent.Invoke(context.ReadValue<float>());
        }

        public void OnRotateCamera(InputAction.CallbackContext context)
        {
            OnRotateCameraEvent.Invoke(context.ReadValue<float>());
        }

        public void OnEnableCameraRotation(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnEnableCameraRotationEvent.Invoke(true);
            }
            else if (context.canceled)
            {
                OnEnableCameraRotationEvent.Invoke(false);
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnJumpEvent.Invoke(true);
            }
            else if (context.canceled)
            {
                OnJumpEvent.Invoke(false);
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAttackEvent.Invoke(true);
            }
            else if (context.canceled)
            {
                OnAttackEvent.Invoke(false);
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnDashEvent.Invoke(true);
            }
            else if (context.canceled)
            {
                OnDashEvent.Invoke(false);
            }
        }
        #endregion
    }
}
