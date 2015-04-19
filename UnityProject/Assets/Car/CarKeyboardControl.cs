using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
	[RequireComponent(typeof (CarController))]
	public class CarKeyboardControl : MonoBehaviour
	{
		private CarController _car;
		private float _steer;
		public float SteerSpeed;

		private void Awake()
		{
			_car = GetComponent<CarController>();
		}

		private void FixedUpdate()
		{
			var h = Input.GetAxis("Horizontal");
			_steer = Mathf.MoveTowards(_steer, h, SteerSpeed*Time.deltaTime);
			_steer = Mathf.Clamp(_steer, -1, 1);
			var adjustedSteer = Mathf.Sign(_steer)*_steer*_steer;
			_car.SetSteer(adjustedSteer);
			_car.SetMoveDirection(Input.GetKey(KeyCode.Space));
		}
	}
}