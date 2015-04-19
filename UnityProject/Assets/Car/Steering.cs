using UnityEngine;

public class Steering : MonoBehaviour
{
	private float _steering;
	public float SteerAngle;
	public WheelCollider[] SteeringWheels;

	public void SetSteering(float steering)
	{
		_steering = Mathf.Clamp(steering, -1, 1);
	}

	private void FixedUpdate()
	{
		foreach (var steeringWheel in SteeringWheels)
		{
			steeringWheel.steerAngle = _steering*SteerAngle;
		}
	}
}