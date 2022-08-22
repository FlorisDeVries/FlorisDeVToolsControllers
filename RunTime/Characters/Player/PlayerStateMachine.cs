using System.Collections.Generic;
using FlorisDeVToolsControllers.Characters.Controllers;
using FlorisDeVToolsControllers.Characters.Player.States;
using FlorisDeVToolsControllers.Input;
using FlorisDeVToolsFSM;
using FlorisDeVToolsUnityExtensions.Extensions;
using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Player
{
    public class PlayerStateMachine : FsmMonobehaviour<PlayerState, PlayerStateMachine>
    {
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private ICharacterController _characterController;
        
        protected override void OnEnable()
        {
            _characterController = gameObject.GetInterface<ICharacterController>();
            
            base.OnEnable();

            StateMachine.ChangeState(PlayerState.LoadingLevel);
        }

        protected override void InitInvokers()
        {
            StateMachine.ChangeState(PlayerState.Default);
        }
        
        protected override void InitializeStateMachine()
        {
            var statesDefinition = new Dictionary<PlayerState, BaseState<PlayerStateMachine>>
            {
                { PlayerState.LoadingLevel , new EmptyPlayerState(this)},
                { PlayerState.Default , new DefaultPlayerState(this, _characterController, _inputHandler)},
                { PlayerState.Dashing , new DashingPlayerState(this, _characterController, _inputHandler)},
                { PlayerState.Channeling , new EmptyPlayerState(this)},
                { PlayerState.Combat , new EmptyPlayerState(this)}
            };

            StateMachine = new FiniteStateMachine<PlayerState, PlayerStateMachine>(statesDefinition);
        }
    }
}