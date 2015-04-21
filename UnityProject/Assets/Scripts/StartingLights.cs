using System.Collections;
using UnityEngine;

public class StartingLights : MonoBehaviour
{
	public float TimeBetweenLights;

	public GameObject YellowLight;
	public GameObject GreenLight;

	private void Start()
	{
		StartCoroutine(LightsSequence());
	}

	private IEnumerator LightsSequence()
	{
		yield return new WaitForSeconds(TimeBetweenLights);
		YellowLight.SetActive(true);
		yield return new WaitForSeconds(TimeBetweenLights);
		GreenLight.SetActive(true);
		FindObjectOfType<CarController>().EnableControls(true);
		yield return new WaitForSeconds(TimeBetweenLights);
		gameObject.SetActive(false);
	}
}
