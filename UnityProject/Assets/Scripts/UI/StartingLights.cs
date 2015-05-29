using System.Collections;
using UnityEngine;

public class StartingLights : MonoBehaviour
{
	public float TimeBetweenLights;

	public GameObject[] Lights;

	private void Start()
	{
		StartCoroutine(LightsSequence());
	}

	private IEnumerator LightsSequence()
	{
		for (int i = 0; i < Lights.Length; i++)
		{
			yield return new WaitForSeconds(TimeBetweenLights);
			Lights[i].SetActive(true);
		}
		LevelManager.OnStartRace();
		yield return new WaitForSeconds(TimeBetweenLights);
		gameObject.SetActive(false);
	}
}
