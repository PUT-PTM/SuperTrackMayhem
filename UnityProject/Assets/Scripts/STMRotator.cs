using UnityEngine;

public class STMRotator : MonoBehaviour
{
	private float rotation;
	private Quaternion maxLeft;
	private Quaternion maxRight;

	private void Start()
	{
		maxLeft = Quaternion.Euler(0, 0, 90);
		maxRight = Quaternion.Euler(0, 0, -90);
	}

	private void Update()
	{
		transform.rotation = Quaternion.Lerp(maxLeft, maxRight, rotation);
	}

	public void SetRotation(byte value)
	{
		//rotation = (float) value + 128;
		rotation = (float)value;
		rotation /= 255;
	}
}
