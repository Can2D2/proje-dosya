using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor_tricep_dips : MonoBehaviour, IExercise
{
    private const int elbow_upper_threshold = 170;
    private const int elbow_lower_threshold = 150;
    private bool c = false;
    public int counter = 0;
    public int Counter => counter;

    private float hip_coor_max = 0f;
    private float hip_coor_min = 1f;
    public float scaled_coor = 0f;
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
        
        int knee_angle = angles[6];
        int elbow_angle = angles[2];
        float hip_coor = coordinates[6][1];

        if(!c && elbow_angle > elbow_upper_threshold){
            c = true;
        }
        else if(c && elbow_angle < elbow_lower_threshold){
            c = false;
            counter++;
        }

        if(hip_coor < hip_coor_min) hip_coor_min = hip_coor;
        if(hip_coor > hip_coor_max) hip_coor_max = hip_coor;

        if (hip_coor_max != hip_coor_min) {
            scaled_coor = (hip_coor - hip_coor_min)/(hip_coor_max - hip_coor_min);
        } 
        else {
            scaled_coor = 0f;
        }

        if(knee_angle < 80 || knee_angle > 100){
            // optional
        }
    }
}
