using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CenterOfMassSetter : MonoBehaviour
{
	public Transform CenterOfMass;

	void Awake()
	{
		GetComponent<Rigidbody>().centerOfMass = CenterOfMass.localPosition;
	}
}
