using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    int currentCamIndex = 0;

    WebCamTexture tex;
    public RawImage display;
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


        if (Input.GetMouseButtonDown(0) && canJump) 
        { 
        
            rb.AddForce(Vector3.up *  jumpForce, ForceMode.Impulse);

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
