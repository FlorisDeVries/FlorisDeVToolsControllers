using System;
using FlorisDeVToolsControllers.Characters.Movement;
using FlorisDeVToolsUnityExtensions.Extensions;
using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Controllers
{
    public abstract class BaseCharacterController : MonoBehaviour, ICharacterController
    {
        protected ICharacterMovement characterMovement;

        protected virtual void OnEnable()
        {
            Debug.Log("Enabled");
            characterMovement = gameObject.GetInterface<ICharacterMovement>();
            characterMovement.MoveInDirection(Vector3.zero);
        }

        public abstract void SetMoveDirection(Vector2 direction);

        public abstract void Jump();

        public abstract void Dash(Action onComplete);

        public abstract void FixedTick();
        public abstract Vector2 GetHorizontalMovement();
    }
}
