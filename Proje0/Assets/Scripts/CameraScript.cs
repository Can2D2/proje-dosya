using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{

    int currentCamIndex = 0;

    WebCamTexture tex;
    public RawImage display;

    public void CameraStart()
    {
        if (tex == null)
        {
        WebCamDevice device = WebCamTexture.devices[currentCamIndex];
        tex = new WebCamTexture(device.name);
        display.texture = tex;

        tex.Play();

        }
    }
    public void CameraStop()
    {
        if (tex != null) 
        { 
        display.texture = null;
        tex.Stop();
        tex = null;
        
        }
    }
    private void Start()
    {
        
        if (tex == null)
        {
            WebCamDevice device = WebCamTexture.devices[currentCamIndex];
            tex = new WebCamTexture(device.name);
            display.texture = tex;

            tex.Play();
        }
        
            else 
            {
                display.texture = null;
                tex.Stop();
                tex = null;
            }

        }
    private void OnApplicationQuit()
    {
        if (tex != null)
        {
            display.texture = null;
            tex.Stop();
            tex = null;
        }
    }

}
