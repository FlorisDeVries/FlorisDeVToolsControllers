using UnityEngine;

namespace FlorisDeVToolsControllers.Input.Camera
{
    [CreateAssetMenu(fileName = "CameraDataSo", menuName = "FlorisDeVTools/GameSetup/CameraDataSo")]
    public class CameraDataSo : ScriptableObject
    {
        public float YRotation { get; private set; }

        public void SetYRotation(float rotation)
        {
            YRotation = rotation;
        }
    }
}
