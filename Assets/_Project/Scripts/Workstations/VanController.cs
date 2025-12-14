using Unity.Netcode;
using UnityEngine;

public sealed class VanController : MonoBehaviour, IInputReceiver
{
    [Header("Modules")]
    [SerializeField] private CarMovingModule carMoving;

    [Header("Vehicle")]
    [SerializeField] private Transform vanTransform;

    private WorkstationManager workstationManager;
    private PlayerCameraController cameraController;

    private bool engineOn;

    private Vector3 savedLocalExitOffset;
    private Quaternion savedLocalExitRotation;


    public void AttachPlayer(NetworkObject player)
    {
        workstationManager = player.GetComponent<WorkstationManager>();
        cameraController = player.GetComponentInChildren<PlayerCameraController>(true);

        player.transform.SetParent(vanTransform, true);
    }


    public void DetachPlayer()
    {
        workstationManager = null;
        cameraController = null;
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

        HandleDriving(input);
    }

    private void HandleDriving(IInputSource input)
    {
        float throttle = input.Move.y;
        float steer = input.Move.x;   

        bool brake =
            Mathf.Abs(throttle) < 0.01f;

        carMoving.SetInput(throttle, steer, brake);
    }

    private void EngineOn()
    {
        engineOn = true;
    }

    private void EngineOff()
    {
        engineOn = false;
        carMoving.StopInstant();
    }

    public void OnEnterVan()
    {
        cameraController?.SetRotatePlayerYaw(false);
    }

    public void OnExitVan()
    {
        cameraController?.SetRotatePlayerYaw(true);
    }

    public void PrepareExitTransform(Transform van, Transform player)
    {
        savedLocalExitOffset = van.InverseTransformPoint(player.position);
        savedLocalExitRotation =
            Quaternion.Inverse(van.rotation) * player.rotation;
    }


    public void RestoreExitTransform(Transform van, Transform player)
    {
        player.position = van.TransformPoint(savedLocalExitOffset);
        player.rotation = van.rotation * savedLocalExitRotation;
    }


    private void ExitVehicle()
    {
        EngineOff();
        workstationManager.ExitServerRpc();
    }
}
