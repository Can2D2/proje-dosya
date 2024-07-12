using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agac : MonoBehaviour
{
    
    public float speed;
    bool tf=true;
    // Start is called before the first frame update
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tf)
        {
            float rnd = Random.Range(-15f, 10f);
            Debug.Log(rnd);
            //rb.position.Set(rb.position.x, rb.position.y + rnd, rb.position.z);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y +rnd, this.transform.position.z);
            tf = false;
        }
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
//-15 +10