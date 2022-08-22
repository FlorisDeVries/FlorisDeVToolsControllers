using FlorisDeVToolsControllers.Characters.Controllers;
using FlorisDeVToolsControllers.Input;
using FlorisDeVToolsFSM;
using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Player.States
{
    public class DefaultPlayerState : BaseState<PlayerStateMachine>
    {
        private readonly ICharacterController _characterController;
        private readonly InputHandler _inputHandler;

        public DefaultPlayerState(PlayerStateMachine owner, ICharacterController characterController,
            InputHandler inputHandler) : base(owner)
        {
            _characterController = characterController;
            _inputHandler = inputHandler;
        }

        public override void Enter()
        {
            base.Enter();

            _inputHandler.OnDashEvent += Dash;
            _inputHandler.OnMoveEvent += Move;
        }

        public override void Exit()
        {
            base.Exit();
            _inputHandler.OnDashEvent -= Dash;
            _inputHandler.OnMoveEvent -= Move;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            _characterController.FixedTick();
        }

        private void Move(Vector2 direction)
        {
            _characterController.SetMoveDirection(direction);
        }
        
        private void Dash(bool shouldDash)
        {
            if (!shouldDash)
                return;

            owner.StateMachine.ChangeState(PlayerState.Dashing);
        }
    }
}