using System;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

public sealed class DoorInteract : NetworkBehaviour, IInteractable
{
    [SerializeField] private string _openDescription;
    [SerializeField] private string _closeDescription;
    [SerializeField] private DoorOpenData _doorOpenData;

    private readonly NetworkVariable<bool> _isOpen = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private Tween _moveTween;
    private Tween _rotTween;
    
    public string GetDescription() => _isOpen.Value ? _closeDescription : _openDescription;

    public bool CanInteract(NetworkObject player) => true;

    public void Interact(NetworkObject player)
    {
        if (!IsServer) return;

        _isOpen.Value = !_isOpen.Value;
    }

    public override void OnNetworkSpawn()
    {
        _isOpen.OnValueChanged += OnDoorStateChanged;
        ApplyDoorState(_isOpen.Value, instant: true);
    }

    public override void OnNetworkDespawn()
    {
        _isOpen.OnValueChanged -= OnDoorStateChanged;
    }

    private void OnDoorStateChanged(bool previous, bool current)
    {
        ApplyDoorState(current, instant: false);
    }

    private void ApplyDoorState(bool open, bool instant)
    {
        if (_doorOpenData.Door == null) return;

        _moveTween?.Kill();
        _rotTween?.Kill();

        var tr = _doorOpenData.Door.transform;

        Vector3 pos = open ? _doorOpenData.DoorOpenPosition : _doorOpenData.DoorClosePosition;
        Vector3 rot = open ? _doorOpenData.DoorOpenRotation : _doorOpenData.DoorCloseRotation;

        if (instant)
        {
            tr.localPosition = pos;
            tr.localRotation = Quaternion.Euler(rot);
            return;
        }

        _moveTween = tr.DOLocalMove(pos, 0.5f);
        _rotTween = tr.DOLocalRotate(rot, 0.5f);
    }
}


[Serializable]
public struct DoorOpenData
{
    public GameObject Door;
    public Vector3 DoorOpenPosition;
    public Vector3 DoorOpenRotation;
    public Vector3 DoorClosePosition;
    public Vector3 DoorCloseRotation;
}
