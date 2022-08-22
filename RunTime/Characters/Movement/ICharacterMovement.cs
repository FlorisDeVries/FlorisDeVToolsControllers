using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Movement
{
    public interface ICharacterMovement
    {
        public void FixedTick();
        void MoveInDirection(Vector3 direction);
    }
}
