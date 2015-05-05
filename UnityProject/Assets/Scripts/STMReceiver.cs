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
            int command = Port.ReadByte();
            byte[] buffer = new byte[2];
            if (command == 0xAA)
            {
                
                Debug.Log("Accelerometer command nr: " + command + "\n");

                buffer[0] = (byte)Port.ReadByte();
                buffer[1] = (byte)Port.ReadByte();
                Int16 axisX = BitConverter.ToInt16(buffer, 0);
                Debug.Log("X axis: " + axisX + "\n");

                buffer[0] = (byte)Port.ReadByte();
                buffer[1] = (byte)Port.ReadByte();
                Data = BitConverter.ToInt16(buffer, 0);
                Debug.Log("Y axis: " + Data + "\n");

                buffer[0] = (byte)Port.ReadByte();
                buffer[1] = (byte)Port.ReadByte();
                Int16 axisZ = BitConverter.ToInt16(buffer, 0);
                Debug.Log("Z axis: " + axisZ + "\n");

                buffer[0] = (byte)Port.ReadByte();
                buffer[1] = (byte)Port.ReadByte();
                Int16 max = BitConverter.ToInt16(buffer, 0);
                Debug.Log("Accelerometer max: " + max + "\n");

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

	public void Dispose()
	{
		_keepListenieng = false;
	}
}