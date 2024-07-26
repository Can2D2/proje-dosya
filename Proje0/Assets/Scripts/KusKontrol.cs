using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class KusKontrol : MonoBehaviour
{
    int currentCamIndex = 0;

    WebCamTexture tex;
    public RawImage display;
    Rigidbody rb;
    public float hiz;

    private void Awake()
    {
            rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (tex == null)
        {
            WebCamDevice device = WebCamTexture.devices[currentCamIndex];
            tex = new WebCamTexture(device.name);
            display.texture = tex;

            tex.Play();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 pos = Input.mousePosition;
        pos.z = 54;
        pos = Camera.main.ScreenToWorldPoint(pos);
        //Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //rb.MovePosition(new Vector3(rb.position.x,pos.y,rb.position.z));
        float dif = pos.y - rb.position.y ;
        float kat = (dif < 5f) ? 0.5f : 1f;
        if (Math.Abs(dif)>0.3f)
        {
            if (dif > 0)
            {
                transform.Translate(Vector3.up*hiz*kat*Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector3.down * hiz *kat* Time.deltaTime); rb.velocity.Set(0f, -hiz, 0f);
            }
        }
        //Debug.Log(Camera.main.ScreenToWorldPoint(mousePos));
        //Debug.Log(rb.position);
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Obstacle"|| other.gameObject.tag == "Sinir")
        {
            if (tex != null)
            {
                display.texture = null;
                tex.Stop();
                tex = null;

            }
            SceneManager.LoadScene("Male UI");
        }
    }

}
