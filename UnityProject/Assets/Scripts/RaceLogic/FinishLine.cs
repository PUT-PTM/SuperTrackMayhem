using UnityEngine;

public class FinishLine : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag(Constants.PlayerCarTag))
		{
			return;
		}

		LevelManager.OnFinishRace();
	}
}
