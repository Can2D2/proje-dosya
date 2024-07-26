using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtBridges : MonoBehaviour
{
    private bool isLifting = false; 
    public int counter = 0; 
    private float hipYMin = 1f; 
    private float hipYMax = 0f; 
    public float scaledY = 0f; 
    public UdpReceiver udp;
    private int[] angles;
    private float[][] coordinates;

    async void Start()
    {
    }

    void Update()
    {
        angles = new int[8];
        coordinates = new float[12][];

        if (udp != null)
        {
            if (udp.angles != null && udp.angles.Length >= 8)
            {
                angles = udp.angles;
                coordinates = udp.coordinates;
            }
            else
            {
                Debug.LogWarning("UdpReceiver's angles array is null or does not contain enough elements.");
            }
        }
        else
        {
            Debug.LogWarning("UdpReceiver is not assigned.");
        }
        
        float hipYLeft = coordinates[6][1];  
        float hipYRight = coordinates[7][1];

        float hipY = (hipYLeft + hipYRight) / 2f;

        if (hipY < hipYMin) hipYMin = hipY;
        if (hipY > hipYMax) hipYMax = hipY;

        if (hipYMax != hipYMin) {
            scaledY = (hipY - hipYMin) / (hipYMax - hipYMin);
        } else {
            scaledY = 0f; // Avoid division by zero if no movement detected
        }

        float upperThreshold = 0.8f;
        float lowerThreshold = 0.2f;

        if (!isLifting && scaledY > upperThreshold) {
            isLifting = true;
        } else if (isLifting && scaledY < lowerThreshold) {
            isLifting = false;
            counter++;
        }
    }
}
