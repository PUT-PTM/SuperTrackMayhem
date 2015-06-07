using UnityEngine;

public class DEBUGWheelRPM : MonoBehaviour
{
    private WheelCollider _wheelCollider;

    void Awake()
    {
        _wheelCollider = GetComponent<WheelCollider>();
    }

    void FixedUpdate()
    {
        Debug.Log("--------------------");
        Debug.Log("RPM: " + _wheelCollider.rpm);
        WheelHit hit;
        _wheelCollider.GetGroundHit(out hit);
        Debug.Log("Forward slip: " + hit.forwardSlip / _wheelCollider.forwardFriction.extremumSlip);
        Debug.Log("Side slip: " + hit.sidewaysSlip / _wheelCollider.sidewaysFriction.extremumSlip);
    }
}
