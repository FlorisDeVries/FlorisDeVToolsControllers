using System;
using FlorisDeVTools.Characters.Movement;
using FlorisDeVTools.Characters.Player;
using FlorisDeVTools.Input.Camera;
using FlorisDeVToolsUnityExtensions.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FlorisDeVTools.Characters.Controllers
{
    public class TopdownController : MonoBehaviour, ICharacterController
    {
        [SerializeField] private CameraDataSo _cameraData;
        [SerializeField] private PlayerMovementPropertiesSo _movementProperties;
        [SerializeField] private Transform _playerRotation;

        [ShowInInspector] private Vector3 _lateralMovement = Vector3.zero;

        [ShowInInspector] private Vector3 _dashDirection = Vector3.zero;
        private float _dashTimer = 0f;

        private Action _onDashComplete;

        private ICharacterMovement _characterMovement;

        private void OnEnable()
        {
            _characterMovement = gameObject.GetInterface<ICharacterMovement>();
            _characterMovement.MoveInDirection(Vector3.zero);
            _dashTimer = _movementProperties.DashDuration;
        }

        public void SetMoveDirection(Vector2 direction)
        {
            var rotatedMove = direction.Rotate(90 - _cameraData.YRotation);
            _lateralMovement = new Vector3(rotatedMove.x, 0, rotatedMove.y) * _movementProperties.Speed;
        }

        public void Jump()
        {
            throw new System.NotImplementedException();
        }

        public void Dash(Action onComplete)
        {
            _dashDirection = _playerRotation.forward * _movementProperties.DashSpeed;
            _dashTimer = 0;

            _onDashComplete = onComplete;
        }

        public void FixedTick()
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

            _characterMovement.MoveInDirection(moveDirection);
            _characterMovement.FixedTick();
        }
    }
}