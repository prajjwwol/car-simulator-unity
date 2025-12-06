using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialCommunicator : MonoBehaviour
{
    public static SerialCommunicator Instance;

    SerialPort serialPort;
    public string portName = "/dev/ttyACM0"; // Change to your Arduino port (Linux: /dev/ttyUSB0 or /dev/ttyACM0)
    public int baudRate = 9600;
    private bool isConnected = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);
        serialPort.ReadTimeout = 100;
        serialPort.WriteTimeout = 100;
        try
        {
            serialPort.Open();
            isConnected = true;
            Debug.Log($"Serial port opened successfully on {portName}");
        }
        catch (System.Exception e)
        {
            isConnected = false;
            Debug.LogError($"Failed to open serial port {portName}: {e.Message}");
            Debug.LogWarning("Check Arduino connection and port name. Common Linux ports: /dev/ttyUSB0, /dev/ttyACM0");
        }
    }

    public void SendSpeed(int speed)
    {
        if (serialPort != null && serialPort.IsOpen && isConnected)
        {
            try
            {
                serialPort.WriteLine(speed.ToString());
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Failed to send speed data: {e.Message}");
                isConnected = false;
            }
        }
    }

    public bool IsConnected()
    {
        return isConnected && serialPort != null && serialPort.IsOpen;
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}