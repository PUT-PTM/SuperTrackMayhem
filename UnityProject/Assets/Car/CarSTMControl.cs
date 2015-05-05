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
		_controller.SetSteer((((float)_receiver.Data - 128) / 128));
		_controller.SetMoveDirection(true);
	}

	void OnDestroy()
	{
		if (_receiver != null)
		{
			_receiver.Dispose();
		}
	}
}
