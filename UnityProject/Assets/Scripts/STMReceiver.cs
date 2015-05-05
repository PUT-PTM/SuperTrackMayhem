using UnityEngine;
using System.IO.Ports;

[RequireComponent(typeof(CarController))]
public class STMReceiver : MonoBehaviour
{
	private CarController _controller;
	private byte val;
	public SerialPort Port = new SerialPort("COM4", 112500, Parity.None, 8, StopBits.One);


	void Start()
	{
		//SerialPort Port = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
		_controller = GetComponent<CarController>();
		if (Port == null)
			Debug.Log("Error, Port = Null");

		foreach (string port2 in SerialPort.GetPortNames())
		{
			Debug.Log(port2);
		}

		try
		{
			Port.Open();
			Debug.Log("Port open OK");
		}
		
		catch 
		{
			Debug.Log("Error, cannot open port");
		}

		Port.DataReceived += DataReceivedHandler;

	}

	void Update()
	{
		//Debug.Log("wywolanie funkcji");
		//Debug.Log(Port.ReadExisting());
		//Port.BaseStream.Flush();
		Port.BaseStream.Flush();
		int indata = Port.ReadByte();
		Debug.Log (indata);
		//Debug.Log(indata);
		/*if(((position-indata)<7)||((position-indata)>-7))
			rotator.SetRotation ((byte)indata);*/

		_controller.SetSteer(((float)indata - 128)/128);
		_controller.SetMoveDirection(true);
	}


	private void DataReceivedHandler(object sender,SerialDataReceivedEventArgs e)
	{
		Debug.Log("obsluga zdarzenia");
		SerialPort sp = (SerialPort)sender;
		string indata = sp.ReadLine();
		Debug.Log (indata);
	//	val=byte.Parse(indata);
	}


}
