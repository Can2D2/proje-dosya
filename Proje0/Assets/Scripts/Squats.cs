using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squats : MonoBehaviour, IExercise
{
    private const int knee_upper_threshold = 165;
    private const int knee_lower_threshold = 90;
    private bool c = false;
    public int counter = 0;
    public int Counter => counter;

    public UdpReceiver udp;
    private int[] angles;

    void Start()
    {
    }

    void Update()
    {
        // Initialize angles to avoid null reference errors
        angles = new int[8]; // Adjust size based on expected data

        if (udp != null)
        {
            if (udp.angles != null && udp.angles.Length >= 8)
            {
                angles = udp.angles;
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

        // Ensure we have enough elements before accessing them
        if (angles.Length > 1)
        {
            Debug.Log($"angles[1] = {angles[1]}");
        }
        else
        {
            Debug.LogWarning("angles array does not contain enough elements.");
        }
        if (angles != null && angles.Length >= 8)
        {
            if (c == false &&angles[6] < knee_lower_threshold && angles[7] < knee_lower_threshold)
            {
                c = true;
            }
            else if (c && angles[6] > knee_upper_threshold && angles[7] > knee_upper_threshold)
            {
                counter += 1;
                c = false;
            }
        }
        else
        {
            Debug.LogWarning("angles array is null or does not have enough elements in Update.");
        }
    }
}