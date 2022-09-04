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

        public override void Jump(bool jump, Action onJumpComplete = null)
        {
            if (!jump)
            {
                _jump = false;
                return;
            }

            if (!characterMovement.IsGrounded)
            {
                return;
            }

            _jump = true;
            _jumpTimer = _movementProperties.MaxJumpDuration;
        }

        public override void Dash(Action onDashComplete)
        {
            _dashDirection = _playerRotation.forward * _movementProperties.DashSpeed;
            _dashTimer = 0;

            _onDashComplete = onDashComplete;
        }

        public override void FixedTick()
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

            var dashVelocity = Vector3.Lerp(_dashDirection, _dashDirection * .1f, dashPercentage);
            var moveDirection = _lateralMovement + dashVelocity;

            if (_jump)
            {
                characterMovement.SetVerticalImpulse(_movementProperties.JumpForce);

                _jumpTimer -= Time.fixedDeltaTime;
                if (_jumpTimer < 0)
                {
                    _jump = false;
                }
            }

            characterMovement.MoveInDirection(moveDirection);
            characterMovement.FixedTick();
        }

        public override Vector2 GetHorizontalMovement()
        {
            return new Vector2(_lateralMovement.x, _lateralMovement.y);
        }
    }
}