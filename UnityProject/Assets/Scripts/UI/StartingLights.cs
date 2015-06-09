using System.Collections;
using UnityEngine;

public class StartingLights : MonoBehaviour
{
    public GameObject[] Lights;
    public float TimeBetweenLights;

    private void Start()
    {
        StartCoroutine(LightsSequence());
    }

    private IEnumerator LightsSequence()
    {
        for (var i = 0; i < Lights.Length; i++)
        {
            yield return new WaitForSeconds(TimeBetweenLights);
            Lights[i].SetActive(true);
        }
        LevelManager.StartRace();
        yield return new WaitForSeconds(TimeBetweenLights);
        gameObject.SetActive(false);
    }
}