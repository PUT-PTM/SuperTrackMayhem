using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarSTMControl : MonoBehaviour
{
	private CarController _controller;
	void Start()
	{
		_controller = GetComponent<CarController>();
	}

	void Update()
	{
		_controller.SetSteer(STMReceiver.Instance.HorizontalAxis/9.8f);
		_controller.SetMoveDirection(!STMReceiver.Instance.Buttons.BreakButtonDown);
	}
}
