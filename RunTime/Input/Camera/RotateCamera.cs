using FlorisDeVToolsUnityExtensions.HelperFunctions;
using UnityEngine;

namespace FlorisDeVToolsControllers.Input.Camera
{
    public class RotateCamera : BetterMonoBehaviour
    {
        [Header("ScriptableObjects")]
        [SerializeField] private InputHandler _inputHandler = default;
        [SerializeField] private CameraDataSo _cameraData = default;

        [Header("Rotation Properties")]
        [SerializeField] private float _acceleration = 5f;
        [SerializeField] private float _deceleration = 5f;
        [SerializeField] private float _rotationSpeed = 5f;


        private bool _rotating = false;
        private float _rotation = 0;
        private float _rotationVelocity = 0;

        private void OnEnable()
        {
            _inputHandler.OnRotateCameraEvent += CursorUpdate;
            _inputHandler.OnEnableCameraRotationEvent += EnableCameraMovement;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _inputHandler.OnRotateCameraEvent -= CursorUpdate;
            _inputHandler.OnEnableCameraRotationEvent -= EnableCameraMovement;
        }

        private void CursorUpdate(float delta)
        {
            _rotation = delta;
        }

        private void EnableCameraMovement(bool enableCameraMovement)
        {
            _rotating = enableCameraMovement;
        }

        private void FixedUpdate()
        {
            if (!_rotating)
            {
                _rotation = 0f;
            }

            _rotationVelocity = Mathf.Abs(_rotationVelocity) < Mathf.Abs(_rotation) ?
                Mathf.Lerp(_rotationVelocity, _rotation, _acceleration * Time.fixedDeltaTime) :
                Mathf.Lerp(_rotationVelocity, _rotation, _deceleration * Time.fixedDeltaTime);

            transform.rotation *= Quaternion.Euler(0, 90f * _rotationVelocity * _rotationSpeed * Time.fixedDeltaTime, 0);

            _cameraData.SetYRotation(transform.localEulerAngles.y);
        }
    }
}
