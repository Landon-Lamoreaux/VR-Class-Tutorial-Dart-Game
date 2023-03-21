using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeStick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.Contains("Dart"))
        {
            Rigidbody body = collision.gameObject.gameObject.GetComponent<Rigidbody>();
            body.isKinematic = true;

        }
    }
    


}
