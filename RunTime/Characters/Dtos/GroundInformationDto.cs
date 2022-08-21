using JetBrains.Annotations;
using UnityEngine;

namespace FlorisDeVTools.Characters.Movement.Dtos
{
    public class GroundInformationDto
    {
        public bool IsGrounded { get; private set; }
        public RaycastHit GroundHit { get; private set; }

        public Vector3 SlopeNormal { get; private set; }
        public Vector3 SlopeDownDirection { get; private set; }
        public bool IsOnSlope { get; private set; }
        public float SlopeAngle { get; private set; }

        public bool IsOnKinematicPlatform { get; private set; }
        public Rigidbody KinematicPlatform { get; private set; }
        public Vector3 PlatformPointVelocity { get; private set; }

        public GroundInformationDto()
        {
            SlopeNormal = Vector3.up;
            IsOnSlope = false;
            IsGrounded = false;
            IsOnKinematicPlatform = false;
            SlopeDownDirection = Vector3.back;
            KinematicPlatform = null;
        }
        
        public GroundInformationDto(RaycastHit groundHit, bool hitGround)
        {
            GroundHit = groundHit;
            IsGrounded = hitGround;

            // Slopes
            SlopeNormal = groundHit.normal;
            IsOnSlope = SlopeNormal != Vector3.up;
            SlopeAngle = Vector3.Angle(SlopeNormal, Vector3.up);
            var projectedNormal = Vector3.ProjectOnPlane(SlopeNormal, Vector3.up);
            SlopeDownDirection = Vector3.ProjectOnPlane(projectedNormal, SlopeNormal).normalized;

            // Platforms
            if (GroundHit.rigidbody)
            {
                IsOnKinematicPlatform = hitGround && GroundHit.rigidbody.isKinematic;
                KinematicPlatform = GroundHit.rigidbody;
                PlatformPointVelocity = hitGround ? GroundHit.rigidbody.GetPointVelocity(groundHit.point) : Vector3.zero;
            }
        }
    }
}