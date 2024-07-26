using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbdominalCrunches : MonoBehaviour, IExercise
{
    private bool isCrunching = false;
    public int counter = 0;
    public int Counter => counter;
    private float shoulderYMin = 1f;
    private float shoulderYMax = 0f;
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
        
        float shoulderY = coordinates[0][1];

        if (shoulderY < shoulderYMin) shoulderYMin = shoulderY;
        if (shoulderY > shoulderYMax) shoulderYMax = shoulderY;

        if (shoulderYMax != shoulderYMin) {
            scaledY = (shoulderY - shoulderYMin) / (shoulderYMax - shoulderYMin);
        } else {
            scaledY = 0f;
        }

        float upperThreshold = 0.75f;
        float lowerThreshold = 0.25f;

        if (!isCrunching && scaledY > upperThreshold) {
            isCrunching = true;
        } else if (isCrunching && scaledY < lowerThreshold) {
            isCrunching = false;
            counter++;
        }
    }
}
