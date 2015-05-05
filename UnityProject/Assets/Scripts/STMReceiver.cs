using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof (CarController))]
public class STMReceiver :IDisposable
{
	private CarController _controller;
	private byte val;
	public SerialPort Port;

	public STMReceiver()
	{
		Port = new SerialPort("COM4", 112500, Parity.None, 8, StopBits.One);
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
	}

	public int Data;

	private bool _keepListenieng = true;
	private Thread t;
	public void StartListening()
	{
		if (!Port.IsOpen)
		{
			Debug.Log("Port is not open, cannot start listening");
			return;
		}
		t = new Thread(InternalStartListening);
		_keepListenieng = true;
		t.Start(); 
	}


	private void InternalStartListening()
	{
		while (_keepListenieng)
		{
			Port.BaseStream.Flush();
			Data = Port.ReadByte();
		}
	}

	public void Dispose()
	{
		_keepListenieng = false;
	}
}