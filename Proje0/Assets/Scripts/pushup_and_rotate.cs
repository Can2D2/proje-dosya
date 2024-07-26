using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pushup_and_rotate : MonoBehaviour
{
    private bool pushedDown = false;
    public int counter = 0;

    private const int elbowBendThreshold = 100;
    private const int hipAlignmentThreshold = 160;

    private float shoulderCoorMin = 1f;
    private float shoulderCoorMax = 0f;
    public float scaledCoor = 0f;
    private float wristCoorMin = 1f;
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

        float shoulderCoor = coordinates[0][1];
        int elbowAngle = angles[2];
        int hipAngle = angles[4];
        float wristCoor = coordinates[4][1];

        if(wristCoor < wristCoorMin) wristCoorMin = wristCoor;

        if (!pushedDown && elbowAngle < elbowBendThreshold)
        {
            pushedDown = true;
        }
        else if (pushedDown && wristCoor > shoulderCoor + (shoulderCoor - wristCoorMin)*0.5)
        {
            pushedDown = false;
            counter++;
        }
        if(shoulderCoor > shoulderCoorMax) shoulderCoorMax = shoulderCoor;
        if(shoulderCoor < shoulderCoorMin) shoulderCoorMin = shoulderCoor;

        if (shoulderCoorMax != shoulderCoorMin) {
            scaledCoor = (scaledCoor-shoulderCoorMin)/(shoulderCoorMax-shoulderCoorMin);
        }

        if(hipAngle < hipAlignmentThreshold){
            // (optional) warning in case alignment is off
        }
    }
}
