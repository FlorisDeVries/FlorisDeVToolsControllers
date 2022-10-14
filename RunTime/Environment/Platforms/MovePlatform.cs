using FlorisDeVToolsFSM.UnityExtensions;
using FlorisDeVToolsMathLibrary;
using UnityEngine;

namespace FlorisDeVToolsControllers.Environment.Platforms
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovePlatform : GameBehaviour
    {
        [SerializeField] private float _moveDuration = 3f;
        [SerializeField] private Vector3 _targetOffset;
        [SerializeField] private Transform _target;

        private Rigidbody _rigidbody;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;

            _startPosition = transform.position;

            if (_target)
            {
                _targetPosition = _target.position;
            }
            else
            {
                _targetPosition = _startPosition + _targetOffset;
            }

        }

        private void FixedUpdate()
        {
            if (IsPaused)
                return;

            var percentage =
                EasingFunction.EaseInOutSine(0, 1, Mathf.PingPong(Time.time, _moveDuration) / _moveDuration);

            var targetPosition = Vector3.Lerp(_startPosition, _targetPosition, percentage);

            _rigidbody.MovePosition(targetPosition);
        }
    }
}