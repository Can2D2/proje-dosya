using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Lunges : MonoBehaviour, IExercise
{
    private const int first_knee_upper_threshold = 165;
    private const int first_knee_lower_threshold = 100;
    private const int second_knee_lower_threshold = 100;

    private bool c = false;
    private bool p = false;
    public int counter = 0;
    public int Counter => counter;

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

        if (coordinates == null || angles == null) return;

        int first_knee = p ? angles[7] : angles[6];
        int second_knee = p ? angles[6] : angles[7];

        if (!c)
        {
            if (first_knee <= first_knee_lower_threshold && second_knee <= second_knee_lower_threshold)
            {
                c = true;
            }
        }
        else
        {
            if (first_knee >= first_knee_upper_threshold)
            {
                c = false;
                p = !p;
                counter += 1;
            }
        }
    }
}