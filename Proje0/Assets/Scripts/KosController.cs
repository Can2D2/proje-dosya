using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KosController : MonoBehaviour
{

    //public float speed;
    public float sideSpeed;

    public Transform centerPos;
    public Transform rightPos;
    public Transform leftPos;

    private int curPos;

    // Start is called before the first frame update
    void Start()
    {
        curPos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
        if (curPos == 0)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(centerPos.position.x, transform.position.y, transform.position.z), sideSpeed * Time.deltaTime);
        }
        if (curPos == 1)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(rightPos.position.x, transform.position.y, transform.position.z), sideSpeed * Time.deltaTime);
        }
        if (curPos == -1)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(leftPos.position.x, transform.position.y, transform.position.z), sideSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) if (curPos > -1) curPos--;

        if (Input.GetKeyDown(KeyCode.RightArrow)) if (curPos < 1) curPos++;
        


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Obstacle")
        {
            SceneManager.LoadScene("Game1.1");
        }
    }

}
