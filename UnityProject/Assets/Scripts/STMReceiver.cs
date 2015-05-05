using System.IO.Ports;
using UnityEngine;

[RequireComponent(typeof (CarController))]
public class STMReceiver : MonoBehaviour
{
	private CarController _controller;
	private byte val;
	public SerialPort Port = new SerialPort("COM4", 112500, Parity.None, 8, StopBits.One);


	private void Start()
	{
		//SerialPort Port = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
		_controller = GetComponent<CarController>();
		if (Port == null)
		{
			Debug.Log("Error, Port = Null");
		}

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

	private void Update()
	{
		//Debug.Log("wywolanie funkcji");
		//Debug.Log(Port.ReadExisting());
		//Port.BaseStream.Flush();
		if (!Port.IsOpen)
		{
			return;
		}
		Port.BaseStream.Flush();
		char[] indata = Port.ReadExisting().ToCharArray();
		Debug.Log(indata);
		//Debug.Log(indata);
		/*if(((position-indata)<7)||((position-indata)>-7))
			rotator.SetRotation ((byte)indata);*/

		_controller.SetSteer((((float) indata[indata.Length - 1] - 128)/128));
		_controller.SetMoveDirection(true);
	}


	private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
	{
		Debug.Log("obsluga zdarzenia");
		SerialPort sp = (SerialPort) sender;
		string indata = sp.ReadLine();
		Debug.Log(indata);
		//	val=byte.Parse(indata);
	}
}