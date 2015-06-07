using UnityEngine;

public class WheelRotator : MonoBehaviour
{
	private Quaternion _baseRotation;
	public WheelCollider PhysicsWheel;

	void Awake()
	{
		_baseRotation = transform.localRotation;
	}

	void Update()
	{
		Quaternion rotation;
		Vector3 position;
		PhysicsWheel.GetWorldPose(out position, out rotation);
		transform.rotation = rotation*_baseRotation;
		transform.position = position;
	}
}
