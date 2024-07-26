using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Reverse_flutter_kicks : MonoBehaviour, IExercise
{
    private float right_ankle_max = 0;
    private float left_ankle_max = 0;
    private bool r = false;
    private bool l = false;
    private bool r_is_increasing = false;
    private bool l_is_increasing = false;
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

        float left_ankle = coordinates[10][1];
        float right_ankle = coordinates[11][1];

        if (left_ankle >= left_ankle_max)
        {
            left_ankle_max = left_ankle;
            l_is_increasing = true;
        }
        else if (l_is_increasing) l = true;

        if (right_ankle >= right_ankle_max)
        {
            right_ankle_max = right_ankle;
            r_is_increasing = true;
        }
        else if (r_is_increasing) r = true;

        if (l && r)
        {
            counter += 1;
            l = false;
            r = false;
        }
    }
}