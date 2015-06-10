using System;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof (CarController))]
public class STMReceiver : IDisposable
{
    private const string PortNameKey = "portName";
    private const string DefaultPortName = "COM4";
    private const string BaudRateKey = "baudRate";
    private const int DefaultBaudRate = 112500;
    private const string ParityKey = "parity";
    private const Parity DefaultParity = Parity.None;
    private const string DataBitsKey = "dataBits";
    private const int DefaultDataBits = 8;
    private const string StopBitsKey = "stopBits";
    private const StopBits DefaultStopBits = StopBits.One;
    private static STMReceiver _instance;
    private readonly int[] _numberOfLedPackets = {100, 20};
    private readonly byte[] _readFloatBuffer = new byte[4];
    private bool _blinkLeds = true;
    private CarController _controller;
    private bool _keepListenieng = true;
    public ButtonsState Buttons;
    public float HorizontalAxis;
    public byte[] ledPacketToSend = new byte[4];
    public SerialPort Port;
    private Thread t;

    public STMReceiver()
    {
        Port = CreatePort();
        LevelManager.RaceStarted += OnRaceStarted;
        LevelManager.RaceFinished += OnRaceFinished;

        try
        {
            Port.Open();
        }
        catch
        {
        }
    }

    public static STMReceiver Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new STMReceiver();
                _instance.StartListening();
            }
            return _instance;
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
            Debug.LogWarning("Port is not open, cannot start listening");
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
        _numberOfLedPackets[0] += 100;
    }

    private void InternalStartListening()
    {
        while (_keepListenieng)
        {
            if (_numberOfLedPackets[0] > 0)
            {
                if (_blinkLeds)
                {
                    ledPacketToSend[0] = 170; // NEW_PACKET
                    ledPacketToSend[1] = 238; // LED_SEQUENCE
                    ledPacketToSend[2] = 01; // LED_ACCORDING_TO_CLOCK
                    ledPacketToSend[3] = 99; // CRC_START
                    Port.Write(ledPacketToSend, 0, 4);
                    _numberOfLedPackets[0]--;
                }
            }
            if (_numberOfLedPackets[1] > 0)
            {
                if (_blinkLeds == false)
                {
                    ledPacketToSend[0] = 170; // NEW_PACKET
                    ledPacketToSend[1] = 238; // LED_SEQUENCE
                    ledPacketToSend[2] = 0; // LED_NO_LEDS
                    ledPacketToSend[3] = 99; // CRC_START
                    Port.Write(ledPacketToSend, 0, 4);
                    _numberOfLedPackets[1]--;
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
                var axisX = ReadFloat();
                var tempHorizontalAxis = ReadFloat();
                var axisZ = ReadFloat();
                var crc = (byte) Port.ReadByte();
                if (crc == 99)
                {
                    HorizontalAxis = tempHorizontalAxis;
                }
            }
            else if (command == 0x38)
            {
                var tempButtons = new ButtonsState
                {
                    BreakButtonDown = Port.ReadByte() != 0,
                    Button1Down = Port.ReadByte() != 0,
                    Button2Down = Port.ReadByte() != 0,
                    Button3Down = Port.ReadByte() != 0
                };
                var crc = (byte) Port.ReadByte();
                if (crc == 99)
                {
                    Buttons = tempButtons;
                }
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