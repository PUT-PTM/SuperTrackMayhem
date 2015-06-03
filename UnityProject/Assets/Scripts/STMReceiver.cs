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

    public float Data;
	public bool Break;

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
           // command = Port.ReadByte();
		while (_keepListenieng)
		{
			Port.BaseStream.Flush();
			//Data = Port.ReadByte();

            if (Port.ReadByte() == 0xAA)
            {

                int command = Port.ReadByte();
                byte[] buffer = new byte[4];

                if (command == 0xAC)
                {

                    Debug.Log("Accelerometer command nr: " + command + "\n");

                    buffer[0] = (byte)Port.ReadByte();
                    buffer[1] = (byte)Port.ReadByte();
                    buffer[2] = (byte)Port.ReadByte();
                    buffer[3] = (byte)Port.ReadByte();
                    float axisX = BitConverter.ToSingle(buffer, 0);
                    Debug.Log("X axis: " + axisX + "\n");

                    buffer[0] = (byte)Port.ReadByte();
                    buffer[1] = (byte)Port.ReadByte();
                    buffer[2] = (byte)Port.ReadByte();
                    buffer[3] = (byte)Port.ReadByte();
                    Data = BitConverter.ToSingle(buffer, 0);
                    Debug.Log("Y axis: " + Data + "\n");

                    buffer[0] = (byte)Port.ReadByte();
                    buffer[1] = (byte)Port.ReadByte();
                    buffer[2] = (byte)Port.ReadByte();
                    buffer[3] = (byte)Port.ReadByte();
                    float axisZ = BitConverter.ToSingle(buffer, 0);
                    Debug.Log("Z axis: " + axisZ + "\n");

                    byte crc = (byte)Port.ReadByte();
                    Debug.Log("CRC: " + crc + "\n\n");

                }
                else if (command == 0x38)
                {
                    Debug.Log("Button command nr: " + command + "\n");
                    byte button1state = (byte)Port.ReadByte();
                    Debug.Log("Button 1 state: " + button1state + "\n");

                    byte button2state = (byte)Port.ReadByte();
                    //Debug.Log("Button 2 state: " + button2state + "\n");

                    byte button3state = (byte)Port.ReadByte();
                    //Debug.Log("Button 3 state: " + button3state + "\n");

                    byte button4state = (byte)Port.ReadByte();
                    //Debug.Log("Button 4 state: " + button4state + "\n");

                    byte crc = (byte)Port.ReadByte();
                    //Debug.Log("CRC: " + crc + "\n\n");

                    if (button1state == 0)
                        Break = false;
                    else
                        Break = true;
                }
                //  Break = false;
            }
		}
        
	}

	public void Dispose()
	{
		_keepListenieng = false;
	}
}