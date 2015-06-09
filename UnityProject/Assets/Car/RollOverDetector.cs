using UnityEngine;

public class RollOverDetector : MonoBehaviour
{
    private float _rollOverTime;
    public float RollAngle = 50;
    public float RollOverTimeThreshold = 4;

    private void Update()
    {
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