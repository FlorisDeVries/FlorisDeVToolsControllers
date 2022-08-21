using System;
using UnityEngine;

namespace FlorisDeVTools.Characters.Controllers
{
    public interface ICharacterController
    {
        void SetMoveDirection(Vector2 direction);
        void Jump();
        void Dash(Action onComplete);
        void FixedTick();
    }
}
