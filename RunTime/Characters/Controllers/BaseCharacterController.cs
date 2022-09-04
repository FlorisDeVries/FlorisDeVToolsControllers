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
            characterMovement = gameObject.GetInterface<ICharacterMovement>();
            if (characterMovement == null)
            {
                Debug.LogWarning($"No CharacterMovement found on gameObject {gameObject.name}. Please assign a character movement component.");
                return;
            }
            
            characterMovement.MoveInDirection(Vector3.zero);
        }

        public abstract void SetMoveDirection(Vector2 direction);

        public abstract void Jump(bool jump, Action onJumpComplete = null);

        public abstract void Dash(Action onDashComplete);

        public abstract void FixedTick();
        public abstract Vector2 GetHorizontalMovement();
    }
}
