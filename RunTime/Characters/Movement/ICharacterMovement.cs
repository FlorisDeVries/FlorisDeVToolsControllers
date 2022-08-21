using UnityEngine;

namespace FlorisDeVTools.Characters.Movement
{
    public interface ICharacterMovement
    {
        public void FixedTick();
        void MoveInDirection(Vector3 direction);
    }
}
