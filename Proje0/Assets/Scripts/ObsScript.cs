using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsScript : MonoBehaviour
{
    public float speed;
    bool tf = true;
    // Start is called before the first frame update


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (tf)
        {
            float rnd = Random.Range(0f, 3f);
            float dis = 0f;
            if (rnd > 2f) dis = 2.5f;
            else if (rnd > 1f) dis = 0f;
            else dis = -2.5f;
            //Debug.Log(rnd);
            //rb.position.Set(rb.position.x, rb.position.y + rnd, rb.position.z);
            this.transform.position = new Vector3(this.transform.position.x+dis, this.transform.position.y , this.transform.position.z);
            tf = false;
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
