using UnityEngine;

public class CarController : MonoBehaviour
{
	private Engine _engine;
	private float _gas;
	private Steering _steering;
	public Transform SteeringWheel;

	private void Awake()
	{
		_engine = GetComponent<Engine>();
		_steering = GetComponent<Steering>();
	}

	public void SetMoveDirection(bool forward)
	{
		// We move forward as default
		_engine.SetGas(forward ? -1 : 1);
	}

	public void SetSteer(float steer)
	{
		SteeringWheel.localEulerAngles = new Vector3(0, steer*90, 0);
		_steering.SetSteering(steer);
	}
}