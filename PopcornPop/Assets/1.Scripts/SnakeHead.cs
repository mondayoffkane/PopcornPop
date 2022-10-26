using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    public float JumpPower = 50f;

    Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //_rb.AddForce(Vector3.up * JumpPower);
            _rb.velocity = Vector3.up * JumpPower;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            //_rb.AddForce(Vector3.up * JumpPower);
            _rb.velocity = Vector3.up * JumpPower;
        }
    }
}
