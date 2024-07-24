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
    private static readonly int PortImage = 1012;
    private static readonly string LocalIP = "127.0.0.1";

    private UdpClient udpClientCoordinates;
    private UdpClient udpClientAngles;
    private UdpClient udpClientImage;

    private string imageData;

    async void Start()
    {
        udpClientCoordinates = new UdpClient(PortCoordinates);
        udpClientAngles = new UdpClient(PortAngles);
        udpClientImage = new UdpClient(PortImage);

        Debug.Log("Listening for Coordinates on port " + PortCoordinates);
        Debug.Log("Listening for Angles on port " + PortAngles);
        Debug.Log("Listening for Image on port " + PortImage);

        // Start listening in background and await completion
        _ = StartUdpListener(udpClientCoordinates, "Coordinates");
        _ = StartUdpListener(udpClientAngles, "Angles");
        _ = StartImageUdpListener(data => 
        {
            imageData = data;
            byte[] imageBytes = Convert.FromBase64String(imageData);});
    
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

    private async Task StartImageUdpListener(Action<string> onImageReceived)
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(LocalIP), PortImage);

        while (true)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(imageData);
                var receivedBytes = await udpClientImage.ReceiveAsync();
                string base64Image = Encoding.UTF8.GetString(receivedBytes.Buffer);
                Debug.Log("Received image data of length: " + base64Image.Length);

                onImageReceived?.Invoke(base64Image);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error receiving image: {ex.Message}");
            }

            await Task.CompletedTask;
        }
    }

    private void OnApplicationQuit()
    {
        udpClientCoordinates?.Close();
        udpClientAngles?.Close();
        udpClientImage?.Close();
    }
}