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

    [SerializeField] Material Base_Mat;
    [SerializeField] Mesh Base_Mesh;

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
    public bool isSuperFever = false;
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

    public float SuperFever_Current_Time;
    public float SuperFever_Interval = 0.1f;
    public int SuperFever_Count = 10;

    [SerializeField] int Current_popcorn_count;
    [SerializeField] bool isMax = false;
    [SerializeField] int Waiting_Pool_Count;
    [SerializeField] int Using_Pool_Count;

    [ReadOnly] [SerializeField] int Total_Spawn_Count_Auto;
    [ReadOnly] [SerializeField] int Total_Spawn_Count_Tap;
    GameManager _gamemanager;
    StageManager _stagemanager;
    //////////////////// ////////
    [Space(10)]
    WaitForSeconds _wait;

    Coroutine _cor;

    private void Awake()
    {
        //Base_Mat = (Resources.Load("Prefab/" + _obj.ToString()) as GameObject).GetComponent<Renderer>().GetMaterial;
        //Base_Mesh = (Resources.Load("Prefab/" + _obj.ToString()) as GameObject).GetComponent<MeshFilter>().sharedMesh;
    }


    private void OnEnable()
    {
        if (_gamemanager == null)
            _gamemanager = GameManager.instance;
        if (_stagemanager == null)
            _stagemanager = transform.root.GetComponent<StageManager>();


        if (_cor != null)
        {
            StopCoroutine(_cor);

            _cor = null;

        }
        _cor = StartCoroutine(Cor_Update());

        isOpen = false;

    }

    public void DataSet() // 게임 시작시 MPS 값 초기화 용
    {
        Total_Spawn_Count_Auto = _gamemanager.RV_Spawn_Double_bool
                ? (Spawn_Count + _stagemanager.Popcorn_Level) * 2
                : Spawn_Count + _stagemanager.Popcorn_Level;

        Total_Spawn_Count_Tap = _gamemanager.RV_Spawn_Double_bool
            ? (Tap_Spawn_Count + _stagemanager.Popcorn_Level)
            : Tap_Spawn_Count + (int)(_stagemanager.Popcorn_Level * 0.5f);

        _gamemanager.MPS_Temp[_gamemanager.Current_Stage_Level]
              = (Total_Spawn_Count_Auto + (Total_Spawn_Count_Tap * 4f));

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
    //            Spawn_Popcorn(Spawn_Count + _gamemanager.Current_Popcorn_Level);
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
            Total_Spawn_Count_Auto = _gamemanager.RV_Spawn_Double_bool
                ? (Spawn_Count + _gamemanager.Current_Popcorn_Level) * 2
                : Spawn_Count + _gamemanager.Current_Popcorn_Level;

            Total_Spawn_Count_Tap = _gamemanager.RV_Spawn_Double_bool
                ? (Tap_Spawn_Count + _gamemanager.Current_Popcorn_Level)
                : Tap_Spawn_Count + (int)(_gamemanager.Current_Popcorn_Level * 0.5f);

            if (Auto_Current_Time >= Auto_Spawn_Interval)
            {
                Auto_Current_Time = 0f;
                Spawn_Popcorn(Total_Spawn_Count_Auto);
            }

            Current_Tap_Time += Time.deltaTime;
            Auto_Current_Time += Time.deltaTime;

            if (Current_Tap_Time >= 5f)
            {
                if (_gamemanager.TapToSpawn_Img.activeSelf == false)
                {
                    _gamemanager.TapToSpawn_Img.SetActive(true);
                }
            }

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.A))
            {
                if (Current_Tap_Time >= Tap_Limit_Interval)
                {
                    //Debug.Log("Click");
                    Current_Tap_Time = 0f;
                    if (_gamemanager.TapToSpawn_Img.activeSelf == true)
                    {
                        _gamemanager.TapToSpawn_Img.SetActive(false);
                    }

                    Spawn_Popcorn(Total_Spawn_Count_Tap);
                    MMVibrationManager.Haptic(HapticTypes.LightImpact);
                    if (isFever == false)
                    {
                        _gamemanager.Fever_time++;
                        if (_gamemanager.Fever_time > _gamemanager.Max_Fever_time)
                            _gamemanager.Fever_time = _gamemanager.Max_Fever_time;
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
                    _gamemanager.Fever_time++;
                    if (_gamemanager.Fever_time > _gamemanager.Max_Fever_time)
                        _gamemanager.Fever_time = _gamemanager.Max_Fever_time;
                }
            }

            if (isSuperFever)
            {
                SuperFever_Current_Time += Time.deltaTime;
                if (SuperFever_Current_Time >= SuperFever_Interval)
                {
                    SuperFever_Current_Time = 0;
                    Spawn_Popcorn(SuperFever_Count);
                }
            }



            Waiting_Pool_Count = Waiting_Pool.childCount;
            Using_Pool_Count = Using_Pool.childCount;

            if (Using_Pool_Count > Max_Pool_Size * 0.8f)
            {
                if (isOpen == false && isSuperFever == false)
                {
                    StartCoroutine(CheckDoor());
                }
            }

            _gamemanager.MPS_Temp[_gamemanager.Current_Stage_Level]
                = (Total_Spawn_Count_Auto + (Total_Spawn_Count_Tap * 4f));
            yield return null;
        }
    }
    bool isOpen = false;
    IEnumerator CheckDoor()
    {
        isOpen = true;
        Door_OnOff(true);
        yield return new WaitForSeconds(10f);
        isOpen = false;
        if (isSuperFever == false)
        {
            Door_OnOff(false);
        }
    }



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


                if (!_popcorn.GetComponent<Renderer>().material.Equals(Base_Mat))
                {
                    _popcorn.GetComponent<Renderer>().material = Base_Mat;
                }
                else
                {

                }
                if (!_popcorn.GetComponent<MeshFilter>().sharedMesh.Equals(Base_Mesh))
                {
                    _popcorn.GetComponent<MeshFilter>().sharedMesh = Base_Mesh;

                }



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

    void Door_OnOff(bool isbool)
    {

        StartCoroutine(Cor());

        IEnumerator Cor()
        {

            _stagemanager.Off_Object[2].SetActive(!isbool);
            _stagemanager.Off_Object[4].SetActive(!isbool);

            if (isbool == false)
            {
                yield return new WaitForSeconds(2f);
                _stagemanager.Off_Object[3].SetActive(false);
            }
            else
            {
                _stagemanager.Off_Object[3].SetActive(true);
            }
            //if (isbool == false && _stagemanager.Off_Object[3].activeSelf == true)
            //{
            //}
            //else if (isbool == true)
            //{
            //}
            //_gamemanager.Full_Cam.SetActive(isbool);
        }

    }

}
