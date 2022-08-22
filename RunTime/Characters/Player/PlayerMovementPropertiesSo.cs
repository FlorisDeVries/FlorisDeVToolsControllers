using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Player
{
    [CreateAssetMenu(fileName = "Player Movement Properties",
        menuName = "Characters/Player/Player Movement Properties")]
    public class PlayerMovementPropertiesSo : ScriptableObject
    {
        [Header("DefaultMovement")]
        [SerializeField] private float _speed = 5.0f;
        
        [Header("Dashing")] 
        [SerializeField] private float _dashDuration = 1f;
        [SerializeField] private float _dashChannelDuration = .2f;
        [SerializeField] private float _dashSpeed = 20f;

        #region Properties
        // Default movement
        public float Speed => _speed;
        
        // Dashing
        public float DashDuration => _dashDuration;
        public float DashSpeed => _dashSpeed;
        
        public Vector2 MoveInput { get; private set; }
        #endregion

        public void SetMoveDirection(Vector2 moveDirection)
        {
            MoveInput = moveDirection;
        }
    }
}