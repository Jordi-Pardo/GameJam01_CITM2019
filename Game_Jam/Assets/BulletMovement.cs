using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{


    public Rigidbody rb;
    public float speed;
    public Transform Spawn;

    public float LifeTime;

    private void Awake()
    {
        rb.velocity =  (this.gameObject.transform.forward * -1) * speed * Time.deltaTime;
        Destroy(this, LifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Zombie")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Zombie")
        {
            Destroy(this.gameObject);
        }
    }

}
