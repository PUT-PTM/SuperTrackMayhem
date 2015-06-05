using System;
using System.IO.Ports;
using System.Threading;
using System.IO;
using UnityEngine;

[RequireComponent(typeof (CarController))]
public class STMReceiver :IDisposable
{
	private CarController _controller;
	private byte val;
	public SerialPort Port;

	public STMReceiver()
	{
		Port = new SerialPort("COM1", 112500, Parity.None, 8, StopBits.One);
        ReadConfigFile(Port);
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

    public void ReadConfigFile(SerialPort Port)
    {
        string[] file = System.IO.File.ReadAllLines(@"SuperTrackMayhem.config");

        if (File.Exists(@"SuperTrackMayhem.config"))
            {
                string[] stringConfig = new string[5];
                int i = 0;
                foreach (string oneLineInFile in file)
                {
                    string[] linia = oneLineInFile.Split(' ');
                    stringConfig[i] = linia[1];
                    i++;
                    Console.WriteLine(linia[1]);
                    if (i == stringConfig.Length)
                        break;
                }
                Port.PortName = stringConfig[0];
                Debug.Log("Numer portu wczytany z pliku to:" +stringConfig[0]);
                Port.BaudRate = Convert.ToInt32(stringConfig[1]);
                if (stringConfig[2] == "Parity.None")
                    Port.Parity = Parity.None;
                else if (stringConfig[2] == "Parity.Even")
                    Port.Parity = Parity.Even;
                else if (stringConfig[2] == "Parity.Mark")
                    Port.Parity = Parity.Mark;
                else if (stringConfig[2] == "Parity.Odd")
                    Port.Parity = Parity.Odd;
                else if (stringConfig[2] == "Parity.Space")
                    Port.Parity = Parity.Space;
                Port.DataBits = Convert.ToInt32(stringConfig[3]);
                if (stringConfig[3] == "StopBits.None")
                    Port.StopBits = StopBits.None;
                else if (stringConfig[3] == "StopBits.One")
                    Port.StopBits = StopBits.One;
                else if (stringConfig[3] == "StopBits.OnePointFive")
                    Port.StopBits = StopBits.OnePointFive;
                else if (stringConfig[3] == "StopBits.Two")
                    Port.StopBits = StopBits.Two;

            }
        }
    }
