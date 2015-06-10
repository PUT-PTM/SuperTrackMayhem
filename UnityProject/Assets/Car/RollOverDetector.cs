using UnityEngine;

public class RollOverDetector : MonoBehaviour
{
    private float _rollOverTime;
    public float RollAngle = 50;
    public float RollOverTimeThreshold = 4;
    private bool _inRace;

    private void Awake()
    {
        LevelManager.RaceStarted += delegate { _inRace = true; };
        LevelManager.RaceFinished += delegate { _inRace = false; };
    }

    private void Update()
    {
        if (!_inRace)
        {
            return;
        }
        var dotProduct = Vector3.Dot(transform.up, Vector3.up);
        if (Mathf.Cos(RollAngle) > dotProduct)
        {
            _rollOverTime += Time.unscaledDeltaTime;
            if (_rollOverTime > RollOverTimeThreshold)
            {
                LevelManager.FinishRace(false);
            }
        }
        else
        {
            _rollOverTime = 0;
        }
    }
}