using System;
using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Controllers
{
    public interface ICharacterController
    {
        void SetMoveDirection(Vector2 direction);
        void Jump();
        void Dash(Action onComplete);
        void FixedTick();
        Vector2 GetHorizontalMovement();
    }
}
