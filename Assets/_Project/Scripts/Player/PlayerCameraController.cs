using Unity.Cinemachine;
using UnityEngine;

public sealed class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _camera;
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

    private float yaw;

    public void Rotate(Vector2 look)
    {
        float yawDelta = look.x * sensitivity;
        float pitchDelta = look.y * sensitivity;

        pitch = Mathf.Clamp(pitch - pitchDelta, minPitch, maxPitch);

        if (rotatePlayerYaw)
        {
            playerRoot.Rotate(Vector3.up * yawDelta, Space.World);
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
        else
        {
            yaw += yawDelta;
            cameraPivot.localRotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    
    public void Enable()
    {
        _camera.enabled = true;
        enabled = true;
    }

    public void Disable()
    {
        _camera.enabled = false;
        enabled = false;
    }

}