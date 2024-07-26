using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leg_raises : MonoBehaviour
{
    private bool c = false;
    public int counter = 0;
    private float hip_coor_min = 1f;
    private float hip_coor_max = 0f;
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
        
        int hip_angle = angles[4];
        if(!c && hip_angle < 100 && hip_angle > 80){
            c = true;
        }
        else if (c && hip_angle > 100){
            c = false;
            counter++;
        }
        float hip_coor = coordinates[6][1];
        if(hip_coor < hip_coor_min) hip_coor_min = hip_coor;
        if(hip_coor > hip_coor_max) hip_coor_max = hip_coor;

        if (hip_coor_max != hip_coor_min) {
            scaled_coor = (hip_coor - hip_coor_min) / (hip_coor_max - hip_coor_min);
        } else {
            scaled_coor = 0f;
        }
    }
}
