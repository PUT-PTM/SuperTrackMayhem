using UnityEngine;
using System.IO.Ports;

public class STMReceiver : MonoBehaviour
{
	public STMRotator rotator;
	// Wszystko poniżej można usunąć
	
	public static byte val;
	public static int position;
	public SerialPort Port = new SerialPort("COM5", 112500, Parity.None, 8, StopBits.One);


	void Start()
	{
		//SerialPort Port = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
	
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
		position = 63;
	//	Port.DtrEnable = true;
	//	Port.RtsEnable = true;

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

		position = indata;
		rotator.SetRotation ((byte)indata);

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
