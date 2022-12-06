using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;
using DG.Tweening;
//using System.Numerics;


public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    //public Transform Waiting_Pool, Using_Pool;
    public float Save_Interval = 1f;
    [SerializeField] float current_save_time = 0f;

    [Space(10)]
    [FoldoutGroup("Upgrade_Value")] public GameObject[] Stage_Group;
    [FoldoutGroup("Upgrade_Value")] public int Current_Max_Stage_Level;
    [FoldoutGroup("Upgrade_Value")] public int Current_Stage_Level = 0;
    [FoldoutGroup("Upgrade_Value")] public StageManager Current_StageManager;

    [FoldoutGroup("Upgrade_Value")] public int Current_Popcorn_Level; //  스폰갯수 증가
    [FoldoutGroup("Upgrade_Value")] public int Current_Popcorn_Max_Level;
    [FoldoutGroup("Upgrade_Value")] public double Current_Popcorn_Base_Price;
    [FoldoutGroup("Upgrade_Value")] public float Current_Popcorn_Upgrade_Scope;
    //[FoldoutGroup("Upgrade_Value")] public double[] Current_Popcorn_Upgrade_Price;

    [Space(10)]
    [FoldoutGroup("Upgrade_Value")] public int Current_Income_Level; // 업그레이드 레벨
    [FoldoutGroup("Upgrade_Value")] public int Current_Income_Max_Level;
    [FoldoutGroup("Upgrade_Value")] public double Current_Up_Income;
    [FoldoutGroup("Upgrade_Value")] public double Current_Up_Income_Base_Price;
    [FoldoutGroup("Upgrade_Value")] public float Current_Up_Income_Scope;
    [FoldoutGroup("Upgrade_Value")] public double Current_Income_Upgrade_Base_Price;
    [FoldoutGroup("Upgrade_Value")] public float Current_Income_Upgrade_Scope;
    //[FoldoutGroup("Upgrade_Value")] public double[] Current_Up_Income; // 금액대 리스트
    //[FoldoutGroup("Upgrade_Value")] public double[] Current_Income_Upgrade_Price;

    [Space(10)]
    [FoldoutGroup("Upgrade_Value")] public int Current_Obj_Max_Level = 5;
    [FoldoutGroup("Upgrade_Value")] public int Current_Object_Level;
    [FoldoutGroup("Upgrade_Value")] public double Current_Object_Base_Price;
    [FoldoutGroup("Upgrade_Value")] public float Current_Object_Upgrade_Scope;
    //[FoldoutGroup("Upgrade_Value")] public double[] Current_Obj_Upgrade_Price;
    [FoldoutGroup("Upgrade_Value")] public GameObject[] Current_Add_Object; // 맵 오브젝트
    [FoldoutGroup("Upgrade_Value")] public GameObject[] Current_Off_Object;
    [FoldoutGroup("Upgrade_Value")] public int Current_Off_Num;
    [FoldoutGroup("Upgrade_Value")] public double Current_NextLevel_Price;



    [Space(10)]
    [FoldoutGroup("UI")] public Text Money_Text;
    [FoldoutGroup("UI")] public Button[] Upgrade_Button;
    [FoldoutGroup("UI")] public Button Auto_Button;
    [FoldoutGroup("UI")] public Button Cam_Rotate_Button;
    [FoldoutGroup("UI")] public Button[] RV_Button_Group;
    [FoldoutGroup("UI")] public Button NextLevel_Button;
    Text NextLevel_Price_text;

    [FoldoutGroup("UI")] public GameObject Rot_Cam;
    [FoldoutGroup("UI")] public GameObject Full_Cam;

    [FoldoutGroup("UI")] public Button Setting_Button;
    [FoldoutGroup("UI")] public GameObject Setting_Panel;
    [FoldoutGroup("UI")] public Sprite[] Income_Img;


    [SerializeField] Text[] Upgrade_Button_Text;
    [SerializeField] Text Income_Text;
    [SerializeField] Text[] RV_Text;

    [FoldoutGroup("UI")] public GameObject MPS_Panel;
    [FoldoutGroup("UI")] public GameObject TapToSpawn_Img;
    [SerializeField] Text[] MPS_Text;
    public double[] MPS_Temp;
    [SerializeField] double[] MPS_value;
    [SerializeField] double[] Stage_Income;

    Button[] MPS_Button;

    [Space(10)]

    [FoldoutGroup("Floating_Money")] public int Start_Pool_Size = 100;
    [FoldoutGroup("Floating_Money")] public int Add_Pool_Size = 50;
    [FoldoutGroup("Floating_Money")] public Transform Floating_Waiting_Pool;
    [FoldoutGroup("Floating_Money")] public Transform Floating_Using_Pool;
    [FoldoutGroup("Floating_Money")] [ReadOnly] int Floating_Count;

    public Spawner _spawner;
    public float Fever_time;
    public float Max_Fever_time = 300f;


    [Space(10)]

    [FoldoutGroup("CPI")] public Vector3 Explosion_pos;
    [FoldoutGroup("CPI")] public float Explosion_power;
    [FoldoutGroup("CPI")] public float Explosion_radius;


    [SerializeField] float Start_x;
    [SerializeField] float End_x;
    [SerializeField] float Move_Distance = 0f;
    [SerializeField] float Limit_Distace = 0.2f;
    [SerializeField] Vector3 Start_Rot;
    public Transform Mouse_Rot;


    /// //////////////////////////////////////

    [Space(10)]
    [Title("Money Value")]
    public double Money;
    static readonly string[] CurrencyUnits = new string[] { "", "K", "M", "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };
    public double temp_money;

    Camera _maincam;
    Touch _touch;
    bool isClick = false;
    // ===============================================================================================

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            Application.targetFrameRate = 60;
        }
        StartCoroutine(Cor_Update());

        Init();

        _maincam = Camera.main;
        Upgrade_Button_Text = new Text[3];
        RV_Text = new Text[3];
        MPS_Text = new Text[3];
        MPS_value = new double[3];
        MPS_Temp = new double[3];
        Stage_Income = new double[3];
        MPS_Button = new Button[3];
    }

    void Init()
    {
        Stage_Group = new GameObject[3];
        DOTween.Sequence(TapToSpawn_Img.transform.DOScale(0.2f, 0.25f)
            .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Yoyo));

    }

    private void Start()
    {
        SetButton();
        for (int i = 0; i <= Current_Max_Stage_Level; i++)
        {
            Current_Stage_Level = i;
            SetStage();
            DataManager.instance.Save_Data();
        }

        StartCoroutine(Cor_MPS());
    }

    void SetStage()
    {
        DataManager.instance.Load_Data();
        //Current_Stage_Level = Current_StageManager.Stage_Level;
        if (Stage_Group[Current_Stage_Level] == null)
        {
            Stage_Group[Current_Stage_Level] = Instantiate(Resources.Load("Prefab/Stage/Stage_" + Current_Stage_Level) as GameObject);
        }

        for (int i = 0; i < Stage_Group.Length; i++)
        {
            try
            {
                Stage_Group[i].SetActive(false);
            }
            catch { }
        }
        Stage_Group[Current_Stage_Level].SetActive(true);
        Current_StageManager = Stage_Group[Current_Stage_Level].GetComponent<StageManager>();

        _maincam.backgroundColor = Current_StageManager.BackGround_Color;

        Current_StageManager.Popcorn_Level = DataManager.instance._gameData.Popcorn_Level[Current_Stage_Level];
        Current_StageManager.Income_Level = DataManager.instance._gameData.Income_Level[Current_Stage_Level];
        Current_StageManager.Object_Level = DataManager.instance._gameData.Object_Level[Current_Stage_Level];

        //Current_Popcorn_Upgrade_Price = Current_StageManager.Popcorn_Upgrade_Price;
        //Current_Income_Upgrade_Price = Current_StageManager.Income_Upgrade_Price;
        //Current_Obj_Upgrade_Price = Current_StageManager.Obj_Upgrade_Price;

        Current_Popcorn_Level = Current_StageManager.Popcorn_Level;
        Current_Income_Level = Current_StageManager.Income_Level;
        Current_Object_Level = Current_StageManager.Object_Level;
        Current_Obj_Max_Level = Current_StageManager.Obj_Max_Level;


        //Current_Up_Income = Current_StageManager.Up_Income;
        Current_Add_Object = Current_StageManager.Add_Object;
        Current_Off_Object = Current_StageManager.Off_Object;

        // 11.25 update
        Current_Popcorn_Max_Level = Current_StageManager.Popcorn_Max_Level;
        Current_Income_Max_Level = Current_StageManager.Income_Max_Level;
        Current_Obj_Max_Level = Current_StageManager.Obj_Max_Level;

        Current_Popcorn_Upgrade_Scope = Current_StageManager.Popcorn_Upgrade_Scope;
        Current_Up_Income_Scope = Current_StageManager.Up_Income_Scope;
        Current_Income_Upgrade_Scope = Current_StageManager.Income_Upgrade_Scope;
        Current_Object_Upgrade_Scope = Current_StageManager.Object_Upgrade_Scope;

        Current_Popcorn_Base_Price = Current_StageManager.Popcorn_Upgrade_Base_Price;
        Current_Up_Income_Base_Price = Current_StageManager.Up_Income_Base_Price;
        Current_Object_Base_Price = Current_StageManager.Object_Upgrade_Base_Price;
        Current_Income_Upgrade_Base_Price = Current_StageManager.Income_Upgrade_Base_Price;

        // 11.29 update
        Current_NextLevel_Price = Current_StageManager.NextLevel_Price;

        // 11.30 update
        Current_Off_Num = Current_StageManager.Off_Num;


        Current_Up_Income = Current_Income_Level + Current_Up_Income_Base_Price * Mathf.Pow(Current_Up_Income_Scope, Current_Income_Level);

        _spawner = Current_StageManager._spawner;
        Check_Level_Price();
        Check_Data();

        // 12.1 update
        Door_OnOff(false);
        RV_Super_Fever_bool = false;
        //Current_StageManager._spawner.isSuperFever = false;
        RV_Super_Fever_time = 0f;
        if (Current_StageManager._spawner.isSuperFever == true)
        {
            RV_Button_Group[2].interactable = false;
        }
        else
        {
            RV_Button_Group[2].interactable = true;

        }
        RV_Button_Group[2].transform.GetChild(1).gameObject.SetActive(true);
        RV_Super_Fever_time = 0;
        Stage_Income[Current_Stage_Level] = Current_Up_Income;

        for (int i = 0; i < 3; i++)
        {
            MPS_Button[i].interactable = true;
            MPS_Button[1].GetComponent<Image>().SetNativeSize();
            MPS_Button[i].gameObject.SetActive(false);
        }
        for (int i = 0; i <= Current_Max_Stage_Level; i++)
        {
            MPS_Button[i].gameObject.SetActive(true);
        }
        MPS_Button[Current_Stage_Level].interactable = false;
        MPS_Button[Current_Stage_Level].GetComponent<Image>().SetNativeSize();

        Current_StageManager._spawner.DataSet();
    }

    IEnumerator Cor_Update()
    {

        WaitForSeconds _deltatime = new WaitForSeconds(Time.deltaTime);

        while (true)
        {
            yield return null; // _deltatime;

            for (int i = 0; i < 3; i++)
            {
                temp_money = RV_Income_Double_bool ? Current_Up_Income * 2f : Current_Up_Income;
                double Temp_testMoney = RV_Income_Double_bool ? Stage_Income[i] * 2d : Stage_Income[i];
                MPS_value[i] = MPS_Temp[i] * Temp_testMoney;
            }


            Money_Text.text = ToCurrencyString(Money);

            RV_Text[0].text = RV_Spawn_Double_bool ? string.Format("00:{0:N0}", RV_Spawn_Double_time) : "Spawn X2";
            RV_Text[1].text = RV_Income_Double_bool ? string.Format("00:{0:N0}", RV_Income_Double_time) : "Income X2";
            RV_Text[2].text = RV_Super_Fever_bool ? string.Format("00:{0:N0}", RV_Super_Fever_time) : "Super Fever";

            MPS_Text[0].text = string.Format("{0}/s", ToCurrencyString(MPS_value[0]));
            MPS_Text[1].text = string.Format("{0}/s", ToCurrencyString(MPS_value[1]));
            MPS_Text[2].text = string.Format("{0}/s", ToCurrencyString(MPS_value[2]));
            //다른부분에 추가하기, 어떻게 산정할지도 계산하기

            Check_Button();

            InputKeyFunc();

        }
    }

    IEnumerator Cor_MPS()
    {
        WaitForSeconds _wait = new WaitForSeconds(1f);
        while (true)
        {
            yield return _wait;
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    if (Current_Stage_Level != i)
                    {
                        Money += MPS_value[i];

                    }
                }
            }
            catch { }
        }
    }


    //void Update()
    //{
    //    Money_Text.text = ToCurrencyString(Money);
    //    Check_Button();

    //    InputKeyFunc();
    //}

    // --------------------------------------------


    void InputKeyFunc()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Current_Off_Object[1].SetActive(false);
            Collider[] _cols = Physics.OverlapSphere(transform.position + Explosion_pos, Explosion_radius);
            foreach (Collider _col in _cols)
            {
                if (_col.transform.CompareTag("Popcorn"))
                {
                    _col.GetComponent<Rigidbody>().AddExplosionForce(Explosion_power, transform.position + Explosion_pos, Explosion_radius);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Current_Off_Object[1].SetActive(true);
        }

        current_save_time += Time.deltaTime;
        if (current_save_time >= Save_Interval)
        {
            current_save_time = 0f;
            DataManager.instance.Save_Data();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Current_Off_Object[2].SetActive(false);
            //Current_Off_Object[3].SetActive(true);
            //Current_Off_Object[4].SetActive(false);
            //Full_Cam.SetActive(true);
            Door_OnOff(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Current_Off_Object[2].SetActive(true);
            //Current_Off_Object[3].SetActive(false);
            //Current_Off_Object[4].SetActive(true);
            //Full_Cam.SetActive(false);
            Door_OnOff(false);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Money += 100000000;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            DataManager.instance.Init_Data();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DataManager.instance.Save_Data();
            Current_Stage_Level--;
            if (Current_Stage_Level < 0) Current_Stage_Level = 0;
            SetStage();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DataManager.instance.Save_Data();
            Current_Stage_Level++;
            if (Current_Stage_Level > 2) Current_Stage_Level = 2;
            SetStage();
        }

        // ///////////////////////////////////
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Start_x = Input.mousePosition.x;
            Start_Rot = Mouse_Rot.rotation.eulerAngles;
        }
        else if (Input.GetMouseButton(0))
        {
            End_x = Input.mousePosition.x;
            Move_Distance = End_x - Start_x;

            Mouse_Rot.rotation = Quaternion.Euler(Start_Rot + new Vector3(0f, Move_Distance * 0.1f, 0f));
            //Current_StageManager.transform.Rotate(new Vector3(0f, Move_Distance * Time.deltaTime, 0f));

        }
#endif
        /////////////////////////////////////////

#if UNITY_ANDROID
        // 12.06 update
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if (_touch.phase == TouchPhase.Began)
            {
                Start_x = _touch.position.x - _touch.deltaPosition.x;
                Start_Rot = Mouse_Rot.rotation.eulerAngles;
                if (isClick == false) { isClick = true; }
            }
            else if (_touch.phase == TouchPhase.Moved)
            {
                if (isClick == true)
                {
                    End_x = _touch.position.x - _touch.deltaPosition.x;
                    Move_Distance = End_x - Start_x;

                    Mouse_Rot.rotation = Quaternion.Euler(Start_Rot + new Vector3(0f, Move_Distance * 0.1f, 0f));
                }
            }
            else if (_touch.phase == TouchPhase.Ended)
            {
                isClick = false;
            }
        }
#endif



        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        //}
    }

    void Door_OnOff(bool isbool)
    {

        StartCoroutine(Cor());

        IEnumerator Cor()
        {

            //Current_Off_Object[2].SetActive(!isbool);
            //Current_Off_Object[4].SetActive(!isbool);
            if (isbool == false)
            {
                if (Current_StageManager._spawner.isSuperFever == true)
                {

                    RV_Super_Fever_bool = false;
                    Current_Off_Object[2].SetActive(true);
                    Current_Off_Object[4].SetActive(true);
                    yield return new WaitForSeconds(2f);
                    Current_Off_Object[3].SetActive(false);
                    Current_StageManager._spawner.isSuperFever = false;
                    RV_Button_Group[2].interactable = true;
                }
                else
                {
                    Current_Off_Object[2].SetActive(true);
                    Current_Off_Object[4].SetActive(true);
                    Current_Off_Object[3].SetActive(false);
                }
            }
            else
            {
                Current_Off_Object[2].SetActive(false);
                Current_Off_Object[4].SetActive(false);
                Current_Off_Object[3].SetActive(true);
            }

            Full_Cam.SetActive(isbool);
        }




    }


    public static string ToCurrencyString(double number)
    {
        string zero = "0";

        if (-1d < number && number < 1d)
        {
            return zero;
        }

        if (double.IsInfinity(number))
        {
            return "Infinity";
        }

        //  부호 출력 문자열
        string significant = (number < 0) ? "-" : string.Empty;

        //  보여줄 숫자
        string showNumber = string.Empty;

        //  단위 문자열
        string unityString = string.Empty;

        //  패턴을 단순화 시키기 위해 무조건 지수 표현식으로 변경한 후 처리
        string[] partsSplit = number.ToString("E").Split('+');

        //  예외
        if (partsSplit.Length < 2)
        {
            return zero;
        }

        //  지수 (자릿수 표현)
        if (!int.TryParse(partsSplit[1], out int exponent))
        {
            Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
            return zero;
        }

        //  몫은 문자열 인덱스
        int quotient = exponent / 3;

        //  나머지는 정수부 자릿수 계산에 사용(10의 거듭제곱을 사용)
        int remainder = exponent % 3;

        //  1A 미만은 그냥 표현
        if (exponent < 3)
        {
            showNumber = System.Math.Truncate(number).ToString();
        }
        else
        {
            //  10의 거듭제곱을 구해서 자릿수 표현값을 만들어 준다.
            var temp = double.Parse(partsSplit[0].Replace("E", "")) * System.Math.Pow(10, remainder);

            //  소수 둘째자리까지만 출력한다.
            showNumber = temp.ToString("F1").Replace(".0", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }



    public void Add_Floating_Pool(int _count)
    {
        Camera _cam = Camera.main;

        for (int i = 0; i < _count; i++)
        {
            GameObject _floating = Instantiate(Resources.Load("Prefab/Floating_Money") as GameObject);
            _floating.transform.SetParent(Floating_Waiting_Pool);
            _floating.transform.position = Floating_Waiting_Pool.position;
            _floating.SetActive(false);
            _floating.GetComponent<Canvas>().worldCamera = _cam;
            _floating.GetComponent<Canvas>().sortingOrder = Floating_Count;
            Floating_Count++;
        }
    }



    public void ManagerAddMoney()
    {

        //temp_money = RV_Income_Double_bool ? Current_Up_Income * 2f : Current_Up_Income;
        Money += temp_money;
    }

    //////////////////////////////////////////////////////////////////////////////////////////////
    ///  UI Button Func //////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////


    void Check_Button()
    {

        if (Current_Popcorn_Level < Current_Popcorn_Max_Level)
        {
            if (Money >= Current_Popcorn_Base_Price * MathF.Pow(Current_Popcorn_Upgrade_Scope, Current_Popcorn_Level))  //  Current_Popcorn_Upgrade_Price[Current_Popcorn_Level])
            {
                Upgrade_Button[0].interactable = true;
            }
            else
            {
                Upgrade_Button[0].interactable = false;
            }

        }
        else
        {
            Upgrade_Button[0].interactable = false;
        }


        if (Current_Income_Level < Current_Income_Max_Level)
        {

            if (Money > Current_Income_Upgrade_Base_Price * MathF.Pow(Current_Income_Upgrade_Scope, Current_Income_Level))
            {

                Upgrade_Button[1].interactable = true;
                Upgrade_Button[1].transform.GetChild(1).GetComponent<Image>().sprite = Income_Img[0];


            }
            else
            {
                Upgrade_Button[1].interactable = false;
                Upgrade_Button[1].transform.GetChild(1).GetComponent<Image>().sprite = Income_Img[1];
            }
        }
        else
        {
            Upgrade_Button[1].interactable = false;
            Upgrade_Button[1].transform.GetChild(1).GetComponent<Image>().sprite = Income_Img[1];
        }

        if (Current_Object_Level < Current_Obj_Max_Level)
        {
            if (Money > Current_Object_Base_Price * MathF.Pow(Current_Object_Upgrade_Scope, Current_Object_Level))
            {
                Upgrade_Button[2].interactable = true;
            }
            else
            {
                Upgrade_Button[2].interactable = false;
            }
        }
        else
        {
            Upgrade_Button[2].interactable = false;

        }


        // 11.29 update

        if (Current_Stage_Level < 2)
        {
            if (Current_Stage_Level == Current_Max_Stage_Level)
            {
                if (Money >= Current_NextLevel_Price)
                {
                    NextLevel_Button.interactable = true;
                }
                else
                {
                    NextLevel_Button.interactable = false;
                }
            }
            else if (Current_Stage_Level < Current_Max_Stage_Level)
            {
                NextLevel_Button.interactable = true;
            }
        }
        else
        {
            NextLevel_Button.interactable = false;
        }

        // ////////////

        //Auto_Button.transform.GetChild(0).GetComponent<Image>().DOFillAmount(Fever_time / 300f, 0.1f).SetEase(Ease.Linear);

        bool isbool;
        isbool = Fever_time >= Max_Fever_time ? true : false;

        Auto_Button.gameObject.SetActive(isbool);

    }

    void Check_Level_Price()
    {

        Upgrade_Button_Text[0].text =
            Current_Popcorn_Level >= Current_Popcorn_Max_Level ? "Max" : ToCurrencyString(Current_Popcorn_Base_Price * MathF.Pow(Current_Popcorn_Upgrade_Scope, Current_Popcorn_Level));

        Upgrade_Button_Text[1].text =
            Current_Income_Level >= Current_Income_Max_Level ? "Max" : ToCurrencyString(Current_Income_Upgrade_Base_Price * MathF.Pow(Current_Income_Upgrade_Scope, Current_Income_Level));

        Upgrade_Button_Text[2].text =
           Current_Object_Level >= Current_Obj_Max_Level ? "Max" : ToCurrencyString(Current_Object_Base_Price * MathF.Pow(Current_Object_Upgrade_Scope, Current_Object_Level));

        Income_Text.text = Current_Income_Level < Current_Income_Max_Level ?
            string.Format("{0}➜{1}", ToCurrencyString(Current_Up_Income), ToCurrencyString(Current_Income_Level + 1 + Current_Up_Income_Base_Price * Mathf.Pow(Current_Up_Income_Scope, Current_Income_Level + 1)))
            : string.Format("{0}", ToCurrencyString(Current_Up_Income));
        //Income_Text.text = string.Format("{0}➜{1}", ToCurrencyString(Current_Up_Income), ToCurrencyString(Current_Up_Income * Current_Up_Income_Scope));

        NextLevel_Price_text.text =
            Current_Stage_Level == Current_Max_Stage_Level ? string.Format("{0}", ToCurrencyString(Current_NextLevel_Price)) : "";


        //DataManager.instance.Save_Data();
    }



    public void Popcorn_Upgrade()
    {
        Money -= Current_Popcorn_Base_Price * MathF.Pow(Current_Popcorn_Upgrade_Scope, Current_Popcorn_Level);

        Current_Popcorn_Level++;
        Current_StageManager.Popcorn_Level = Current_Popcorn_Level;
        DataManager.instance.Save_Data();
        Check_Level_Price();
        //Stage_Income[Current_Stage_Level] = Current_Up_Income;
    }
    public void Income_Upgrade()
    {
        Money -= Current_Income_Upgrade_Base_Price * MathF.Pow(Current_Income_Upgrade_Scope, Current_Income_Level);
        Current_Income_Level++;
        Current_StageManager.Income_Level = Current_Income_Level;
        Current_Up_Income = Current_Income_Level + Current_Up_Income_Base_Price * Mathf.Pow(Current_Up_Income_Scope, Current_Income_Level);
        DataManager.instance.Save_Data();
        Check_Level_Price();
        Stage_Income[Current_Stage_Level] = Current_Up_Income;
    }
    public void Obj_Upgrade()
    {
        Money -= Current_Object_Base_Price * MathF.Pow(Current_Object_Upgrade_Scope, Current_Object_Level);
        Current_Object_Level++;
        Current_StageManager.Object_Level = Current_Object_Level;
        DataManager.instance.Save_Data();
        if (Current_Object_Level == Current_Off_Num)
        {
            Current_Off_Object[0].SetActive(false);
        }
        Current_Add_Object[Current_Object_Level - 1].SetActive(true);
        Current_Add_Object[Current_Object_Level - 1].transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        Check_Level_Price();
    }

    void Auto_Tap()
    {

        _spawner.isFever = true;

        DOTween.Sequence().AppendCallback(() =>
            {
                //Auto_Button.interactable = false;
                Auto_Button.gameObject.SetActive(false);
                DOTween.To(() => Fever_time, x => Fever_time = x, 0, 20f).SetEase(Ease.Linear);

            }).AppendInterval(20f)
        .OnComplete(() => _spawner.isFever = false);

    }


    void SetButton()
    {
        Upgrade_Button[0].onClick.AddListener(() => Popcorn_Upgrade());
        Upgrade_Button[1].onClick.AddListener(() => Income_Upgrade());
        Upgrade_Button[2].onClick.AddListener(() => Obj_Upgrade());

        RV_Button_Group[0].onClick.AddListener(() => RV_Spawn_Dobule());
        RV_Button_Group[1].onClick.AddListener(() => RV_Income_Double());
        RV_Button_Group[2].onClick.AddListener(() => RV_Super_Fever());


        Upgrade_Button_Text[0] = Upgrade_Button[0].transform.GetChild(0).GetComponent<Text>();
        Upgrade_Button_Text[1] = Upgrade_Button[1].transform.GetChild(0).GetComponent<Text>();
        Upgrade_Button_Text[2] = Upgrade_Button[2].transform.GetChild(0).GetComponent<Text>();

        Income_Text = Upgrade_Button[1].transform.GetChild(1).GetChild(0).GetComponent<Text>();

        RV_Text[0] = RV_Button_Group[0].transform.GetChild(0).GetComponent<Text>();
        RV_Text[1] = RV_Button_Group[1].transform.GetChild(0).GetComponent<Text>();
        RV_Text[2] = RV_Button_Group[2].transform.GetChild(0).GetComponent<Text>();

        NextLevel_Button.onClick.AddListener(() => NextLevel());
        NextLevel_Price_text = NextLevel_Button.transform.GetChild(0).GetComponent<Text>();

        Auto_Button.onClick.AddListener(() => Auto_Tap());
        Cam_Rotate_Button.onClick.AddListener(() => Cam_Rot());

        // 11.30 update
        Setting_Button.onClick.AddListener(() => Setting_OnOff());
        Setting_Panel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => Setting_OnOff());
        Setting_Panel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => Sound_OnOff());
        Setting_Panel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => Vibe_OnOff());

        // 12.01 update
        MPS_Button[0] = MPS_Panel.transform.GetChild(0).GetComponent<Button>();
        MPS_Button[1] = MPS_Panel.transform.GetChild(1).GetComponent<Button>();
        MPS_Button[2] = MPS_Panel.transform.GetChild(2).GetComponent<Button>();

        MPS_Text[0] = MPS_Button[0].transform.GetChild(0).GetComponent<Text>();
        MPS_Text[1] = MPS_Button[1].transform.GetChild(0).GetComponent<Text>();
        MPS_Text[2] = MPS_Button[2].transform.GetChild(0).GetComponent<Text>();

        for (int i = 0; i < 3; i++)
        {
            var temp = i;
            MPS_Button[i].onClick.AddListener(() =>
            {
                DataManager.instance.Save_Data();
                Current_Stage_Level = temp;
                Check_Level_Price();

                SetStage();
            });
        }

    }

    public bool RV_Spawn_Double_bool;
    float RV_Spawn_Double_time;
    void RV_Spawn_Dobule() // double spawn count
    {
        RV_Spawn_Double_bool = true;
        RV_Button_Group[0].transform.GetChild(1).gameObject.SetActive(false);

        DOTween.Sequence().AppendCallback(() =>
        {
            //RV_Button_Group[0].gameObject.SetActive(false);
            RV_Button_Group[0].interactable = false;
            DOTween.To(() => 30f, x => RV_Spawn_Double_time = x, 0, 30f).SetEase(Ease.Linear);

        }).AppendInterval(30f)
        .OnComplete(() =>
        {
            RV_Spawn_Double_bool = false;
            //RV_Button_Group[0].gameObject.SetActive(true);
            RV_Button_Group[0].interactable = true;
            RV_Button_Group[0].transform.GetChild(1).gameObject.SetActive(true);
        });
    }

    //public bool RV_Spawn_Special_bool;
    //float RV_Spawn_Special_time;
    //void RV_Spawn_Special() // spawn something special
    //{
    //    RV_Spawn_Special_bool = true;
    //    DOTween.Sequence().AppendCallback(() =>
    //    {
    //        RV_Button_Group[1].gameObject.SetActive(false);
    //        DOTween.To(() => RV_Spawn_Special_time, x => RV_Spawn_Special_time = x, 0, 30f).SetEase(Ease.Linear);

    //    }).AppendInterval(30f)
    //    .OnComplete(() =>
    //    {
    //        RV_Spawn_Special_bool = false;
    //        RV_Button_Group[1].gameObject.SetActive(true);
    //    });
    //}

    public bool RV_Income_Double_bool;
    float RV_Income_Double_time;
    void RV_Income_Double() // double income
    {
        RV_Income_Double_bool = true;
        RV_Button_Group[1].transform.GetChild(1).gameObject.SetActive(false);
        DOTween.Sequence().AppendCallback(() =>
        {
            //RV_Button_Group[1].gameObject.SetActive(false);
            RV_Button_Group[1].interactable = false;
            DOTween.To(() => 30f, x => RV_Income_Double_time = x, 0, 30f).SetEase(Ease.Linear);

        }).AppendInterval(30f)
        .OnComplete(() =>
        {
            RV_Income_Double_bool = false;
            //RV_Button_Group[1].gameObject.SetActive(true);
            RV_Button_Group[1].interactable = true;
            RV_Button_Group[1].transform.GetChild(1).gameObject.SetActive(true);
        });
    }

    public bool RV_Super_Fever_bool;
    float RV_Super_Fever_time;
    void RV_Super_Fever() // super spawn object and open 4 door
    {
        RV_Super_Fever_bool = true;
        Current_StageManager._spawner.isSuperFever = true;
        RV_Button_Group[2].transform.GetChild(1).gameObject.SetActive(false);
        DOTween.Sequence().AppendCallback(() =>
        {
            //RV_Button_Group[2].gameObject.SetActive(false);
            RV_Button_Group[2].interactable = false;
            Door_OnOff(true);
            DOTween.To(() => 30f, x => RV_Super_Fever_time = x, 0, 30f).SetEase(Ease.Linear);

        }).AppendInterval(30f)
        .OnComplete(() =>
        {
            Door_OnOff(false);
            RV_Super_Fever_bool = false;
            Current_StageManager._spawner.isSuperFever = false;
            //RV_Button_Group[2].gameObject.SetActive(true);
            RV_Button_Group[2].interactable = true;
            RV_Button_Group[2].transform.GetChild(1).gameObject.SetActive(true);
        });
    }

    void NextLevel()
    {
        if (Current_Stage_Level == Current_Max_Stage_Level)
        {
            Money -= Current_NextLevel_Price;
            Current_Max_Stage_Level++;
        }

        DataManager.instance.Save_Data();
        Current_Stage_Level++;
        Check_Level_Price();

        SetStage();




    }


    /// <summary>
    /// Other Func /////////////////////////////////
    /// </summary>

    void Check_Data()
    {

        for (int i = 1; i <= Current_Object_Level; i++)
        {
            Current_Add_Object[i - 1].SetActive(true);

            if (i == Current_Off_Num)
            {
                Current_Off_Object[0].SetActive(false);
            }
        }

    }


    void Cam_Rot()
    {
        //Debug.Log("Rot Cam");
        Rot_Cam.SetActive(!Rot_Cam.activeSelf);
        if (Rot_Cam.activeSelf == false)
        {
            Mouse_Rot.rotation = Quaternion.Euler(Vector3.zero);

        }
        //Current_StageManager.Rotate_Button();
    }


    void Setting_OnOff()
    {
        Setting_Panel.SetActive(!Setting_Panel.activeSelf);
        //Debug.Log("onoff");
    }

    void Sound_OnOff()
    {

    }

    void Vibe_OnOff()
    {

    }

}
