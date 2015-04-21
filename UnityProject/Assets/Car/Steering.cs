using UnityEngine;

public class Steering : MonoBehaviour
{
	private float _steering;
	public float SteerAngle;
	public WheelCollider[] SteeringWheels;
	public Transform[] SteeringheelGraphics;

	public void SetSteering(float steering)
	{
		_steering = Mathf.Clamp(steering, -1, 1);
	}

	private void FixedUpdate()
	{
		var steer = _steering*SteerAngle;
		foreach (var steeringWheel in SteeringWheels)
		{
			steeringWheel.steerAngle = steer;
		}
		foreach (var steeringwheel in SteeringheelGraphics)
		{
			var eulerAngles = steeringwheel.localEulerAngles;
			eulerAngles.x = steer;
			steeringwheel.localEulerAngles = eulerAngles;
		}
	}
}