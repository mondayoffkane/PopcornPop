using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public float Speed = 5f;


    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Popcorn"))
        {
            other.GetComponent<Rigidbody>().AddForce(transform.forward * Speed);
            //other.GetComponent<Rigidbody>().velocity = transform.forward * Speed*0.1f;
        }
    }






}
