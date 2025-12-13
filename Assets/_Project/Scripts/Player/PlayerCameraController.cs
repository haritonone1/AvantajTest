using UnityEngine;

public sealed class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform playerRoot;

    [SerializeField] private float sensitivity = 2.5f;
    [SerializeField] private float minPitch = -70f;
    [SerializeField] private float maxPitch = 70f;

    private float pitch;
    private bool rotatePlayerYaw = true;

    public void SetRotatePlayerYaw(bool value)
    {
        rotatePlayerYaw = value;
    }

    public void Rotate(Vector2 look)
    {
        float yaw = look.x * sensitivity;
        float pitchDelta = look.y * sensitivity;

        pitch = Mathf.Clamp(pitch - pitchDelta, minPitch, maxPitch);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        if (rotatePlayerYaw)
        {
            playerRoot.Rotate(Vector3.up * yaw, Space.World);
        }
        else
        {
            cameraPivot.Rotate(Vector3.up * yaw, Space.Self);
        }
    }
    
    public void Enable()
    {
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
    }

}