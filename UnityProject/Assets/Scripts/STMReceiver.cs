using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof (CarController))]
public class STMReceiver : IDisposable
{
    private const string PortNameKey = "portName";
    private const string DefaultPortName = "COM5";
    private const string BaudRateKey = "baudRate";
    private const int DefaultBaudRate = 112500;
    private const string ParityKey = "parity";
    private const Parity DefaultParity = Parity.None;
    private const string DataBitsKey = "dataBits";
    private const int DefaultDataBits = 8;
    private const string StopBitsKey = "stopBits";
    private const StopBits DefaultStopBits = StopBits.One;
    private readonly byte[] _readFloatBuffer = new byte[4];
    private CarController _controller;
    private bool _keepListenieng = true;
    public ButtonsState Buttons;
    public float HorizontalAxis;
    public SerialPort Port;
    public byte[] ledPacketToSend = new byte[3];
    private Thread t;
    private bool _blinkLeds = true;
    private int [] _packetToSend = {100,20};

    public STMReceiver()
    {
        Port = CreatePort();
        LevelManager.RaceStarted += OnRaceStarted;
        LevelManager.RaceFinished += OnRaceFinished;

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

    public void Dispose()
    {
        _keepListenieng = false;
        LevelManager.RaceFinished -= OnRaceFinished;
        LevelManager.RaceStarted -= OnRaceStarted;
    }

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

    private void OnRaceStarted()
    {
        _blinkLeds = false;
    }

    private void OnRaceFinished(bool success)
    {
        _blinkLeds = true;
        _packetToSend[0] += 100;
    }

    private void InternalStartListening()
    {
        while (_keepListenieng)
        {            
            if (_packetToSend[0] > 0)
            {
                if (_blinkLeds == true)
                {
                    ledPacketToSend[0] = 170;
                    ledPacketToSend[1] = 238;
                    ledPacketToSend[2] = 01;
                    Debug.Log("Leds ON");
                    Port.Write(ledPacketToSend, 0, 3);
                    _packetToSend[0]--;
                }
            }
            if (_packetToSend[1]>0)
            {
                if (_blinkLeds == false)
                {
                        ledPacketToSend[0] = 170;
                        ledPacketToSend[1] = 238;
                        ledPacketToSend[2] = 0;
                        Debug.Log("Leds OFF");
                        Port.Write(ledPacketToSend, 0, 3);
                        _packetToSend[1]--;
               }
            }
            Port.BaseStream.Flush();
            // Wait for packet start byte
            if (Port.ReadByte() != 0xAA)
            {
                continue;
            }

            var command = Port.ReadByte();

            if (command == 0xAC)
            {
                Debug.Log("Accelerometer command nr: " + command + "\n");

                var axisX = ReadFloat();
                Debug.Log("X axis: " + axisX + "\n");

                HorizontalAxis = ReadFloat();
                Debug.Log("Y axis: " + HorizontalAxis + "\n");

                var axisZ = ReadFloat();
                Debug.Log("Z axis: " + axisZ + "\n");

                var crc = (byte) Port.ReadByte();
                Debug.Log("CRC: " + crc + "\n\n");
            }
            else if (command == 0x38)
            {
                Debug.Log("Button command nr: " + command + "\n");
                Buttons.BreakButtonDown = Port.ReadByte() != 0;
                Debug.Log("Button 1 state: " + Buttons.BreakButtonDown + "\n");

                Buttons.Button1Down = Port.ReadByte() != 0;
                Buttons.Button2Down = Port.ReadByte() != 0;
                Buttons.Button3Down = Port.ReadByte() != 0;
                var crc = (byte) Port.ReadByte();
            }
        }
    }

    private float ReadFloat()
    {
        _readFloatBuffer[0] = (byte) Port.ReadByte();
        _readFloatBuffer[1] = (byte) Port.ReadByte();
        _readFloatBuffer[2] = (byte) Port.ReadByte();
        _readFloatBuffer[3] = (byte) Port.ReadByte();
        return BitConverter.ToSingle(_readFloatBuffer, 0);
    }

    /// <summary>
    ///     Creates a SerialPort object based on config file data (if available)
    /// </summary>
    private SerialPort CreatePort()
    {
        string portName;
        if (ConfigReader.TryGetValue(PortNameKey, out portName))
        {
            return new SerialPort(DefaultPortName, DefaultBaudRate, DefaultParity, DefaultDataBits, DefaultStopBits);
        }

        int baudRate;
        if (!ConfigReader.TryGetInt(BaudRateKey, out baudRate))
        {
            baudRate = DefaultBaudRate;
        }

        Parity parity;
        string parityVal;
        if (!ConfigReader.TryGetValue(ParityKey, out parityVal))
        {
            parity = DefaultParity;
        }
        else
        {
            switch (parityVal)
            {
                case "Parity.None":
                    parity = Parity.None;
                    break;
                case "Parity.Even":
                    parity = Parity.Even;
                    break;
                case "Parity.Odd":
                    parity = Parity.Odd;
                    break;
                case "Parity.Space":
                    parity = Parity.Space;
                    break;
                default:
                    parity = DefaultParity;
                    break;
            }
        }

        int dataBits;
        if (!ConfigReader.TryGetInt(DataBitsKey, out dataBits))
        {
            dataBits = DefaultDataBits;
        }

        StopBits stopBits;
        string stopBitsVal;
        if (!ConfigReader.TryGetValue(StopBitsKey, out stopBitsVal))
        {
            stopBits = DefaultStopBits;
        }
        else
        {
            switch (stopBitsVal)
            {
                case "StopBits.None":
                    stopBits = StopBits.None;
                    break;
                case "StopBits.One":
                    stopBits = StopBits.One;
                    break;
                case "StopBits.OnePointFive":
                    stopBits = StopBits.OnePointFive;
                    break;
                case "StopBits.Two":
                    stopBits = StopBits.Two;
                    break;
                default:
                    stopBits = DefaultStopBits;
                    break;
            }
        }

        return new SerialPort(portName, baudRate, parity, dataBits, stopBits);
    }
}