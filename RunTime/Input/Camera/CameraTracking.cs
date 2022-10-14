using Cinemachine;
using FlorisDeVToolsFSM.UnityExtensions;
using FlorisDeVToolsUnityExtensions.HelperFunctions;
using UnityEngine;

namespace FlorisDeVToolsControllers.Input.Camera
{
    public class CameraTracking : GameBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private Transform _trackingTarget;
#pragma warning restore CS0649
        [SerializeField] private Vector3 _offset;

        [Header("Camera Distance")]
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private CinemachineVirtualCamera _vCam = default;
        [SerializeField] private Vector2 _minMaxCameraDistance = new Vector2(1, 10);
        private CinemachineTransposer _followTransposer;

        private void OnEnable()
        {
            _followTransposer = _vCam.GetCinemachineComponent<CinemachineTransposer>();

            _inputHandler.OnZoomEvent += Zoom;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _inputHandler.OnZoomEvent -= Zoom;
        }

        private void Zoom(float zoomValue)
        {
            if (zoomValue == 0 || IsPaused)
                return;

            var offset = _followTransposer.m_FollowOffset * (1f - Mathf.Sign(zoomValue) * .1f);
            
            // Ensure min max following distance
            if (offset.magnitude < _minMaxCameraDistance.x || offset.magnitude > _minMaxCameraDistance.y)
            {
                return;
            }
            
            _followTransposer.m_FollowOffset = offset;
        }

        private void Update()
        {
            if (_trackingTarget == null)
            {
                return;
            }

            transform.position = _trackingTarget.position;
        }
    }
}
