using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Constants.PlayerCarTag))
        {
            return;
        }

        LevelManager.FinishRace(true);
    }
}