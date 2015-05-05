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
		Port = new SerialPort("COM5", 112500, Parity.None, 8, StopBits.One);
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
            int command;
            Int16 axisX, axisY, axisZ, max;
            byte crc;
            byte[] buffer = new byte[2];

           // command = Port.ReadByte();




		while (_keepListenieng)
		{
			Port.BaseStream.Flush();
			//Data = Port.ReadByte();
            command = Port.ReadByte();

            if (command == 0xAA)
            {
                
                Debug.Log("Accelerometer command nr: " + command + "\n");

                buffer[0] = (byte)Port.ReadByte();
                buffer[1] = (byte)Port.ReadByte();
                axisX = BitConverter.ToInt16(buffer, 0);
                Debug.Log("X axis: " + axisX + "\n");

                buffer[0] = (byte)Port.ReadByte();
                buffer[1] = (byte)Port.ReadByte();
                Data = BitConverter.ToInt16(buffer, 0);
                Debug.Log("Y axis: " + Data + "\n");

                buffer[0] = (byte)Port.ReadByte();
                buffer[1] = (byte)Port.ReadByte();
                axisZ = BitConverter.ToInt16(buffer, 0);
                Debug.Log("Z axis: " + axisZ + "\n");

                buffer[0] = (byte)Port.ReadByte();
                buffer[1] = (byte)Port.ReadByte();
                max = BitConverter.ToInt16(buffer, 0);
                Debug.Log("Accelerometer max: " + max + "\n");

                crc = (byte)Port.ReadByte();
                Debug.Log("CRC: " + crc + "\n\n");

            }
		}
	}

	public void Dispose()
	{
		_keepListenieng = false;
	}
}