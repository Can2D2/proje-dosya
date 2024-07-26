using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class VUps : MonoBehaviour
{
    private const int hip_upper_threshold = 170; 
    private const int hip_lower_threshold = 100;   
    private const int shoulder_upper_threshold = 170;
    private const int shoulder_lower_threshold = 100;

    private bool isLyingFlat = true; 
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

        if (isLyingFlat && angles[4] > hip_upper_threshold && angles[5] > hip_upper_threshold &&
            angles[4] > shoulder_upper_threshold && angles[5] > shoulder_upper_threshold)
        {
            isLyingFlat = false;
        }
        else if (!isLyingFlat && angles[4] < hip_lower_threshold && angles[5] < hip_lower_threshold &&
                 angles[4] < shoulder_lower_threshold && angles[5] < shoulder_lower_threshold)
        {
            counter += 1;
            isLyingFlat = true;
        }
    }
}
