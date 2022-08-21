using FlorisDeVToolsMathLibrary;
using UnityEngine;

namespace FlorisDeVTools.Environment.Platforms
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovePlatform : MonoBehaviour
    {
        [SerializeField] private float _moveDuration = 3f;
        [SerializeField] private Vector3 _targetOffset;

        private Rigidbody _rigidbody;
        private Vector3 _startPosition;
        private Vector3 _targetPosition;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;

            _startPosition = transform.position;
            _targetPosition = _startPosition + _targetOffset;
        }

        private void FixedUpdate()
        {
            var percentage =
                EasingFunction.EaseInOutSine(0, 1, Mathf.PingPong(Time.time, _moveDuration) / _moveDuration);

            var targetPosition = Vector3.Lerp(_startPosition, _targetPosition, percentage);

            _rigidbody.MovePosition(targetPosition);
        }
    }
}