using UnityEngine;

public class Speedometer : MonoBehaviour
{
    private Rigidbody _car;
    public Transform Hand;
    public float Rotation120;

    private void Awake()
    {
        _car = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var speed = Vector3.Dot(_car.velocity, _car.transform.forward)*3.6f;
        // The speed is in m/s even though the speedometer says mph. 
        // This is intentional since our car runs at unrealistic speeds.
        Hand.localRotation = Quaternion.AngleAxis(speed/120*Rotation120, Vector3.forward);
    }
}