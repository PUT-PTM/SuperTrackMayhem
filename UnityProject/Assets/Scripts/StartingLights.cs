using System.Collections;
using UnityEngine;

public class StartingLights : MonoBehaviour
{
	public float TimeBetweenLights;

	public TurnOnLight YellowLight;
	public TurnOnLight GreenLight;

	private void Start()
	{
		StartCoroutine(LightsSequence());
	}

	private IEnumerator LightsSequence()
	{
		yield return new WaitForSeconds(TimeBetweenLights);
		YellowLight.TurnOn();
		yield return new WaitForSeconds(TimeBetweenLights);
		GreenLight.TurnOn();
		FindObjectOfType<CarController>().EnableControls(true);
		yield return new WaitForSeconds(TimeBetweenLights);
		gameObject.SetActive(false);
	}
}
