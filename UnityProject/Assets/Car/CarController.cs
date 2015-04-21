using UnityEngine;

public class CarController : MonoBehaviour
{
	private Engine _engine;
	private float _gas;
	private Steering _steering;
	public Transform SteeringWheel;
	private bool _controlsEnabled = false;

	private void Awake()
	{
		_engine = GetComponent<Engine>();
		_steering = GetComponent<Steering>();
	}

	public void EnableControls(bool controlsEnabled)
	{
		_controlsEnabled = controlsEnabled;
		if (!_controlsEnabled)
		{
			_engine.SetGas(0);
		}
	}

	public void SetMoveDirection(bool forward)
	{
		if (!_controlsEnabled)
		{
			return;
		}
		// We move forward as default
		_engine.SetGas(forward ? -1 : 1);
	}

	public void SetSteer(float steer)
	{
		if (!_controlsEnabled)
		{
			return;
		}
		SteeringWheel.localEulerAngles = new Vector3(0, steer*90, 0);
		_steering.SetSteering(steer);
	}
}