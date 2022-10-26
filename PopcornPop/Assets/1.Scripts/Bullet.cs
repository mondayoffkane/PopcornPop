using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Change_Color"))
        {
        
            GetComponent<MeshRenderer>().material.color
                = other.GetComponent<MeshRenderer>().material.color;
        }
    }
}
