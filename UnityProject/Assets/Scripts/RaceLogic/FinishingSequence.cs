using System.Collections;
using UnityEngine;

public class FinishingSequence : MonoBehaviour
{
    public GameObject SuccessScreen;
    public GameObject FailScreen;
    public GameObject StartSequence;

    void Awake()
    {
        LevelManager.RaceFinished += OnRaceFinished;
    }

    void OnRaceFinished(bool success)
    {
        if (success)
        {
            SuccessScreen.SetActive(true);
        }
        else
        {
            FailScreen.SetActive(true);
        }
        StartCoroutine(RaceFinishedCoroutine());
    }

    IEnumerator RaceFinishedCoroutine()
    {
        yield return new WaitForSeconds(5);
        FailScreen.SetActive(false);
        SuccessScreen.SetActive(false);
        StartSequence.SetActive(true);
    }
}
