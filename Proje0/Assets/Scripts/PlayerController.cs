using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    int currentCamIndex = 0;

<<<<<<< HEAD
    WebCamTexture tex;
    public RawImage display;
=======
    public Squats a;
    private int prevCounter = 0;

>>>>>>> 0ac31c5937ae2d52c2ff9ecbd5dffe4615471032
    Rigidbody rb;
    public float jumpForce;
    bool canJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        if (tex == null)
        {
            WebCamDevice device = WebCamTexture.devices[currentCamIndex];
            tex = new WebCamTexture(device.name);
            display.texture = tex;

            tex.Play();

        }
=======
        //Debug.Log("The value from ScriptA is: " + squats.counter);
        
>>>>>>> 0ac31c5937ae2d52c2ff9ecbd5dffe4615471032
    }

    // Update is called once per frame
    void Update()
    {

        if (a != null && a.counter > prevCounter && canJump) 
        { 
        
            rb.AddForce(Vector3.up *  jumpForce, ForceMode.Impulse);
            prevCounter = a.counter;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {

            canJump = false; 
        
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Obstacle")
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
