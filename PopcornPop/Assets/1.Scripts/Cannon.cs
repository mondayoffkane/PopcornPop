using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Cannon : MonoBehaviour
{

    public float _sense = 0.02f;
    public float Power = 3f;
    public float Line_MaxTime = 2f;
    public float Line_DivisionTime = 0.01f;
    public float Shoot_Interval = 0.2f;
    public int Max_Bullet_Count, Current_Bullet_Count;

    public Vector3 Dir;

    [Space(10)]
    [Title("Serialize")]
    [SerializeField] Vector3 Start_Pos;
    [SerializeField] Vector3 End_Pos;
    [SerializeField] Transform Bullet_Spawn_Pos;
    [SerializeField] float _distance;
    [SerializeField] Transform Current_Bullet;
    [SerializeField] LineRenderer _line;
    [SerializeField] WaitForSeconds _waitseconds;



    void Start()
    {
        _line = GetComponent<LineRenderer>();
        Bullet_Spawn_Pos = transform.GetChild(0);
        _waitseconds = new WaitForSeconds(Shoot_Interval);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Start_Pos = Input.mousePosition;

        }
        else if (Input.GetMouseButton(0))
        {
            End_Pos = Input.mousePosition;
            Dir = new Vector3(Start_Pos.x - End_Pos.x, Start_Pos.y - End_Pos.y, 0f).normalized;


            _distance = Vector3.Distance(Start_Pos, End_Pos);

            DrawLine();
            transform.LookAt(transform.position + Dir);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(Cor_Shoot());
        }



    }

    void DrawLine()
    {
        float _time = 0f;
        _line.positionCount = (int)(Line_MaxTime / Line_DivisionTime);

        for (int i = 0; i < Line_MaxTime / Line_DivisionTime; i++)
        {
            _line.SetPosition(i,
                Bullet_Spawn_Pos.position
                + Dir * _distance * Power * _sense * _time
                + Vector3.up * 0.5f * Physics.gravity.y * _time * _time);
            _time += Line_DivisionTime;
        }

    }

    IEnumerator Cor_Shoot()
    {
        for (int i = 0; i < Max_Bullet_Count; i++)
        {

            Current_Bullet = (Instantiate(Resources.Load("Prefab/Bullet"), Bullet_Spawn_Pos.position, Quaternion.identity)
                as GameObject).transform;

            Current_Bullet.GetComponent<Rigidbody>().AddForce(Dir * _distance * Power);



            yield return _waitseconds;
        }
    }


}
