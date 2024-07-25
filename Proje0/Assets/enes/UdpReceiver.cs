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

    async void Start()
    {
        udpClientCoordinates = new UdpClient(PortCoordinates);
        udpClientAngles = new UdpClient(PortAngles);

        Debug.Log("Listening for Coordinates on port " + PortCoordinates);
        Debug.Log("Listening for Angles on port " + PortAngles);

        // Start listening in background and await completion
        _ = StartUdpListener(udpClientCoordinates, "Coordinates");
        _ = StartUdpListener(udpClientAngles, "Angles");
    }

    private async Task StartUdpListener(UdpClient udpClient, string label)
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(LocalIP), 0);

        while (true)
        {
            try
            {
                var receivedBytes = await udpClient.ReceiveAsync();
                string message = Encoding.UTF8.GetString(receivedBytes.Buffer);
                Debug.Log($"{label} Received: {message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error receiving {label}: {ex.Message}");
            }

            await Task.CompletedTask;
        }
    }

    private void OnApplicationQuit()
    {
        udpClientCoordinates?.Close();
        udpClientAngles?.Close();
    }
}