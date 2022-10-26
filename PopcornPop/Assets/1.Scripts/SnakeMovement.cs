using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{

    public List<Transform> BodyParts = new List<Transform>();

    public float mindistance = 0.25f;

    public int beginsize;


    public float speed = 1f;
    public float rotationspeed = 50f;

    public GameObject bodyprefab;


    [SerializeField] float dis;
    [SerializeField] Transform curBodyPart;
    [SerializeField] Transform PrevBodyPart;


    void Start()
    {
        for (int i = 0; i < beginsize - 1; i++)
        {
            AddBodyPart();
        }
    }


    //void Update()
    //{
    //    Move();

    //    if (Input.GetKeyDown(KeyCode.Q))
    //        AddBodyPart();
    //}

    private void LateUpdate()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Q))
            AddBodyPart();
    }

    public void Move()
    {
        float curspeed = speed;
        if (Input.GetKey(KeyCode.W))
            curspeed *= 2;

        BodyParts[0].Translate(BodyParts[0].forward * curspeed * Time.smoothDeltaTime, Space.World);
        //BodyParts[0].Translate(Vector3.forward * curspeed * Time.smoothDeltaTime, Space.World);

        if (Input.GetAxis("Horizontal") != 0)
            BodyParts[0].Rotate(Vector3.up * rotationspeed * Time.deltaTime * Input.GetAxis("Horizontal"));

        for (int i = 1; i < BodyParts.Count; i++)
        {
            curBodyPart = BodyParts[i];
            PrevBodyPart = BodyParts[i - 1];

            dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);
            Vector3 newpos = PrevBodyPart.position;


            float T = Time.deltaTime * dis / mindistance * curspeed;

            if (T > 0.5f)
                T = 0.5f;
            curBodyPart.position = Vector3.Slerp(curBodyPart.position, /* newpos*/ PrevBodyPart.position, T);
            curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, PrevBodyPart.rotation, T);
        }
    }


    public void AddBodyPart()
    {
        Transform newpart = (Instantiate(bodyprefab, BodyParts[BodyParts.Count - 1].position, BodyParts[BodyParts.Count - 1].rotation) as GameObject).transform;

        newpart.SetParent(transform);
        BodyParts.Add(newpart);
    }



}
