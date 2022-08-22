using FlorisDeVToolsControllers.Characters.Controllers;
using FlorisDeVToolsControllers.Input;
using FlorisDeVToolsFSM;
using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Player.States
{
    public class DashingPlayerState : BaseState<PlayerStateMachine>
    {
        private readonly ICharacterController _characterController;
        private readonly InputHandler _inputHandler;

        public DashingPlayerState(PlayerStateMachine owner, ICharacterController characterController,
            InputHandler inputHandler) : base(owner)
        {
            _characterController = characterController;
            _inputHandler = inputHandler;
        }

        public override void Enter()
        {
            base.Enter();
           
            _characterController.Dash(FinishDash);
            _inputHandler.OnMoveEvent += Move;
        }

        public override void Exit()
        {
            base.Exit();
            
            _inputHandler.OnMoveEvent -= Move;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            _characterController.FixedTick();
        }

        private void FinishDash()
        {
            owner.StateMachine.ChangeState(PlayerState.Default);
        }

        private void Move(Vector2 direction)
        {
            _characterController.SetMoveDirection(direction);
        }
    }
}