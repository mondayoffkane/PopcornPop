using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshChanger : MonoBehaviour
{

    public Mesh Change_Mesh;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Popcorn"))
        {
            other.GetComponent<MeshFilter>().sharedMesh = Change_Mesh;
        }
    }
}
