using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using MondayOFF;



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
    [FoldoutGroup("UI")] public Button Previous_Button;
    Text NextLevel_Price_text;

    [FoldoutGroup("UI")] public GameObject Rot_Cam;
    [FoldoutGroup("UI")] public GameObject Full_Cam;

    [FoldoutGroup("UI")] public Button Setting_Button;
    [FoldoutGroup("UI")] public GameObject Setting_Panel;
    [FoldoutGroup("UI")] public Sprite[] Upgrade_Img;
    [FoldoutGroup("UI")] public Sprite[] Income_Img;
    [FoldoutGroup("UI")] public GameObject MPS_Panel;
    [FoldoutGroup("UI")] public GameObject TapToSpawn_Img;
    [FoldoutGroup("UI")] public double[] MPS_Temp;
    [FoldoutGroup("UI")] public GameObject Order_Panel;
    [FoldoutGroup("UI")] public GameObject Fever_Img_Group;
    [FoldoutGroup("UI")] public GameObject Fever_Effect;

    [FoldoutGroup("UI")] public Button NoAds_Button;
    [FoldoutGroup("UI")] public GameObject NoAds_Panel;

    Text[] Upgrade_Button_Text;
    Text Income_Text;
    Text[] RV_Text;

    Text[] MPS_Text;
    double[] MPS_value;
    double[] Stage_Income;
    Toggle Sound_Toggle;
    Toggle Vibe_Toggle;

    Button[] MPS_Button;

    Button Recive_Button;


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


    float Start_x;
    float End_x;
    float Move_Distance = 0f;
    [SerializeField] float Limit_Distace = 0.2f;
    Vector3 Start_Rot;
    public Transform Mouse_Rot;

    public bool isSound = true;
    public bool isVibe = true;
    public Color Fever_Color;
    [FoldoutGroup("Sound")] public AudioClip[] _clips;
    [FoldoutGroup("Sound")] AudioSource _audio;

    //////////////////////////////////////
    [FoldoutGroup("Order")]
    public enum Order
    {
        Tap,
        Spawn,
        ColorChange


    }
    public Order Order_State;
    [FoldoutGroup("Order")] public int[] Tap_Count;
    [FoldoutGroup("Order")] public int[] Spawn_Count;
    [FoldoutGroup("Order")] public bool isOrder = false;
    [FoldoutGroup("Order")] public double Reward_Scope = 250;
    [FoldoutGroup("Order")] public float Order_Current_time;
    [FoldoutGroup("Order")] public float Order_Max_Time = 60f;
    [SerializeField] public int Order_Num;
    public int Order_Current_Count;
    public int Order_Max_Count;
    [SerializeField] public double Reward_Money;
    [SerializeField] float Order_x;
    [SerializeField] Image Check_Line_Img;
    GameObject Check_Img;
    Text[] Order_Text;

    // /////////////////////
    [FoldoutGroup("ETC")] public bool isRV_Hide = true;

    /// //////////////////////////////////////

    [Space(10)]
    [Title("Money Value")]
    public double Money;
    static readonly string[] CurrencyUnits = new string[] { "", "K", "M", "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };
    public double temp_money;

    public Camera _maincam;
    Touch _touch;
    bool isClick = false;

    float RV_x;
    int RV_Current_num;
    bool isRV_Visible = false;
    [SerializeField] float RV_Current_Interval = 0f;
    [SerializeField] float RV_On_Interval = 60f;
    [SerializeField] float RV_Current_Wait_Interval = 0f;
    [SerializeField] float RV_Off_Interval = 30f;
    public Color[] RV_Line_Color;
    [SerializeField] bool isRV_On = false;
    [SerializeField] float _PlayTime = 0f;
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
        _audio = GetComponent<AudioSource>();

        _maincam = Camera.main;
        Upgrade_Button_Text = new Text[3];
        RV_Text = new Text[4];
        MPS_Text = new Text[3];
        MPS_value = new double[3];
        MPS_Temp = new double[3];
        Stage_Income = new double[3];
        MPS_Button = new Button[3];
        Order_Text = new Text[4];
    }

    void Init()
    {
        Stage_Group = new GameObject[3];
        DOTween.Sequence(TapToSpawn_Img.transform.DOScale(0.2f, 0.25f)
            .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Yoyo));
        RV_x = RV_Button_Group[0].transform.localPosition.x;
        Order_x = Order_Panel.transform.localPosition.x;
    }

    private void Start()
    {
        IAPManager.RegisterProduct("popcorn_noads", () => PlayNoAds());

        SetButton();
        for (int i = 0; i <= Current_Max_Stage_Level; i++)
        {
            Current_Stage_Level = i;
            SetStage();
            DataManager.instance.Save_Data();
        }

        if (DataManager.instance._gameData.Max_Stage_Level == 0)
        {
            EventTracker.TryStage(Current_Stage_Level + 1);
            Debug.Log("Send TryStage : Stage_" + (Current_Stage_Level + 1));
        }


        StartCoroutine(Cor_MPS());
        StartCoroutine(Cor_Time());

        NoAds_Button.gameObject.SetActive(false);

        StartCoroutine(NoAdsButton_On());

    }

    void SetStage()
    {
        DataManager.instance.Load_Data();

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



        Current_Popcorn_Level = Current_StageManager.Popcorn_Level;
        Current_Income_Level = Current_StageManager.Income_Level;
        Current_Object_Level = Current_StageManager.Object_Level;
        Current_Obj_Max_Level = Current_StageManager.Obj_Max_Level;



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


        Current_Up_Income = Current_Income_Level * Current_Up_Income_Base_Price + Current_Up_Income_Base_Price * Mathf.Pow(Current_Up_Income_Scope, Current_Income_Level);

        _spawner = Current_StageManager._spawner;
        Check_Level_Price();
        Check_Data();

        // 12.1 update
        Door_OnOff(false);
        RV_Super_Fever_bool = false;

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

        // 12.07 update

        switch (Current_Stage_Level)
        {
            case 0:
                Upgrade_Button[0].transform.GetChild(1).GetComponent<Text>().text = "Add Popcorn";
                NextLevel_Button.gameObject.SetActive(true);
                Previous_Button.gameObject.SetActive(false);
                break;
            case 1:
                Upgrade_Button[0].transform.GetChild(1).GetComponent<Text>().text = "Add Jewelry";
                NextLevel_Button.gameObject.SetActive(true);
                Previous_Button.gameObject.SetActive(true);
                break;
            case 2:
                Upgrade_Button[0].transform.GetChild(1).GetComponent<Text>().text = "Add Popcorn";
                NextLevel_Button.gameObject.SetActive(false);
                Previous_Button.gameObject.SetActive(true);
                break;

            default:

                break;
        }
        //Fever_Img_Group.SetActive(false);

    }
    IEnumerator Cor_Time()
    {
        WaitForSeconds _deltatime = new WaitForSeconds(Time.deltaTime);

        if (isRV_Hide == true)
        {

            while (true)
            {
                yield return null; // _deltatime;
                //_PlayTime += Time.deltaTime;

                if (isRV_Visible == false)
                {
                    RV_Current_Interval += Time.deltaTime;
                    if (RV_Current_Interval >= RV_On_Interval)
                    {
                        RV_Current_Interval = 0;
                        RV_Button_OnOff();
                    }
                }
                else if (isRV_Visible == true && isRV_On == false)
                {
                    RV_Current_Wait_Interval += Time.deltaTime;
                    RV_Button_Group[RV_Current_num].transform.GetChild(1).GetComponent<Image>().fillAmount
                        = (RV_Off_Interval - RV_Current_Wait_Interval) / RV_Off_Interval;

                    RV_Button_Group[RV_Current_num].transform.GetChild(1).GetComponent<Image>().color
                        = RV_Button_Group[RV_Current_num].transform.GetChild(1).GetComponent<Image>().fillAmount > 0.4
                        ? RV_Button_Group[RV_Current_num].transform.GetChild(1).GetComponent<Image>().color = RV_Line_Color[0]
                    : RV_Button_Group[RV_Current_num].transform.GetChild(1).GetComponent<Image>().color = RV_Line_Color[1];


                    if (RV_Current_Wait_Interval >= RV_Off_Interval)
                    {
                        RV_Current_Wait_Interval = 0;
                        RV_Button_OnOff();
                    }
                }

                // update 12.09
                if (isOrder == false)
                {
                    Order_Current_time += Time.deltaTime;
                    if (Order_Current_time >= Order_Max_Time)
                    {
                        Order_Current_time = 0f;
                        isOrder = true;
                        Order_Func();
                    }
                }
                else
                {
                    if (Order_Current_Count >= Order_Max_Count)
                    {

                        if (Check_Img.activeSelf == false)
                        {
                            Check_Line_Img.DOFillAmount(1, 0.5f).SetEase(Ease.Linear)
                                .OnComplete(() =>
                                {
                                    Order_Text[1].gameObject.SetActive(false);
                                    Order_Text[2].gameObject.SetActive(false);
                                    Recive_Button.gameObject.SetActive(true);
                                }
                                );
                        }
                        Check_Img.SetActive(true);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                DOTween.Sequence(RV_Button_Group[i].GetComponent<RectTransform>().DOLocalMoveX(RV_x - 245f, 0.5f).SetEase(Ease.Linear));
                RV_Button_Group[i].transform.GetChild(1).GetComponent<Image>().fillAmount = 0f;
            }
        }

    }


    IEnumerator Cor_Update()
    {

        WaitForSeconds _deltatime = new WaitForSeconds(Time.deltaTime);

        while (true)
        {
            yield return null; // _deltatime;
            _PlayTime += Time.deltaTime;

            for (int i = 0; i < 3; i++)
            {
                temp_money = RV_Income_Double_bool ? Current_Up_Income * 2f : Current_Up_Income;
                double Temp_testMoney = RV_Income_Double_bool ? Stage_Income[i] * 2d : Stage_Income[i];
                MPS_value[i] = MPS_Temp[i] * Temp_testMoney;
            }


            Money_Text.text = ToCurrencyString(Money);

            RV_Text[0].text = RV_Spawn_Double_bool ? string.Format("00:{0:N0}", RV_Spawn_Double_time) : "Spawn X2";
            RV_Text[1].text = RV_Income_Double_bool ? string.Format("00:{0:N0}", RV_Income_Double_time) : "Income X2";
            RV_Text[2].text = RV_Super_Fever_bool ? string.Format("00:{0:N0}", RV_Super_Fever_time) : "Auto Click";
            RV_Text[3].text = RV_GoldPop_bool ? string.Format("00:{0:N0}", RV_GoldPop_time) : "Gold Pop";

            MPS_Text[0].text = string.Format("{0}/s", ToCurrencyString(MPS_value[0]));
            MPS_Text[1].text = string.Format("{0}/s", ToCurrencyString(MPS_value[1]));
            MPS_Text[2].text = string.Format("{0}/s", ToCurrencyString(MPS_value[2]));

            Order_Text[1].text = string.Format("({0}/{1})", Order_Current_Count, Order_Max_Count);

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


    void InputKeyFunc()
    {
#if UNITY_EDITOR
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
            Door_OnOff(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
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
        }

        else if (Input.GetKeyDown(KeyCode.Z))
        {
            RV_Button_OnOff();
        }

#endif
        /////////////////////////////////////////

#if !UNITY_EDITOR
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
    }

    void Door_OnOff(bool isbool)
    {

        StartCoroutine(Cor());

        IEnumerator Cor()
        {
            if (isbool == false)
            {
                if (Current_StageManager._spawner.isSuperFever == true)
                {

                    RV_Super_Fever_bool = false;
                    Current_Off_Object[2].SetActive(true);
                    Current_Off_Object[4].SetActive(true);
                    Fever_Img_Group.SetActive(false);
                    Fever_Effect.SetActive(false);
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
                    Fever_Img_Group.SetActive(false);
                    Fever_Effect.SetActive(false);
                }
            }
            else
            {
                Current_Off_Object[2].SetActive(false);
                Current_Off_Object[4].SetActive(false);
                Current_Off_Object[3].SetActive(true);
                Fever_Img_Group.SetActive(true);
                Fever_Effect.SetActive(true);
            }

            Full_Cam.SetActive(isbool);
        }


    }


    public static string ToCurrencyString(double number, int _num = 0)
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
            //Debug.LogWarningFormat("Failed - ToCurrentString({0}) : partSplit[1] = {1}", number, partsSplit[1]);
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
            switch (_num)
            {
                case 0:
                    showNumber = temp.ToString("F1").Replace(".0", "");
                    break;

                case 1:
                    if (remainder == 2)
                    {
                        showNumber = temp.ToString("F0").Replace(".0", "");
                    }
                    else
                    {
                        showNumber = temp.ToString("F1").Replace(".0", "");
                    }

                    break;

                case 2: //  소수 둘째자리까지만 출력한다.
                    showNumber = temp.ToString("F2").Replace(".0", "");
                    break;
            }


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
                //Upgrade_Button[0].interactable = true;
                Upgrade_Button[0].GetComponent<Image>().sprite = Upgrade_Img[0];
                Upgrade_Button[0].transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                //Upgrade_Button[0].interactable = false;
                Upgrade_Button[0].GetComponent<Image>().sprite = Upgrade_Img[1];
                Upgrade_Button[0].transform.GetChild(2).gameObject.SetActive(true);
            }

        }
        else
        {
            //Upgrade_Button[0].interactable = false;
            Upgrade_Button[0].GetComponent<Image>().sprite = Upgrade_Img[1];
            Upgrade_Button[0].transform.GetChild(2).gameObject.SetActive(false);
        }


        if (Current_Income_Level < Current_Income_Max_Level)
        {

            if (Money > Current_Income_Upgrade_Base_Price * MathF.Pow(Current_Income_Upgrade_Scope, Current_Income_Level))
            {
                //Upgrade_Button[1].interactable = true;
                Upgrade_Button[1].GetComponent<Image>().sprite = Upgrade_Img[0];
                Upgrade_Button[1].transform.GetChild(1).GetComponent<Image>().sprite = Income_Img[0];
                Upgrade_Button[1].transform.GetChild(3).gameObject.SetActive(false);


            }
            else
            {
                //Upgrade_Button[1].interactable = false;
                Upgrade_Button[1].GetComponent<Image>().sprite = Upgrade_Img[1];
                Upgrade_Button[1].transform.GetChild(1).GetComponent<Image>().sprite = Income_Img[1];
                Upgrade_Button[1].transform.GetChild(3).gameObject.SetActive(true);
            }
        }
        else
        {
            //Upgrade_Button[1].interactable = false;
            Upgrade_Button[1].GetComponent<Image>().sprite = Upgrade_Img[1];
            Upgrade_Button[1].transform.GetChild(1).GetComponent<Image>().sprite = Income_Img[1];
            Upgrade_Button[1].transform.GetChild(3).gameObject.SetActive(false);
        }

        if (Current_Object_Level < Current_Obj_Max_Level)
        {
            if (Money > Current_Object_Base_Price * MathF.Pow(Current_Object_Upgrade_Scope, Current_Object_Level))
            {
                //Upgrade_Button[2].interactable = true;
                Upgrade_Button[2].GetComponent<Image>().sprite = Upgrade_Img[0];
                //Upgrade_Button[2].transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                //Upgrade_Button[2].interactable = false;
                Upgrade_Button[2].GetComponent<Image>().sprite = Upgrade_Img[1];
                //Upgrade_Button[2].transform.GetChild(2).gameObject.SetActive(true);
            }
        }
        else
        {
            //Upgrade_Button[2].interactable = false;
            Upgrade_Button[2].GetComponent<Image>().sprite = Upgrade_Img[1];
            //Upgrade_Button[2].transform.GetChild(2).gameObject.SetActive(true);
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
            string.Format("{0}➜{1}", ToCurrencyString(Current_Up_Income, 1), ToCurrencyString((Current_Income_Level + 1) * Current_Up_Income_Base_Price + Current_Up_Income_Base_Price * Mathf.Pow(Current_Up_Income_Scope, Current_Income_Level + 1), 1))
            : string.Format("{0}", ToCurrencyString(Current_Up_Income, 1));
        //Income_Text.text = string.Format("{0}➜{1}", ToCurrencyString(Current_Up_Income), ToCurrencyString(Current_Up_Income * Current_Up_Income_Scope));

        NextLevel_Price_text.text =
            Current_Stage_Level == Current_Max_Stage_Level ? string.Format("{0}", ToCurrencyString(Current_NextLevel_Price)) : "";

    }



    public void Popcorn_Upgrade()
    {
        if (PlayerPrefs.GetInt("NoAds", 0) == 0)
        {
            MondayOFF.AdsManager.ShowInterstitial();
        }
        if (Current_Popcorn_Level < Current_Popcorn_Max_Level)
        {
            if (Money >= Current_Popcorn_Base_Price * MathF.Pow(Current_Popcorn_Upgrade_Scope, Current_Popcorn_Level))  //  Current_Popcorn_Upgrade_Price[Current_Popcorn_Level])
            {
                Money -= Current_Popcorn_Base_Price * MathF.Pow(Current_Popcorn_Upgrade_Scope, Current_Popcorn_Level);

                Current_Popcorn_Level++;
                Current_StageManager.Popcorn_Level = Current_Popcorn_Level;
                DataManager.instance.Save_Data();
                Check_Level_Price();
            }
            else
            {
                AdsManager.ShowRewarded(() =>
                {
                    Current_Popcorn_Level++;
                    Current_StageManager.Popcorn_Level = Current_Popcorn_Level;
                    DataManager.instance.Save_Data();
                    Check_Level_Price();
                    EventTracker.LogCustomEvent("RV", new Dictionary<string, string> { { "RV_Upg_AddPopcorn", "1" } });
                });
            }


        }

    }
    public void Income_Upgrade()
    {
        if (PlayerPrefs.GetInt("NoAds", 0) == 0)
        {
            MondayOFF.AdsManager.ShowInterstitial();
        }
        if (Current_Income_Level < Current_Income_Max_Level)
        {
            if (Money > Current_Income_Upgrade_Base_Price * MathF.Pow(Current_Income_Upgrade_Scope, Current_Income_Level))
            {
                Money -= Current_Income_Upgrade_Base_Price * MathF.Pow(Current_Income_Upgrade_Scope, Current_Income_Level);
                Current_Income_Level++;
                Current_StageManager.Income_Level = Current_Income_Level;
                Current_Up_Income = Current_Income_Level * Current_Up_Income_Base_Price + Current_Up_Income_Base_Price * Mathf.Pow(Current_Up_Income_Scope, Current_Income_Level);
                DataManager.instance.Save_Data();
                Check_Level_Price();
                Stage_Income[Current_Stage_Level] = Current_Up_Income;

            }
            else
            {
                AdsManager.ShowRewarded(() =>
                {
                    Current_Income_Level++;
                    Current_StageManager.Income_Level = Current_Income_Level;
                    Current_Up_Income = Current_Income_Level * Current_Up_Income_Base_Price + Current_Up_Income_Base_Price * Mathf.Pow(Current_Up_Income_Scope, Current_Income_Level);
                    DataManager.instance.Save_Data();
                    Check_Level_Price();
                    Stage_Income[Current_Stage_Level] = Current_Up_Income;
                    EventTracker.LogCustomEvent("RV", new Dictionary<string, string> { { "RV_Upg_Income", "2" } });
                });
            }
        }

    }
    public void Obj_Upgrade()
    {
        if (PlayerPrefs.GetInt("NoAds", 0) == 0)
        {
            MondayOFF.AdsManager.ShowInterstitial();
        }

        if (Current_Object_Level < Current_Obj_Max_Level)
        {
            if (Money > Current_Object_Base_Price * MathF.Pow(Current_Object_Upgrade_Scope, Current_Object_Level))
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

        }

    }

    void Auto_Tap()
    {

        _spawner.isFever = true;
        DOTween.Sequence().AppendCallback(() =>
            {
                Fever_Img_Group.SetActive(true);
                Fever_Effect.SetActive(true);
                _maincam.backgroundColor = Fever_Color;
                Auto_Button.gameObject.SetActive(false);
                Door_OnOff(true);
                DOTween.To(() => Fever_time, x => Fever_time = x, 0, 20f).SetEase(Ease.Linear);

            }).AppendInterval(20f)
        .OnComplete(() =>
        {
            Fever_Img_Group.SetActive(false);
            Fever_Effect.SetActive(false);
            _spawner.isFever = false;
            Door_OnOff(false);
            _maincam.backgroundColor = Current_StageManager.BackGround_Color;
        });

    }


    void SetButton()
    {
        Upgrade_Button[0].onClick.AddListener(() => Popcorn_Upgrade());
        Upgrade_Button[1].onClick.AddListener(() => Income_Upgrade());
        Upgrade_Button[2].onClick.AddListener(() => Obj_Upgrade());


        RV_Button_Group[0].onClick.AddListener(() => AdsManager.ShowRewarded(() => RV_Spawn_Dobule()));
        RV_Button_Group[1].onClick.AddListener(() => AdsManager.ShowRewarded(() => RV_Income_Double()));
        RV_Button_Group[2].onClick.AddListener(() => AdsManager.ShowRewarded(() => RV_Super_Fever()));
        RV_Button_Group[3].onClick.AddListener(() => AdsManager.ShowRewarded(() => RV_GoldPop()));


        Upgrade_Button_Text[0] = Upgrade_Button[0].transform.GetChild(0).GetComponent<Text>();
        Upgrade_Button_Text[1] = Upgrade_Button[1].transform.GetChild(0).GetComponent<Text>();
        Upgrade_Button_Text[2] = Upgrade_Button[2].transform.GetChild(0).GetComponent<Text>();

        Income_Text = Upgrade_Button[1].transform.GetChild(1).GetChild(0).GetComponent<Text>();

        RV_Text[0] = RV_Button_Group[0].transform.GetChild(0).GetComponent<Text>();
        RV_Text[1] = RV_Button_Group[1].transform.GetChild(0).GetComponent<Text>();
        RV_Text[2] = RV_Button_Group[2].transform.GetChild(0).GetComponent<Text>();
        RV_Text[3] = RV_Button_Group[3].transform.GetChild(0).GetComponent<Text>();

        NextLevel_Button.onClick.AddListener(() => NextLevel());
        NextLevel_Price_text = NextLevel_Button.transform.GetChild(0).GetComponent<Text>();
        Previous_Button.onClick.AddListener(() => PreviousLevel());

        Auto_Button.onClick.AddListener(() => Auto_Tap());
        Cam_Rotate_Button.onClick.AddListener(() => Cam_Rot());

        // 11.30 update
        Setting_Button.onClick.AddListener(() => Setting_OnOff());
        Setting_Panel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => Setting_OnOff());

        // 12.01 update
        MPS_Button[0] = MPS_Panel.transform.GetChild(0).GetComponent<Button>();
        MPS_Button[1] = MPS_Panel.transform.GetChild(1).GetComponent<Button>();
        MPS_Button[2] = MPS_Panel.transform.GetChild(2).GetComponent<Button>();

        MPS_Text[0] = MPS_Button[0].transform.GetChild(0).GetComponent<Text>();
        MPS_Text[1] = MPS_Button[1].transform.GetChild(0).GetComponent<Text>();
        MPS_Text[2] = MPS_Button[2].transform.GetChild(0).GetComponent<Text>();

        // 12.06 update
        Sound_Toggle = Setting_Panel.transform.GetChild(2).GetComponent<Toggle>();
        Vibe_Toggle = Setting_Panel.transform.GetChild(3).GetComponent<Toggle>();

        Sound_Toggle.onValueChanged.AddListener(delegate { Sound_OnOff(); });
        Vibe_Toggle.onValueChanged.AddListener(delegate { Vibe_OnOff(); });

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

        // 12.09 update
        Check_Img = Order_Panel.transform.GetChild(0).gameObject;
        Check_Line_Img = Order_Panel.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        Order_Text[0] = Order_Panel.transform.GetChild(1).GetComponent<Text>();
        Order_Text[1] = Order_Panel.transform.GetChild(2).GetComponent<Text>();
        Order_Text[2] = Order_Panel.transform.GetChild(3).GetComponent<Text>();
        Order_Text[3] = Order_Panel.transform.GetChild(4).GetChild(0).GetComponent<Text>();
        Recive_Button = Order_Panel.transform.GetChild(4).GetComponent<Button>();
        Recive_Button.onClick.AddListener(() =>
        {
            Order_Current_Count = 0;
            isOrder = false;
            Money += Reward_Money;
            DOTween.Sequence(Order_Panel.transform.DOLocalMoveX(Order_x, 0.5f).SetEase(Ease.Linear));
            Recive_Button.gameObject.SetActive(false);
            Order_Text[1].gameObject.SetActive(true);
            Order_Text[2].gameObject.SetActive(true);
            Check_Img.SetActive(false);
        });

        // 12.14 InApp purchase Update
        NoAds_Button.onClick.AddListener(() => NoAdsPanel_OnOff());
        NoAds_Panel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => NoAdsPanel_OnOff());
        NoAds_Panel.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => IAP_NoAdsPurchase());


        Setting_Panel.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => IAPManager.RestorePurchase());


    }

    public bool RV_Spawn_Double_bool;
    float RV_Spawn_Double_time;
    void RV_Spawn_Dobule() // double spawn count
    {
        RV_Spawn_Double_bool = true;
        RV_Button_Group[0].transform.GetChild(1).gameObject.SetActive(false);

        DOTween.Sequence().AppendCallback(() =>
        {
            isRV_On = true;
            RV_Button_Group[0].interactable = false;
            DOTween.To(() => 30f, x => RV_Spawn_Double_time = x, 0, 30f).SetEase(Ease.Linear);

            //Dictionary<string, string> _dic = new Dictionary<string, string>();
            //_dic.Add(string.Format("RV_Spawn")
            //    , string.Format("1"));
            //EventTracker.LogCustomEvent("RV_", _dic);
            EventTracker.LogCustomEvent("RV", new Dictionary<string, string> { { "RV_Spawn", "1" } });

        }).AppendInterval(30f)
        .OnComplete(() =>
        {
            isRV_On = false;
            RV_Spawn_Double_bool = false;
            //RV_Button_Group[0].gameObject.SetActive(true);
            RV_Button_Group[0].interactable = true;
            RV_Button_Group[0].transform.GetChild(1).gameObject.SetActive(true);
            //RV_Button_OnOff();
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

    bool RV_Income_Double_bool;
    float RV_Income_Double_time;
    void RV_Income_Double() // double income
    {
        RV_Income_Double_bool = true;
        RV_Button_Group[1].transform.GetChild(1).gameObject.SetActive(false);
        DOTween.Sequence().AppendCallback(() =>
        {
            isRV_On = true;
            RV_Button_Group[1].interactable = false;
            DOTween.To(() => 30f, x => RV_Income_Double_time = x, 0, 30f).SetEase(Ease.Linear);

            //Dictionary<string, string> _dic = new Dictionary<string, string>();
            //_dic.Add(string.Format("RV_Income")
            //    , string.Format("3"));
            //EventTracker.LogCustomEvent("RV_", _dic);
            EventTracker.LogCustomEvent("RV", new Dictionary<string, string> { { "RV_Income", "3" } });


        }).AppendInterval(30f)
        .OnComplete(() =>
        {
            isRV_On = false;
            RV_Income_Double_bool = false;
            RV_Button_Group[1].interactable = true;
            RV_Button_Group[1].transform.GetChild(1).gameObject.SetActive(true);
            //RV_Button_OnOff();
        });
    }

    bool RV_Super_Fever_bool;
    float RV_Super_Fever_time;
    void RV_Super_Fever() // super spawn object and open 4 door
    {
        RV_Super_Fever_bool = true;
        Current_StageManager._spawner.isSuperFever = true;
        RV_Button_Group[2].transform.GetChild(1).gameObject.SetActive(false);
        DOTween.Sequence().AppendCallback(() =>
        {
            isRV_On = true;
            //Fever_Img_Group.SetActive(true);
            _maincam.backgroundColor = Fever_Color;
            RV_Button_Group[2].interactable = false;
            Door_OnOff(true);
            DOTween.To(() => 30f, x => RV_Super_Fever_time = x, 0, 30f).SetEase(Ease.Linear);

            //Dictionary<string, string> _dic = new Dictionary<string, string>();
            //_dic.Add(string.Format("RV_SuperFever")
            //    , string.Format("2"));
            //EventTracker.LogCustomEvent("RV_", _dic);
            EventTracker.LogCustomEvent("RV", new Dictionary<string, string> { { "RV_SuperFever", "2" } });

        }).AppendInterval(30f)
        .OnComplete(() =>
        {
            isRV_On = false;
            //Fever_Img_Group.SetActive(false);
            _maincam.backgroundColor = Current_StageManager.BackGround_Color;
            Door_OnOff(false);
            RV_Super_Fever_bool = false;
            Current_StageManager._spawner.isSuperFever = false;

            RV_Button_Group[2].interactable = true;
            RV_Button_Group[2].transform.GetChild(1).gameObject.SetActive(true);
            //RV_Button_OnOff();
        });
    }

    bool RV_GoldPop_bool;
    float RV_GoldPop_time;
    void RV_GoldPop()
    {
        RV_GoldPop_bool = true;
        Current_StageManager._spawner.isGold = true;
        RV_Button_Group[3].transform.GetChild(1).gameObject.SetActive(false);
        DOTween.Sequence().AppendCallback(() =>
        {
            isRV_On = true;
            RV_Button_Group[3].interactable = false;

            DOTween.To(() => 30f, x => RV_GoldPop_time = x, 0, 30f).SetEase(Ease.Linear);
            EventTracker.LogCustomEvent("RV", new Dictionary<string, string> { { "RV_GoldPop", "4" } });

        }).AppendInterval(30f)
        .OnComplete(() =>
        {
            isRV_On = false;
            RV_GoldPop_bool = false;
            Current_StageManager._spawner.isGold = false;
            RV_Button_Group[3].interactable = true;
            RV_Button_Group[3].transform.GetChild(1).gameObject.SetActive(true);
            //RV_Button_OnOff();
        });
    }



    void NextLevel()
    {
        if (Current_Stage_Level == Current_Max_Stage_Level)
        {
            Money -= Current_NextLevel_Price;

            EventTracker.ClearStage(Current_Stage_Level + 1);


            //Dictionary<string, string> _dic = new Dictionary<string, string>();
            //_dic.Add(string.Format("Stage_{0}", Current_Stage_Level + 1)
            //    , string.Format("PlayTime:{0:0}s", _PlayTime));
            //EventTracker.LogCustomEvent("Stage_Level,PlayTime", _dic);
            EventTracker.LogCustomEvent("Stage_PlayTime", new Dictionary<string, string> { { (Current_Stage_Level + 1).ToString() + "_Stage", ((int)_PlayTime).ToString() + "s" } });

            _PlayTime = 0f;

            Current_Max_Stage_Level++;

            EventTracker.TryStage(Current_Stage_Level + 2);

            if (Current_Stage_Level == 0)
            {
                CustomReviewManager.instance.StoreReview();
            }

        }

        DataManager.instance.Save_Data();
        Current_Stage_Level++;
        Check_Level_Price();

        SetStage();

    }

    void PreviousLevel()
    {

        DataManager.instance.Save_Data();
        Current_Stage_Level--;
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

        Rot_Cam.SetActive(!Rot_Cam.activeSelf);
        if (Rot_Cam.activeSelf == false)
        {
            Mouse_Rot.rotation = Quaternion.Euler(Vector3.zero);

        }

    }


    void Setting_OnOff()
    {
        Setting_Panel.SetActive(!Setting_Panel.activeSelf);

    }

    public void Sound_OnOff()
    {
        isSound = Sound_Toggle.isOn;

    }

    public void Vibe_OnOff()
    {
        isVibe = Vibe_Toggle.isOn;

    }


    public void Vibe(int _num)
    {
        if (isVibe)
        {
            switch (_num)
            {
                case 0:
                    MMVibrationManager.Haptic(HapticTypes.LightImpact);
                    break;

                case 1:
                    MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                    break;
                case 2:

                    MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
                    break;

                default:

                    break;
            }
        }
    }

    public void Sound(int _num)
    {
        if (isSound)
        {
            _audio.clip = _clips[_num];
            _audio.Play();
        }
    }


    void RV_Button_OnOff()
    {
        isRV_Visible = !isRV_Visible;
        if (isRV_Visible)
        {
            RV_Current_num = UnityEngine.Random.Range(0, 3);
            DOTween.Sequence(RV_Button_Group[RV_Current_num].GetComponent<RectTransform>().DOLocalMoveX(RV_x - 245f, 0.5f).SetEase(Ease.Linear));
            RV_Button_Group[RV_Current_num].transform.GetChild(1).GetComponent<Image>().fillAmount = 1f;
        }
        else
        {
            RV_Current_Wait_Interval = 0;
            DOTween.Sequence(RV_Button_Group[RV_Current_num].GetComponent<RectTransform>().DOLocalMoveX(RV_x, 0.5f).SetEase(Ease.Linear));
        }

    }


    void Order_Func()
    {

        DOTween.Sequence(Order_Panel.transform.DOLocalMoveX(Order_x + 417f, 0.5f).SetEase(Ease.Linear));

        Order_State = (Order)(UnityEngine.Random.Range(0, Enum.GetValues(typeof(Order)).Length));

        Order_Num = UnityEngine.Random.Range(0, Tap_Count.Length);
        Order_Current_Count = 0;
        Check_Img.SetActive(false);
        Check_Line_Img.fillAmount = 0;
        Reward_Money = Current_Up_Income * Reward_Scope * (double)(Order_Num + 1);
        Order_Text[1].gameObject.SetActive(true);
        Order_Text[2].gameObject.SetActive(true);
        Order_Text[2].text = string.Format("Reward : {0}", ToCurrencyString(Reward_Money));
        Order_Text[3].text = string.Format("Recive ${0}", ToCurrencyString(Reward_Money));
        switch (Order_State)
        {
            case Order.Tap:
                Order_Max_Count = Tap_Count[Order_Num];
                Order_Text[0].text = string.Format("Tap {0} Times", Order_Max_Count);
                break;

            case Order.Spawn:
                Order_Max_Count = Spawn_Count[Order_Num];
                Order_Text[0].text = string.Format("Make {0} Corns", Order_Max_Count);
                break;

            case Order.ColorChange:
                Order_Max_Count = Spawn_Count[Order_Num];
                Order_Text[0].text = string.Format("Dyeing {0}", Order_Max_Count);
                break;


            default:

                break;
        }

    }

    public void Add_Order_count(Order _order)
    {
        if (_order == Order_State)
        {
            Order_Current_Count++;

        }
    }


    private void OnApplicationQuit()
    {
        //Dictionary<string, string> _dic = new Dictionary<string, string>();
        //_dic.Add(string.Format("Stage_{0}", Current_Stage_Level + 1)
        //    , string.Format("PlayTime:{0:0}s", _PlayTime));
        //EventTracker.LogCustomEvent("Stage_Level,PlayTime", _dic);
        EventTracker.LogCustomEvent("Stage_PlayTime", new Dictionary<string, string> { { (Current_Stage_Level + 1).ToString() + "_Stage", ((int)_PlayTime).ToString() + "s" } });

        _PlayTime = 0f;
    }






    public void PlayNoAds()
    {
        AdsManager.noads = true;
        PlayerPrefs.SetInt("NoAds", 1);

        AdsManager.HideBanner();
        AdsManager.HidePlayOn();


        NoAds_Button.gameObject.SetActive(false);
        NoAds_Panel.SetActive(false);
        Debug.Log("Hide Ads");
    }


    public void IAP_NoAdsPurchase()
    {
        IAPManager.PurchaseProduct("popcorn_noads");
    }


    void NoAdsPanel_OnOff()
    {
        NoAds_Panel.SetActive(!NoAds_Panel.activeSelf);
    }

    IEnumerator NoAdsButton_On()
    {
        yield return new WaitForSeconds(5f);
        if (PlayerPrefs.GetInt("NoAds", 0) == 1)
        {
            NoAds_Button.gameObject.SetActive(false);
        }
        else
        {
            NoAds_Button.gameObject.SetActive(true);
        }
    }


}
