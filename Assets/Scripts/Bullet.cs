using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.MovePosition(rigidbody.position + rigidbody.rotation * (Vector3.forward * 10f * Time.fixedDeltaTime));

    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
