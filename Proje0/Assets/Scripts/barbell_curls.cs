using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barbell_curls : MonoBehaviour
{
    private float wristCoorMax = 0f;
    private float wristCoorMin = 1f;
    public float scaledCoor = 0f;

    private int elbowLowerThreshold = 90;
    private int elbowUpperThreshold = 150;
    public int counter = 0;
    private bool c = false;
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

        int elbowAngle = angles[2];
        float wristCoor = coordinates[5][1];
        int hipAngle = angles[4];

        if(!c && elbowAngle > elbowUpperThreshold) c = true;
        else if(c && elbowAngle < elbowLowerThreshold){
            counter++;
            c = false;
        }

        if(wristCoor < wristCoorMin) wristCoorMin = wristCoor;
        if(wristCoor > wristCoorMax) wristCoorMax = wristCoor;

        if(wristCoorMax != wristCoorMin){
            scaledCoor = (wristCoor-wristCoorMin)/(wristCoorMax-wristCoorMin);
        }

        if(hipAngle < 160){
            // optional
        }
    }
}
