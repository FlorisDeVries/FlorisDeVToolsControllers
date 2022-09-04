using System;
using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Controllers
{
    public interface ICharacterController
    {
        void SetMoveDirection(Vector2 direction);
        void Jump(bool jump, Action onJumpComplete = null);
        void Dash(Action onDashComplete = null);
        void FixedTick();
        Vector2 GetHorizontalMovement();
    }
}
