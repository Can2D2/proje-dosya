using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleCrunches : MonoBehaviour, IExercise
{
    private bool leftElbowToRightKnee = false;
    private bool rightElbowToLeftKnee = false;
    public int counter = 0;
    public int Counter => counter;

    private const int kneeBendThreshold = 90;
    private const int kneeExtendThreshold = 150;
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
        
        int leftKneeAngle = angles[6];
        int rightKneeAngle = angles[7];

        if (!leftElbowToRightKnee && leftKneeAngle < kneeBendThreshold)
        {
            leftElbowToRightKnee = true;
        }
        if (!rightElbowToLeftKnee && rightKneeAngle < kneeBendThreshold)
        {
            rightElbowToLeftKnee = true;
        }

        if(leftElbowToRightKnee && rightKneeAngle >= kneeExtendThreshold){
            counter++;
            leftElbowToRightKnee = false;
        }
        if(rightElbowToLeftKnee && leftKneeAngle >= kneeExtendThreshold) {
            counter++;}
            rightElbowToLeftKnee = false;
    }
}
