using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit_Floor : MonoBehaviour
{

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Popcorn"))
        {
            collision.transform.position = new Vector3(0f, 200f, 0f);
            collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.transform.SetParent(GameManager.instance.Waiting_Pool);
            collision.transform.gameObject.SetActive(false);
        }
    }
}
