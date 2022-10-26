using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sling : MonoBehaviour
{
    public GameObject Ball;

    public Vector3 Start_Pos; // 마우스 초기위치 
    public Vector3 End_Pos; // 마우스 이동위치 
    public Vector3 _pos; // 공 생성위치 
    public float _distance; // 거리값 ( 공 날리는 힘으로 사용 )
    public Transform _ball;
    public float _sense = 0.02f; // 유니티 엔진 값 보정
    
    
    [SerializeField] LineRenderer _line;

    public float Parabola_time = 2f; // 최대 보여줄 시간 
    public float Parabola_Division_time = 0.01f; // 시간 분할 값 ( 라인포지션 시간 간격 )
    
    public Vector3 Dir2;
    private void Start()
    {
        _line = GetComponent<LineRenderer>();
    }


    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Start_Pos = Input.mousePosition;
            _pos = Camera.main.ScreenToWorldPoint(Start_Pos);
            _ball = Instantiate(Ball, new Vector3(_pos.x, _pos.y, 0f), Quaternion.identity).transform;
        }
        else if (Input.GetMouseButton(0))
        {
            End_Pos = Input.mousePosition;

            Dir2 = new Vector3(Start_Pos.x - End_Pos.x, Start_Pos.y - End_Pos.y, 0f).normalized;


            _distance = Vector3.Distance(Start_Pos, End_Pos);

            SetLine();
          
        }
        else if (Input.GetMouseButtonUp(0))
        {

            _ball.GetComponent<Rigidbody>().isKinematic = false;
            _ball.GetComponent<Rigidbody>().AddForce(Dir2 * _distance);
        }

    }

    void SetLine()
    {
        float _time = 0f;
        _line.positionCount = (int)(Parabola_time / Parabola_Division_time);

        for (int i = 0; i < Parabola_time / Parabola_Division_time; i++)
        {
            _line.SetPosition(i,
                new Vector3(_pos.x, _pos.y, 0f)
                + Dir2 * _distance * _sense // v0
                * _time // t
                + Vector3.up * 0.5f * Physics.gravity.y * _time * _time); // -1/2gt*t

            _time += Parabola_Division_time;

        }

        // 시간에 따른 좌표값 계산 
        // X = V0 * Cos() *t
        // Y = V0 * Sin() *t -1/2*g*t*t

        // 근데 여기서는 각도를 안쓰므로 삼각함ㅅ 생략해서 아래처럼 한번에 묶음 
        // => V0 *t -1/2*g*t*t

    }



}
