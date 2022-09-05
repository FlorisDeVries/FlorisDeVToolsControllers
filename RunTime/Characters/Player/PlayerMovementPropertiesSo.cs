using UnityEngine;

namespace FlorisDeVToolsControllers.Characters.Player
{
    [CreateAssetMenu(fileName = "Player Movement Properties",
        menuName = "FlorisDeVTools/Characters/Player/Player Movement Properties")]
    public class PlayerMovementPropertiesSo : ScriptableObject
    {
        [Header("DefaultMovement")]
        [SerializeField] private float _speed = 5.0f;
        
        [Header("Dashing")] 
        [SerializeField] private float _dashDuration = 1f;
        [SerializeField] private float _dashChannelDuration = .2f;
        [SerializeField] private float _dashSpeed = 20f;
        
        [Header("Jumping")]
        [SerializeField] private float _jumpForce = 10f;
        [Tooltip("For how long the player can keep the jump button pressed to increase jump height")]
        [SerializeField] private float _maxJumpDuration = .2f;
        [SerializeField] private int _airJumps = 1;
        [SerializeField] private float _isGroundedCoyoteTime = .2f;
        [SerializeField] private float _jumpButtonCoyoteTime = .2f;

        #region Properties
        // Default movement
        public float Speed => _speed;
        
        // Dashing
        public float DashDuration => _dashDuration;
        public float DashSpeed => _dashSpeed;
        
        // Jumping
        public float JumpForce => _jumpForce;
        public float MaxJumpDuration => _maxJumpDuration;
        public float IsGroundedCoyoteTime => _isGroundedCoyoteTime;
        public float JumpButtonCoyoteTime => _jumpButtonCoyoteTime;
        public int AirJumps => _airJumps;
        
        public Vector2 MoveInput { get; private set; }
        #endregion

        public void SetMoveDirection(Vector2 moveDirection)
        {
            MoveInput = moveDirection;
        }
    }
}