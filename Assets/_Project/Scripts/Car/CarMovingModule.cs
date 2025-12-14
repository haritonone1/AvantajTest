using System.Collections.Generic;
using UnityEngine;

public sealed class CarMovingModule : MonoBehaviour
{
    public float Speed => speed;

    [Header("Wheels")]
    [SerializeField] private List<WheelCollider> wheelColliders = new();
    [SerializeField] private int frontWheelsCount = 2;

    [Header("Drive")]
    [SerializeField] private float motorPower = 500f;
    [SerializeField] private float brakePower = 5000f;
    [SerializeField] private float maxSteerAngle = 45f;
    [SerializeField] private float steerSmooth = 6f;

    [Header("Engine")]
    [SerializeField] private float speedMultiplier = 1f;

    private float speed;
    private float engineRPM;
    private int currentGear = 1;

    private readonly float[] gearSpeedLimits = { 0f, 5f, 10f, 15f, 80f, 120f };

    private float currentSteerAngle;

    private float throttle;
    private float steer;
    private bool brake;

    public void SetInput(float throttle, float steer, bool brake)
    {
        this.throttle = Mathf.Clamp(throttle, -1f, 1f);
        this.steer = Mathf.Clamp(steer, -1f, 1f);
        this.brake = brake;
    }

    private void FixedUpdate()
    {
        UpdateSpeed();
        ApplyMotor();
        ApplySteering();
        ApplyBrakes();
    }

    private void UpdateSpeed()
    {
        if (wheelColliders.Count == 0) return;

        float rpm = wheelColliders[0].rpm;
        float radiansPerSecond = rpm * 6f * Mathf.Deg2Rad;
        float metersPerSecond = radiansPerSecond * wheelColliders[0].radius;

        speed = Mathf.Lerp(speed, metersPerSecond * 3.6f, Time.fixedDeltaTime * 10f);
    }

    private void ApplyMotor()
    {
        float torque = throttle * motorPower * speedMultiplier;

        foreach (var wheel in wheelColliders)
            wheel.motorTorque = torque;
    }

    private void ApplySteering()
    {
        float targetAngle = steer * maxSteerAngle;
        currentSteerAngle = Mathf.Lerp(
            currentSteerAngle,
            targetAngle,
            Time.fixedDeltaTime * steerSmooth
        );

        for (int i = 0; i < frontWheelsCount; i++)
            wheelColliders[i].steerAngle = currentSteerAngle;
    }

    private void ApplyBrakes()
    {
        float brakeTorque = brake ? brakePower : 0f;

        foreach (var wheel in wheelColliders)
            wheel.brakeTorque = brakeTorque;
    }

    public void StopInstant()
    {
        foreach (var wheel in wheelColliders)
        {
            wheel.motorTorque = 0f;
            wheel.brakeTorque = brakePower;
        }
    }
}
