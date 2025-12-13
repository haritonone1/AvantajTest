using Unity.Netcode;
using UnityEngine;

public class VanController : MonoBehaviour, IInputReceiver
{
    [Header("Vehicle")]
    [SerializeField] private Rigidbody vehicleRigidbody;

    private WorkstationManager workstationManager;
    private PlayerCameraController cameraController; // 👈 КАМЕРА КОНКРЕТНОГО ИГРОКА

    private bool engineOn;

    private Transform vanTransform;
    private Vector3 savedLocalExitPosition;
    private Quaternion savedLocalExitRotation;

    // ===== ВЫЗЫВАЕТСЯ WORKSTATION'ОМ =====
    public void AttachPlayer(NetworkObject player)
    {
        workstationManager = player.GetComponent<WorkstationManager>();
        cameraController = player.GetComponentInChildren<PlayerCameraController>(true);
    }

    public void DetachPlayer()
    {
        cameraController = null;
        workstationManager = null;
    }

    public void ReceiveInput(IInputSource input)
    {
        if (cameraController != null)
            cameraController.Rotate(input.Look);

        if (input.Back)
        {
            ExitVehicle();
            return;
        }

        if (!engineOn)
        {
            if (input.Ignition)
                EngineOn();
            return;
        }

        HandleVehicleMovement(input);
    }

    public void OnEnterVan()
    {
        cameraController?.SetRotatePlayerYaw(false);
    }

    public void OnExitVan()
    {
        cameraController?.SetRotatePlayerYaw(true);
    }

    // ===== VEHICLE PLACEHOLDER =====
    private void HandleVehicleMovement(IInputSource input)
    {
        Throttle(input.Move.y);
        Steer(input.Move.x);

        if (input.Move.y == 0)
            Brake();
    }

    public void EngineOn() => engineOn = true;
    public void EngineOff() => engineOn = false;

    public void Throttle(float amount)
    {
        if (amount <= 0f) return;

        vehicleRigidbody.AddForce(
            vehicleRigidbody.transform.forward * amount * 1500f * Time.deltaTime,
            ForceMode.Force
        );
    }

    public void Brake()
    {
        vehicleRigidbody.linearVelocity *= 0.98f;
    }

    public void Steer(float amount)
    {
        vehicleRigidbody.AddTorque(
            Vector3.up * amount * 400f * Time.deltaTime,
            ForceMode.Force
        );
    }

    public void PrepareExitTransform(Transform van, Transform player)
    {
        vanTransform = van;
        savedLocalExitPosition = van.InverseTransformPoint(player.position);
        savedLocalExitRotation = Quaternion.Inverse(van.rotation) * player.rotation;
    }

    private void ExitVehicle()
    {
        EngineOff();
        workstationManager.ExitServerRpc();
    }

    public void RestoreExitTransform(Transform player)
    {
        if (vanTransform == null) return;

        player.position = vanTransform.TransformPoint(savedLocalExitPosition);
        player.rotation = vanTransform.rotation * savedLocalExitRotation;
    }
}
