using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance_Force : MonoBehaviour
{
    public float Min_Force = 5f;
    public float Max_Force = 10f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Popcorn"))
        {
            var _force = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            other.GetComponent<Rigidbody>().AddForce(_force * Random.Range(Min_Force, Max_Force));
        }
    }
}
