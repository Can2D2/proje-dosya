using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{

    public Squats a;
    private int prevCounter = 0;

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
        //Debug.Log("The value from ScriptA is: " + squats.counter);
        
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
            SceneManager.LoadScene("Game2.1");
        }
    }

}
