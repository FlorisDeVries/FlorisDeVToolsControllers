using FlorisDeVToolsFSM;

namespace FlorisDeVToolsControllers.Characters.Player.States
{
    public class EmptyPlayerState : BaseState<PlayerStateMachine>
    {
        public EmptyPlayerState(PlayerStateMachine owner) : base(owner)
        {
        }
    }
}