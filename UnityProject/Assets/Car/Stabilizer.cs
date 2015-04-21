using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class Stabilizer : MonoBehaviour
{
	private Rigidbody _rigidbody;
	public float AntiRollForce;
	public WheelCollider LeftWheelCollider;
	public WheelCollider RightWheelCollider;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		WheelHit hit;
		var leftGrounded = LeftWheelCollider.GetGroundHit(out hit);
		float suspensionTravelLeft = 1;
		if (leftGrounded)
		{
			suspensionTravelLeft = (-LeftWheelCollider.transform.InverseTransformPoint(hit.point).y - LeftWheelCollider.radius)
			                       /LeftWheelCollider.suspensionDistance;
		}

		var rightGrounded = RightWheelCollider.GetGroundHit(out hit);
		float suspensionTravelRight = 1;
		if (rightGrounded)
		{
			suspensionTravelRight = (-RightWheelCollider.transform.InverseTransformPoint(hit.point).y - RightWheelCollider.radius)
			                        /RightWheelCollider.suspensionDistance;
		}

		var antiRoll = (suspensionTravelLeft - suspensionTravelRight)*AntiRollForce;

		if (leftGrounded)
		{
			_rigidbody.AddForceAtPosition(transform.up*(-antiRoll), LeftWheelCollider.transform.position);
		}

		if (rightGrounded)
		{
			_rigidbody.AddForceAtPosition(transform.up*antiRoll, RightWheelCollider.transform.position);
		}
	}
}