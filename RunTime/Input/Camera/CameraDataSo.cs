using UnityEngine;

namespace FlorisDeVTools.Input.Camera
{
    [CreateAssetMenu(fileName = "CameraDataSo", menuName = "GameSetup/CameraDataSo")]
    public class CameraDataSo : ScriptableObject
    {
        public float YRotation { get; private set; }

        public void SetYRotation(float rotation)
        {
            YRotation = rotation;
        }
    }
}
