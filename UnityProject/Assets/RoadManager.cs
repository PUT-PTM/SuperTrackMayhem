using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
	public RoadPath CurrentPath;
	public float GenerateDistance;
	public GameObject Left30Prefab;
	public GameObject Left45Prefab;
	public Transform Player;
	private Dictionary<RoadType, GameObject> prefabs;
	public float RepeatChance;
	public GameObject Right30Prefab;
	public GameObject Right45Prefab;
	public float StraightChance;
	public GameObject StraightPrefab;

	private void Start()
	{
		prefabs = new Dictionary<RoadType, GameObject>();
		prefabs[RoadType.Left30] = Left30Prefab;
		prefabs[RoadType.Left45] = Left45Prefab;
		prefabs[RoadType.Right30] = Right30Prefab;
		prefabs[RoadType.Right45] = Right45Prefab;
		prefabs[RoadType.Straight] = StraightPrefab;
	}

	private bool HasToGenerate()
	{
		return Vector3.Distance(CurrentPath.End.position, Player.position) < GenerateDistance;
	}

	private void Update()
	{
		if (!HasToGenerate()) return;
		GameObject prefab;
		var rv = Random.value;
		if (Random.value < RepeatChance)
		{
			prefab = prefabs[CurrentPath.Type];
		}
		else if (Random.value < StraightChance)
		{
			prefab = StraightPrefab;
		}
		else if (rv < 0.25f)
		{
			prefab = Left30Prefab;
		}
		else if (rv < 0.5f)
		{
			prefab = Left45Prefab;
		}
		else if (rv < 0.75f)
		{
			prefab = Right30Prefab;
		}
		else
		{
			prefab = Right45Prefab;
		}

		var target = CurrentPath.End;
		var newGO = (GameObject)Instantiate(prefab, target.position, target.rotation);
		CurrentPath = newGO.GetComponent<RoadPath>();
	}
}