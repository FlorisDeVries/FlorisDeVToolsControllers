using System;
using FlorisDeVToolsControllers.Characters.Movement;
using FlorisDeVToolsControllers.Characters.Player;
using FlorisDeVToolsControllers.Input.Camera;
using FlorisDeVToolsUnityExtensions.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Controllers
{
    public class SamplePlayerController : BaseCharacterController
    {
        [Header("Required References")] [SerializeField]
        private CameraDataSo _cameraData;

        [SerializeField] private PlayerMovementPropertiesSo _movementProperties;
        [SerializeField] private Transform _playerRotation;

        [Header("Movement properties")] [ShowInInspector]
        private Vector3 _lateralMovement = Vector3.zero;

        [Header("Dashing properties")] [ShowInInspector]
        private Vector3 _dashDirection = Vector3.zero;

        private float _dashTimer = 0f;
        private Action _onDashComplete;
        private bool _jump = false;
        private float _jumpTimer = 0f;
        private int _jumpCount = 0;

        // For coyote time
        private float _timeSinceLastJumpPress = 0f;
        private float _timeSinceLastGrounded = 0f;
        private const float LandStutterTime = .05f;


        protected override void OnEnable()
        {
            base.OnEnable();

            _dashTimer = _movementProperties.DashDuration;
        }

        public override void SetMoveDirection(Vector2 direction)
        {
            var rotatedMove = direction.Rotate(90 - _cameraData.YRotation);
            _lateralMovement = new Vector3(rotatedMove.x, 0, rotatedMove.y) * _movementProperties.Speed;
        }

        public override void FixedTick()
        {
            JumpLogic();

            var moveDirection = _lateralMovement + DashLogic();
            characterMovement.MoveInDirection(moveDirection);
            characterMovement.FixedTick();
        }

        #region Dashing

        public override void Dash(Action onDashComplete)
        {
            _dashDirection = _playerRotation.forward * _movementProperties.DashSpeed;
            _dashTimer = 0;

            _onDashComplete = onDashComplete;
        }

        private Vector3 DashLogic()
        {
            var dashPercentage = 0f;
            if (_dashTimer < _movementProperties.DashDuration)
            {
                _dashTimer += Time.fixedDeltaTime;
                // Goes from 1f .. 0f
                dashPercentage = _dashTimer / _movementProperties.DashDuration;
            }
            else
            {
                _dashDirection = Vector3.zero;
                _onDashComplete?.Invoke();
            }

            return Vector3.Lerp(_dashDirection, _dashDirection * .1f, dashPercentage);
        }

        #endregion

        #region Jumping
        public override void Jump(bool jump, Action onJumpComplete = null)
        {
            if (!jump)
            {
                _jump = false;
                return;
            }
            
            _timeSinceLastJumpPress = 0;
            
            // Handle air jumps
            if (_jumpCount > 0)
            {
                InitiateJump();
                return;
            }

            if (!characterMovement.IsGrounded)
            {
                CheckGroundedCoyote();
                return;
            }

            InitiateJump();
        }

        private void JumpLogic()
        {
            // Update timers
            _timeSinceLastJumpPress += Time.fixedDeltaTime;

            if (characterMovement.IsGrounded)
            {
                // Sometimes IsGrounded can stutter, quickly swapping true/false 
                if (_timeSinceLastGrounded > LandStutterTime)
                {
                    // Just landed
                    _jumpCount = 0;
                    CheckJumpCoyote();
                }

                _timeSinceLastGrounded = 0;
            }
            else
                _timeSinceLastGrounded += Time.fixedDeltaTime;

            if (!_jump) return;

            characterMovement.SetVerticalInput(_movementProperties.JumpForce);

            _jumpTimer -= Time.fixedDeltaTime;
            if (_jumpTimer < 0)
            {
                _jump = false;
            }
        }

        private void CheckGroundedCoyote()
        {
            if (_timeSinceLastGrounded > _movementProperties.IsGroundedCoyoteTime)
            {
                return;
            }

            InitiateJump();
        }

        private void CheckJumpCoyote()
        {
            if (_timeSinceLastJumpPress > _movementProperties.JumpButtonCoyoteTime)
            {
                return;
            }

            InitiateJump();
        }

        private void InitiateJump()
        {
            if (_jumpCount > _movementProperties.AirJumps)
            {
                return;
            }
            
            _jumpCount++;
            _jump = true;
            _jumpTimer = _movementProperties.MaxJumpDuration;
        }

        #endregion

        public override Vector2 GetHorizontalMovement()
        {
            return new Vector2(_lateralMovement.x, _lateralMovement.y);
        }
    }
}