using UnityEngine;

public enum RoadType
{
	Left45,
	Left30,
	Straight,
	Right30,
	Right45
}

public class RoadPath : MonoBehaviour
{
	public Transform End;
	public RoadType Type;
}