using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof (CarController))]
	public class CarUserControl : MonoBehaviour
	{
		private CarController m_Car; // the car controller we want to use
		public float Steer;
		public Transform SteeringWheel;
		public float SteerSpeed;

		private void Awake()
		{
			// get the car controller
			m_Car = GetComponent<CarController>();
		}

		private void FixedUpdate()
		{
			// pass the input to the car!
			var h = Input.GetAxis("Horizontal");
			Steer = Mathf.MoveTowards(Steer, h, SteerSpeed*Time.deltaTime);
			Steer = Mathf.Clamp(Steer, -1, 1);
			var adjustedSteer = Mathf.Sign(Steer)*Steer*Steer;
			SteeringWheel.localEulerAngles = new Vector3(0, 0, adjustedSteer*90);
			var v = Input.GetAxis("Vertical");
#if !MOBILE_INPUT
			var handbrake = Input.GetAxis("Jump");
			m_Car.Move(adjustedSteer, v, v, handbrake);
#else
            m_Car.Move(h, v, v, 0f);
#endif
		}
	}
}