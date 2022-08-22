using FlorisDeVToolsMathLibrary;
using UnityEngine;

namespace FlorisDeVToolsControllers.Environment.Platforms
{
    [RequireComponent(typeof(Rigidbody))]
    public class RotatePlatform : MonoBehaviour
    {
        [SerializeField]
        [Range(-360f, 360f)]
        private float _rotationSpeed = 30.0f;

        private Rigidbody _rigidbody;
        private float _angle;
        
        private float angle
        {
            get => _angle;
            set => _angle = MathLibrary.WrapAngle(value);
        }

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.useGravity = false;
        }

        public void FixedUpdate()
        {
            angle += _rotationSpeed * Time.deltaTime;
            
            var rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            _rigidbody.MoveRotation(rotation);
        }
    }
}
