using UnityEngine;

public class CarController : MonoBehaviour
{
	private bool _breaking;
	private float _gas;
	private Rigidbody _rigidbody;
	public Engine CarEngine;
	public Steering SteeringSystem;
	public Transform SteeringWheel;

	void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void SetBreaks(bool breaking)
	{
		_breaking = breaking;
		SetGas(_gas);
	}

	public void SetSteer(float steer)
	{
		SteeringWheel.localEulerAngles = new Vector3(0, 0, steer*90);
		SteeringSystem.SetSteering(steer);
	}

	public void SetGas(float gas)
	{
		_gas = _breaking ? -1 : Mathf.Clamp(gas, -1, 1);
		CarEngine.SetGas(_gas);
	}
}