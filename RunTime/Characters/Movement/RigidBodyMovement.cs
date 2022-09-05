using FlorisDeVToolsControllers.Characters.Dtos;
using FlorisDeVToolsUnityExtensions.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Movement
{
    public class RigidBodyMovement : MonoBehaviour, ICharacterMovement
    {
        [Header("Ground detection")] [SerializeField]
        private float _groundDetectionDistance = .2f;

        [SerializeField] private float _slopeDetectionDistance = .5f;

        [Header("Slopes and Steps")] [SerializeField]
        private float _maxSlopeAngle = 35f;

        [Range(0f, 1f)] [SerializeField] private float _slideControl = .2f;
        [SerializeField] private float _onSteepSlopeTime = .5f;
        [SerializeField] private float _slopeSlideSpeed = 5f;
        [SerializeField] private float _maxSlideSpeed = 20f;

        [Header("Jumping and air control")] 
        [SerializeField] private float _fallMultiplier = 2;
        [SerializeField] private float _jumpRiseFallOff = 1;
        [SerializeField] private float _maxRiseSpeed = 10; 
        [SerializeField] private float _maxFallSpeed = -10; 

        private Rigidbody _rigidbody;
        private Transform _cachedTransform;

        private Vector3 _moveDirection;
        private Vector3 _projectedMoveDirectionOnSlope;

        private float _verticalInput;
        private GroundInformationDto _groundInformation = new();
        private float _slideTimer = 0f;


        #region Info Panel

        [FoldoutGroup("Info")]
        [ReadOnly]
        [ShowInInspector]
        public bool IsGrounded => _groundInformation.IsGrounded;

        [FoldoutGroup("Info")]
        [ReadOnly]
        [ShowInInspector]
        private bool IsOnSlope => _groundInformation.IsOnSlope;

        [FoldoutGroup("Info")]
        [ReadOnly]
        [ShowInInspector]
        private bool IsOnPlatform => _groundInformation.IsOnKinematicPlatform;

        #endregion

        private void OnEnable()
        {
            _cachedTransform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void FixedTick()
        {
            GroundCheck();

            var direction = _moveDirection;
            direction = CalculateSlopeBehaviour(direction);
            direction = PlatformDetection(direction);
            direction = HandleGravity(direction);

            _rigidbody.velocity = direction;
        }

        private void GroundCheck()
        {
            // Check for ground
            if (Physics.Raycast(transform.position + Vector3.up * .01f, Vector3.down, out var groundHit,
                    _groundDetectionDistance))
            {
                _groundInformation = new GroundInformationDto(groundHit, true);
                return;
            }

            // If not found, check for slope
            if (Physics.Raycast(transform.position + Vector3.up * .01f, Vector3.down, out var slopeHit,
                    _slopeDetectionDistance))
            {
                _groundInformation = new GroundInformationDto(slopeHit, false);
                return;
            }

            // If not found we are flying
            _groundInformation = new GroundInformationDto();
        }

        private Vector3 CalculateSlopeBehaviour(Vector3 direction)
        {
            if (!_groundInformation.IsOnSlope)
            {
                _slideTimer = 0f;
                return direction;
            }

            _projectedMoveDirectionOnSlope = Vector3.ProjectOnPlane(direction, _groundInformation.SlopeNormal);
            if (_groundInformation.SlopeAngle < _maxSlopeAngle)
            {
                _slideTimer = 0f;
                return _projectedMoveDirectionOnSlope;
            }

            _slideTimer += Time.fixedDeltaTime;
            var slidePercentage = Mathf.Min(_slideTimer / _onSteepSlopeTime, 1f);
            var slideMovement = Vector3.Lerp(_projectedMoveDirectionOnSlope,
                _groundInformation.SlopeDownDirection * Mathf.Min(_slopeSlideSpeed * _slideTimer, _maxSlideSpeed),
                Mathf.Min(slidePercentage, 1f - _slideControl));

            return slideMovement;
        }

        private Vector3 PlatformDetection(Vector3 direction)
        {
            // Moving platforms
            if (!_groundInformation.IsOnKinematicPlatform)
            {
                return direction;
            }

            return direction + _groundInformation.PlatformPointVelocity;
        }

        private Vector3 HandleGravity(Vector3 direction)
        {
            var currentVelocity = _rigidbody.velocity.y;
            if (_verticalInput <= 0)
            {
                currentVelocity =  Mathf.Min(currentVelocity, _verticalInput);
            }
            float verticalVelocity;
            
            // Override while receiving verticalInput
            if (_verticalInput > 0)
            {
                verticalVelocity = _verticalInput;
                _verticalInput += Physics.gravity.y * _jumpRiseFallOff * Time.fixedDeltaTime;
            }
            else
            {
                verticalVelocity = currentVelocity + (Physics.gravity.y * _fallMultiplier * Time.fixedDeltaTime);
            }

            verticalVelocity = Mathf.Clamp(verticalVelocity, _maxFallSpeed, _maxRiseSpeed);
            
            // var verticalVelocity = Mathf.Min(_rigidbody.velocity.y, 0);
            return direction + new Vector3(0, verticalVelocity, 0);
        }

        public void MoveInDirection(Vector3 direction)
        {
            _moveDirection = direction;
        }

        public void SetVerticalInput(float verticalInput)
        {
            _verticalInput = verticalInput;
        }

        private void OnDrawGizmosSelected()
        {
            if (_cachedTransform == null)
            {
                return;
            }

            // Move Direction
            Gizmos.color = Color.green;
            ExtraGizmos.DrawArrow(_cachedTransform.position, _moveDirection);

            // Move Direction On Slope
            Gizmos.color = Color.red;
            ExtraGizmos.DrawArrow(_cachedTransform.position, _projectedMoveDirectionOnSlope);

            // Slope Normal
            Gizmos.color = Color.blue;
            ExtraGizmos.DrawArrow(_cachedTransform.position, _groundInformation.SlopeNormal);

            // SlopeDown Direction
            Gizmos.color = Color.magenta;
            ExtraGizmos.DrawArrow(_cachedTransform.position, _groundInformation.SlopeDownDirection);

            // SlopeDown Direction
            Gizmos.color = Color.yellow;
            ExtraGizmos.DrawArrow(_cachedTransform.position, Vector3.up * _verticalInput);
        }
    }
}