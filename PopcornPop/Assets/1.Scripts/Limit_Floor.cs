using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit_Floor : MonoBehaviour
{

    GameManager _gamemanager;
    private void Start()
    {
        _gamemanager = GameManager.instance;
        GetComponent<MeshRenderer>().enabled = false;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Popcorn"))
        {
            collision.transform.position = new Vector3(0f, 0f, 0f);
            collision.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            collision.transform.SetParent(_gamemanager._spawner.Waiting_Pool);
            collision.transform.gameObject.SetActive(false);
        }
    }
}
