using System;
using FlorisDeVToolsControllers.Characters.Movement;
using FlorisDeVToolsControllers.Characters.Player;
using FlorisDeVToolsControllers.Input.Camera;
using FlorisDeVToolsUnityExtensions.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Controllers
{
    public class TopdownController : BaseCharacterController
    {
        [SerializeField] private CameraDataSo _cameraData;
        [SerializeField] private PlayerMovementPropertiesSo _movementProperties;
        [SerializeField] private Transform _playerRotation;

        [ShowInInspector] private Vector3 _lateralMovement = Vector3.zero;

        [ShowInInspector] private Vector3 _dashDirection = Vector3.zero;
        private float _dashTimer = 0f;

        private Action _onDashComplete;

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

        public override void Jump()
        {
            throw new System.NotImplementedException();
        }

        public override void Dash(Action onComplete)
        {
            _dashDirection = _playerRotation.forward * _movementProperties.DashSpeed;
            _dashTimer = 0;

            _onDashComplete = onComplete;
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

            characterMovement.MoveInDirection(moveDirection);
            characterMovement.FixedTick();
        }

        public override Vector2 GetHorizontalMovement()
        {
            return new Vector2(_lateralMovement.x, _lateralMovement.y);
        }
    }
}