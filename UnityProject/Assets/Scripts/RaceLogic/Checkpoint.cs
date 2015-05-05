using System;
using UnityEngine;

[RequireComponent(typeof (Collider))]
public class Checkpoint : MonoBehaviour
{
	public int index;
	public GameObject NextCheckpoint;

	public static event Action<int> CheckPointReached;

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag(Constants.PlayerCarTag))
		{
			return;
		}

		if (CheckPointReached != null)
		{
			CheckPointReached(index);
		}

		if (NextCheckpoint != null)
		{
			NextCheckpoint.SetActive(true);
		}
		gameObject.SetActive(false);
	}
}