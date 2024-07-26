using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UdpReceiver : MonoBehaviour
{
    private static readonly int PortCoordinates = 2024;
    private static readonly int PortAngles = 3036;
    private static readonly string LocalIP = "127.0.0.1";

    private UdpClient udpClientCoordinates;
    private UdpClient udpClientAngles;

    public float[][] coordinates; // Coordinates as integer array
    public int[] angles; // Angles as integer array

    async void Start()
    {
        udpClientCoordinates = new UdpClient(PortCoordinates);
        udpClientAngles = new UdpClient(PortAngles);

        Debug.Log("Listening for Coordinates on port " + PortCoordinates);
        Debug.Log("Listening for Angles on port " + PortAngles);

        _ = StartUdpListener(udpClientCoordinates, "Coordinates", data => coordinates = ParseCoordinates(data));
        _ = StartUdpListener(udpClientAngles, "Angles", data => angles = ParseAngles(data));
    }

    private async Task StartUdpListener(UdpClient udpClient, string label, Action<string> dataHandler)
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            try
            {
                var receivedBytes = await udpClient.ReceiveAsync();
                string message = Encoding.UTF8.GetString(receivedBytes.Buffer);
                Debug.Log($"{label} Received: {message}");

                // Log the raw JSON data
                Debug.Log($"Raw JSON Data: {message}");

                dataHandler?.Invoke(message);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error receiving {label}: {ex.Message}");
            }
        }
    }

    private float[][] ParseCoordinates(string jsonData)
    {
        try
        {
            return JsonUtility.FromJson<floatArrayWrapper>(jsonData).values;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing coordinates: {ex.Message}");
            return new float[0][];
        }
    }

    private int[] ParseAngles(string jsonData)
    {
        try
        {
            return JsonUtility.FromJson<IntArrayWrapper>(jsonData).values;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing angles: {ex.Message}");
            return new int[0];
        }
    }

    [Serializable]
    private class IntArrayWrapper
    {
        public int[] values; // Ensure this field name matches the JSON key
    }
    [Serializable]
    private class floatArrayWrapper
    {
        public float[][] values; // Ensure this field name matches the JSON key
    }
}
