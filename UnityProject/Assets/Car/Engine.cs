using System;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class Engine : MonoBehaviour
{
	private float _gas;
	private Rigidbody _rigidbody;
	private Transform _transform;
	public float BreakingForce;
	public float ForwardForce;
	public WheelCollider[] RearWheels;
	public float BreakingThreshold = -0.1f;

	private void Awake()
	{
		_transform = GetComponent<Transform>();
		_rigidbody = GetComponent<Rigidbody>();
	}

	public void SetGas(float gas)
	{
		_gas = gas;
	}

	private void FixedUpdate()
	{
		var dot = Vector3.Dot(_transform.forward*_gas, _rigidbody.velocity);
		var breaking = dot < BreakingThreshold;

		if (breaking)
		{
			foreach (var wheelCollider in RearWheels)
			{
				wheelCollider.brakeTorque = BreakingForce;
				wheelCollider.motorTorque = 0;
			}
		}
		else
		{
			var motorTorque = _gas*ForwardForce;
			foreach (var wheelCollider in RearWheels)
			{
				wheelCollider.motorTorque = motorTorque;
				wheelCollider.brakeTorque = 0;
			}
		}
	}
}