using UnityEngine;

[RequireComponent(typeof (Collider))]
public class Checkpoint : MonoBehaviour
{
	public GameObject NextCheckpoint;

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag(Constants.PlayerCarTag))
		{
			return;
		}
		Debug.Log("Trigger");
		if (NextCheckpoint != null)
		{
			NextCheckpoint.SetActive(true);
		}
		gameObject.SetActive(false);
	}
}