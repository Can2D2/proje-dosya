using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Jumping_jacks : MonoBehaviour
{
    private const int shoulder_upper_threshold = 120;
    private const int shoulder_lower_threshold = 40;
    private bool c = false;
    private int prev_hip = 200;
    public int counter = 0;
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

        int curr_hip = angles[5];

        if (!c && angles[0] > shoulder_upper_threshold && angles[1] > shoulder_upper_threshold && prev_hip - curr_hip > 20)
        {
            c = true;
            prev_hip = curr_hip;
        }
        else if (c && angles[0] < shoulder_lower_threshold && angles[1] < shoulder_lower_threshold && curr_hip - prev_hip > 20)
        {
            counter += 1;
            c = false;
            prev_hip = curr_hip;
        }
    }
}