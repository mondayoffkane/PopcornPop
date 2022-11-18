using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MoreMountains.NiceVibrations;

public class Spawner : MonoBehaviour
{
    public enum Obj_Type
    {
        Popcorn,
        Gem,
        Coin,
        Ball
    }
    public Obj_Type _obj;

    public float Auto_Spawn_Interval = 1f;
    public int Spawn_Count = 1;

    public float Tap_Limit_Interval = 0.1f;
    public int Tap_Spawn_Count = 2;

    [SerializeField] float Current_Tap_Time = 0f;

    public float Power;
    public Transform Waiting_Pool;
    public Transform Using_Pool;

    public int Start_Pool_Size = 300;
    public int Add_Pool_Size = 100;
    public int Max_Pool_Size = 1000;

    public bool isFever = false;
    // //////////////////////////////
    [Space(10)]
    [Title("Serialize")]
    [SerializeField] float Auto_Current_Time;
    [SerializeField] Vector3 Default_Dir = new Vector3(-70f, 36f, 0f);
    [SerializeField] Transform Spawn_Pos;
    [SerializeField] Vector3 Shoot_Dir;
    public float Fever_Current_Time;
    public float Fever_Interval = 0.2f;
    public int Fever_Count = 5;

    [SerializeField] int Current_popcorn_count;
    [SerializeField] bool isMax = false;
    [SerializeField] int Waiting_Pool_Count;
    [SerializeField] int Using_Pool_Count;


    //////////////////// ////////
    [Space(10)]
    WaitForSeconds _wait;

    Coroutine _cor;

    private void OnEnable()
    {
        if (_cor != null)
        {
            StopCoroutine(_cor);

            _cor = null;

        }
        _cor = StartCoroutine(Cor_Update());

    }

    void Start()
    {
        Add_Pool(Start_Pool_Size);



        //StartCoroutine(Cor_Spawn());
        //StartCoroutine(Cor_Update());
        if (Spawn_Pos == null)
        {
            Spawn_Pos = transform.GetChild(0);
        }
    }





    //IEnumerator Cor_Spawn()
    //{
    //    _wait = new WaitForSeconds(Time.deltaTime);
    //    while (true)
    //    {
    //        if (Auto_Current_Time >= Auto_Spawn_Interval)
    //        {
    //            Auto_Current_Time = 0f;
    //            Spawn_Popcorn(Spawn_Count + GameManager.instance.Current_Popcorn_Level);
    //        }

    //        Current_Tap_Time += Time.deltaTime;
    //        Auto_Current_Time += Time.deltaTime;
    //        yield return _wait;


    //    }

    //}
    IEnumerator Cor_Update()
    {
        WaitForSeconds _deltatime = new WaitForSeconds(Time.deltaTime);
        while (true)
        {
            yield return null;
            if (Auto_Current_Time >= Auto_Spawn_Interval)
            {
                Auto_Current_Time = 0f;
                Spawn_Popcorn(Spawn_Count + GameManager.instance.Current_Popcorn_Level);
            }

            Current_Tap_Time += Time.deltaTime;
            Auto_Current_Time += Time.deltaTime;


            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.A))
            {
                if (Current_Tap_Time >= Tap_Limit_Interval)
                {
                    //Debug.Log("Click");
                    Current_Tap_Time = 0f;
                    Spawn_Popcorn(Tap_Spawn_Count + (int)(GameManager.instance.Current_Popcorn_Level * 0.5f));
                    MMVibrationManager.Haptic(HapticTypes.LightImpact);
                    if (isFever == false)
                    {
                        GameManager.instance.Fever_time++;
                        if (GameManager.instance.Fever_time > GameManager.instance.Max_Fever_time)
                            GameManager.instance.Fever_time = GameManager.instance.Max_Fever_time;
                    }
                }
            }

            if (isFever)
            {
                if (Fever_Current_Time >= Fever_Interval)
                {
                    Fever_Current_Time = 0f;
                    Spawn_Popcorn(Fever_Count);
                }
                Fever_Current_Time += Time.deltaTime;
            }

            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.S))
            {
                Spawn_Popcorn(15);
                if (isFever == false)
                {
                    GameManager.instance.Fever_time++;
                    if (GameManager.instance.Fever_time > GameManager.instance.Max_Fever_time)
                        GameManager.instance.Fever_time = GameManager.instance.Max_Fever_time;
                }
            }

            Waiting_Pool_Count = Waiting_Pool.childCount;
            Using_Pool_Count = Using_Pool.childCount;

        }
    }


    //private void Update()
    //{
    //    if (Auto_Current_Time >= Auto_Spawn_Interval)
    //    {
    //        Auto_Current_Time = 0f;
    //        Spawn_Popcorn(Spawn_Count + GameManager.instance.Current_Popcorn_Level);
    //    }

    //    Current_Tap_Time += Time.deltaTime;
    //    Auto_Current_Time += Time.deltaTime;


    //    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.A))
    //    {
    //        if (Current_Tap_Time >= Tap_Limit_Interval)
    //        {
    //            Debug.Log("Click");
    //            Current_Tap_Time = 0f;
    //            Spawn_Popcorn(Tap_Spawn_Count + (int)(GameManager.instance.Current_Popcorn_Level * 0.5f));
    //            MMVibrationManager.Haptic(HapticTypes.LightImpact);
    //            if (isFever == false)
    //            {
    //                GameManager.instance.Fever_time++;
    //                if (GameManager.instance.Fever_time > GameManager.instance.Max_Fever_time)
    //                    GameManager.instance.Fever_time = GameManager.instance.Max_Fever_time;
    //            }
    //        }
    //    }

    //    if (isFever)
    //    {
    //        if (Fever_Current_Time >= Fever_Interval)
    //        {
    //            Fever_Current_Time = 0f;
    //            Spawn_Popcorn(Fever_Count);
    //        }
    //        Fever_Current_Time += Time.deltaTime;
    //    }

    //    if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.S))
    //    {
    //        Spawn_Popcorn(15);
    //        if (isFever == false)
    //        {
    //            GameManager.instance.Fever_time++;
    //            if (GameManager.instance.Fever_time > GameManager.instance.Max_Fever_time)
    //                GameManager.instance.Fever_time = GameManager.instance.Max_Fever_time;
    //        }
    //    }

    //    Waiting_Pool_Count = Waiting_Pool.childCount;
    //    Using_Pool_Count = Using_Pool.childCount;
    //}



    void Spawn_Popcorn(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            if (Waiting_Pool.childCount <= 0)
            {
                Add_Pool(Add_Pool_Size);
            }
            if (Waiting_Pool.childCount > 0)
            {

                Rigidbody _popcorn = Waiting_Pool.GetChild(0).GetComponent<Rigidbody>();
                _popcorn.transform.SetParent(Using_Pool);
                _popcorn.transform.position = Spawn_Pos.position;
                _popcorn.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360f), Random.Range(0, 360f), Random.Range(0, 360f)));
                _popcorn.gameObject.SetActive(true);
                //_popcorn.GetComponent<Popcorn>().Price = GameManager.instance.SetPrice();
                Popcorn _corn = _popcorn.GetComponent<Popcorn>();

                //_corn.Price = GameManager.instance.Up_Income[GameManager.instance.Income_Level];
                //_corn.Price_Index = GameManager.instance.Income_Index;

                //Debug.Log(GameManager.instance.Up_Income[GameManager.instance.Income_Index]);
                //GameManager.instance.SetPopcorn_Price(ref _corn.Price, ref _corn.Price_Index);

                int _num = Random.Range(0, 10);

                Spawn_Pos.rotation = Quaternion.Euler(Default_Dir.x, Default_Dir.y * _num, Default_Dir.z);
                Shoot_Dir = Spawn_Pos.forward.normalized;
                _popcorn.AddTorque(Shoot_Dir * Power);
                _popcorn.AddForce(Shoot_Dir * Power);
            }
        }
    }

    void Add_Pool(int _count)
    {
        if (isMax == false)
        {
            for (int i = 0; i < _count; i++)
            {
                if (Current_popcorn_count < Max_Pool_Size)
                {
                    GameObject _popcorn = Instantiate(Resources.Load("Prefab/" + _obj.ToString()) as GameObject);
                    _popcorn.transform.SetParent(Waiting_Pool);
                    _popcorn.transform.position = Spawn_Pos.position;
                    _popcorn.SetActive(false);
                    Current_popcorn_count++;
                }
                else
                {
                    isMax = true;
                }
            }
        }
    }

}
