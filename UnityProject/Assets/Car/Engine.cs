using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class Engine : MonoBehaviour
{
	private float _gas;
	private Rigidbody _rigidbody;
	private Transform _transform;
	public float BreakingForce;
	public float ForwardForce;
    public bool UseFourWheels = true;
    public WheelCollider[] FrontWheels;
	public WheelCollider[] RearWheels;
	public float BreakingThreshold = -0.1f;
    private bool _forceBreak;

	private void Awake()
	{
		_transform = GetComponent<Transform>();
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void SetGas(float gas)
	{
		_gas = gas;
	    _forceBreak = gas == 0;
	}

	private void FixedUpdate()
	{
		var dot = Vector3.Dot(_transform.forward*_gas, _rigidbody.velocity);
		var breaking = dot < BreakingThreshold || _forceBreak;

	    int wheelCount = UseFourWheels ? 4 : 2;

		if (breaking)
		{
			foreach (var wheelCollider in RearWheels)
			{
			    wheelCollider.brakeTorque = BreakingForce / wheelCount;
				wheelCollider.motorTorque = 0;
			}

		    if (!UseFourWheels)
		    {
		        return;
		    }
		    foreach (var wheelCollider in FrontWheels)
		    {
		        wheelCollider.brakeTorque = BreakingForce / wheelCount;
		        wheelCollider.motorTorque = 0;
		    }
		}
		else
		{
			var motorTorque = _gas*ForwardForce;
			foreach (var wheelCollider in RearWheels)
			{
				wheelCollider.motorTorque = motorTorque / wheelCount;
				wheelCollider.brakeTorque = 0;
			}

		    if (!UseFourWheels)
		    {
		        return;
		    }
		    foreach (var wheelCollider in FrontWheels)
		    {
		        wheelCollider.brakeTorque = BreakingForce/wheelCount;
		        wheelCollider.brakeTorque = 0;
		    }
		}
	}
}