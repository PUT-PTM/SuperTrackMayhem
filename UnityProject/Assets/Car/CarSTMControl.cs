using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarSTMControl : MonoBehaviour
{
	private CarController _controller;
	private STMReceiver _receiver;
	void Start()
	{
		_controller = GetComponent<CarController>();
		_receiver = new STMReceiver();
		_receiver.StartListening();
	}

	void Update()
	{
		_controller.SetSteer(_receiver.HorizontalAxis/9.8f);
		_controller.SetMoveDirection(!_receiver.Buttons.BreakButtonDown);
	}

	void OnDestroy()
	{
		if (_receiver != null)
		{
			_receiver.Dispose();
		}
	}
}
