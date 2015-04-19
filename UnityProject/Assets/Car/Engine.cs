using UnityEngine;

public class Engine : MonoBehaviour
{
	private float _gas;
	public float BreakingForce;
	public float ForwardForce;
	public WheelCollider[] RearWheels;

	public void SetGas(float gas)
	{
		_gas = gas;
	}

	private void FixedUpdate()
	{
		foreach (var wheelCollider in RearWheels)
		{
			wheelCollider.motorTorque = _gas > 0 ? ForwardForce*_gas : BreakingForce*_gas;
		}
	}
}