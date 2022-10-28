using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour
{
    public float Speed;

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Popcorn"))
        {
            other.GetComponent<Rigidbody>().velocity += transform.forward * Speed;
            //other.GetComponent<Rigidbody>().velocity = Vector3.back * Speed;
        }
    }
}
