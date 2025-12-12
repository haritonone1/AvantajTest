using UnityEngine;

public sealed class PlayerCameraController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform cameraPitch;
    [SerializeField] private GameObject cinemachineCamera;

    [Header("Settings")]
    [SerializeField] private float sensitivity = 2.5f;
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    private float pitch;

    public void Enable()
    {
        cinemachineCamera.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Disable()
    {
        cinemachineCamera.SetActive(false);
    }

    public void Rotate(Vector2 look)
    {
        player.Rotate(Vector3.up * look.x * sensitivity, Space.World);

        pitch -= look.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        cameraPitch.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}