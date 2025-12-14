using UnityEngine;

public class WheelRotator : MonoBehaviour
{

    [SerializeField] private WheelCollider _wheelCollider;
    
    private void Update()
    {
        _wheelCollider.GetWorldPose(out var position, out var rotation);
        transform.rotation = rotation;
        transform.position = position;
    }
}