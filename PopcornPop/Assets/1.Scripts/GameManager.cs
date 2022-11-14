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
    [FoldoutGroup("Upgrade_Value")] public double[] Current_Popcorn_Upgrade_Price;

    [Space(10)]
    [FoldoutGroup("Upgrade_Value")] public int Current_Income_Level; // 업그레이드 레벨   
    [FoldoutGroup("Upgrade_Value")] public double[] Current_Up_Income; // 금액대 리스트
    [FoldoutGroup("Upgrade_Value")] public double[] Current_Income_Upgrade_Price;

    [Space(10)]
    [FoldoutGroup("Upgrade_Value")] public int Current_Max_Obj_Level = 5;
    [FoldoutGroup("Upgrade_Value")] public int Current_Object_Level;

    [FoldoutGroup("Upgrade_Value")] public double[] Current_Obj_Upgrade_Price;
    [FoldoutGroup("Upgrade_Value")] public GameObject[] Current_Add_Object; // 맵 오브젝트
    [FoldoutGroup("Upgrade_Value")] public GameObject[] Current_Off_Object;



    [Space(10)]    
    [FoldoutGroup("UI")] public Text Money_Text;
    [FoldoutGroup("UI")] public Button[] Upgrade_Button;
    [FoldoutGroup("UI")] public Button Auto_Button;
    [FoldoutGroup("UI")] public Button Cam_Rotate_Button;

    [FoldoutGroup("UI")] public GameObject Rot_Cam;
    [FoldoutGroup("UI")] public GameObject Full_Cam;


    [Space(10)]

    [FoldoutGroup("Floating_Money")] public int Start_Pool_Size = 100;
    [FoldoutGroup("Floating_Money")] public int Add_Pool_Size = 50;
    [FoldoutGroup("Floating_Money")] public Transform Floating_Waiting_Pool;
    [FoldoutGroup("Floating_Money")] public Transform Floating_Using_Pool;

    public Spawner _spawner;
    public float Fever_time;
    public float Max_Fever_time = 300f;


    [Space(10)]
    
    [FoldoutGroup("CPI")] public Vector3 Explosion_pos;
    [FoldoutGroup("CPI")] public float Explosion_power;
    [FoldoutGroup("CPI")] public float Explosion_radius;



    /// //////////////////////////////////////

    [Space(10)]
    [Title("Money Value")]
    public double Money;
    static readonly string[] CurrencyUnits = new string[] { "", "K", "M", "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };


    // ===============================================================================================

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // Application.targetFrameRate = 60;
        }

        Init();

    }

    void Init()
    {
        Stage_Group = new GameObject[3];


    }

    private void Start()
    {
        DataManager.instance.Load_Data();
        SetStage();


        SetButton();
        Check_Data();
        Check_Level_Price();
    }

    void SetStage()
    {

        Current_Stage_Level = Current_StageManager.Stage_Level;

        if (Stage_Group[Current_Stage_Level] == null)
        {
            Stage_Group[Current_Stage_Level] = Instantiate(Resources.Load("Prefab/Stage/Stage_" + Current_Stage_Level) as GameObject);
        }

        for (int i = 0; i < Stage_Group.Length; i++)
        {
            Stage_Group[i].SetActive(false);
        }
        Stage_Group[Current_Stage_Level].SetActive(true);
        Current_StageManager = Stage_Group[Current_Stage_Level].GetComponent<StageManager>();


        Current_Popcorn_Upgrade_Price = Current_StageManager.Popcorn_Upgrade_Price;
        Current_Income_Upgrade_Price = Current_StageManager.Income_Upgrade_Price;
        Current_Up_Income = Current_StageManager.Up_Income;
        Current_Add_Object = Current_StageManager.Add_Object;
        Current_Off_Object = Current_StageManager.Off_Object;
              


    }



    void Update()
    {
        Money_Text.text = ToCurrencyString(Money);
        Check_Button();

        InputKeyFunc();
    }

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
            Current_Off_Object[2].SetActive(false);
            Current_Off_Object[3].SetActive(true);
            Current_Off_Object[4].SetActive(false);
            Full_Cam.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Current_Off_Object[2].SetActive(true);
            Current_Off_Object[3].SetActive(false);
            Current_Off_Object[4].SetActive(true);
            Full_Cam.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Money += 100000000;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            DataManager.instance.Init_Data();
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        //}
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
            showNumber = temp.ToString("F").Replace(".00", "");
        }

        unityString = CurrencyUnits[quotient];

        return string.Format("{0}{1}{2}", significant, showNumber, unityString);
    }



    public void Add_Floating_Pool(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            GameObject _floating = Instantiate(Resources.Load("Prefab/Floating_Money") as GameObject);
            _floating.transform.SetParent(Floating_Waiting_Pool);
            _floating.transform.position = Floating_Waiting_Pool.position;
            _floating.SetActive(false);
        }
    }



    public void ManagerAddMoney()
    {
        Money += Current_Up_Income[Current_Income_Level];
    }



    void Check_Button()
    {

        if (Current_Popcorn_Level < Current_Popcorn_Upgrade_Price.Length)
        {
            if (Money > Current_Popcorn_Upgrade_Price[Current_Popcorn_Level])
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


        if (Current_Income_Level < Current_Income_Upgrade_Price.Length)
        {

            if (Money > Current_Income_Upgrade_Price[Current_Income_Level])
            {
                Upgrade_Button[1].interactable = true;
            }
            else
            {
                Upgrade_Button[1].interactable = false;
            }
        }
        else
        {
            Upgrade_Button[1].interactable = false;
        }

        if (Current_Object_Level < Current_Max_Obj_Level)
        {
            if (Money > Current_Obj_Upgrade_Price[Current_Object_Level])
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


        Auto_Button.transform.GetChild(0).GetComponent<Image>().DOFillAmount(Fever_time / 300f, 0.1f).SetEase(Ease.Linear);

        Auto_Button.interactable = Fever_time >= Max_Fever_time ? true : false;


    }

    void Check_Level_Price()
    {

        Upgrade_Button[0].transform.GetChild(0).GetComponent<Text>().text =
            Current_Popcorn_Level >= Current_Popcorn_Upgrade_Price.Length ? "Max" : ToCurrencyString(Current_Popcorn_Upgrade_Price[Current_Popcorn_Level]);

        Upgrade_Button[1].transform.GetChild(0).GetComponent<Text>().text =
            Current_Income_Level >= Current_Income_Upgrade_Price.Length ? "Max" : ToCurrencyString(Current_Income_Upgrade_Price[Current_Income_Level]);

        Upgrade_Button[2].transform.GetChild(0).GetComponent<Text>().text =
           Current_Object_Level >= Current_Max_Obj_Level ? "Max" : ToCurrencyString(Current_Obj_Upgrade_Price[Current_Object_Level]);
        DataManager.instance.Save_Data();
    }



    public void Popcorn_Upgrade()
    {
        Money -= Current_Popcorn_Upgrade_Price[Current_Popcorn_Level];

        Current_Popcorn_Level++;
        Check_Level_Price();

    }
    public void Income_Upgrade()
    {
        Money -= Current_Income_Upgrade_Price[Current_Income_Level];
        Current_Income_Level++;
        Check_Level_Price();
    }
    public void Obj_Upgrade()
    {
        Money -= Current_Obj_Upgrade_Price[Current_Object_Level];
        Current_Object_Level++;

        if (Current_Object_Level == 4)
        {
            Current_Off_Object[0].SetActive(false);
        }
        Current_Add_Object[Current_Object_Level - 1].SetActive(true);
        Check_Level_Price();
    }

    void Auto_Tap()
    {

        _spawner.isFever = true;

        DOTween.Sequence().AppendCallback(() =>
            {
                Auto_Button.interactable = false;
                DOTween.To(() => Fever_time, x => Fever_time = x, 0, 20f).SetEase(Ease.Linear);

            }).AppendInterval(20f)
        .OnComplete(() => _spawner.isFever = false);

    }


    void SetButton()
    {
        Upgrade_Button[0].onClick.AddListener(() => Popcorn_Upgrade());
        Upgrade_Button[1].onClick.AddListener(() => Income_Upgrade());
        Upgrade_Button[2].onClick.AddListener(() => Obj_Upgrade());

        Auto_Button.onClick.AddListener(() => Auto_Tap());
        Cam_Rotate_Button.onClick.AddListener(() => Cam_Rot());
    }


    void Check_Data()
    {

        for (int i = 1; i <= Current_Object_Level; i++)
        {
            Current_Add_Object[i - 1].SetActive(true);

            if (i == 4)
            {
                Current_Off_Object[0].SetActive(false);
            }
        }

    }


    void Cam_Rot()
    {
        Rot_Cam.SetActive(!Rot_Cam.activeSelf);
    }




}
