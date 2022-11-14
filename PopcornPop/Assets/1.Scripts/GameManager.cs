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

    public Transform Waiting_Pool, Using_Pool;
    public float Save_Interval = 1f;
    [SerializeField] float current_save_time = 0f;

    [Space(10)]
    [Title("Upgrade Value")]

    public int Increase_Popcorn_Level; //  스폰갯수 증가    
    public double[] Popcorn_Upgrade_Price;

    [Space(10)]
    public int Income_Level; // 업그레이드 레벨   
    public double[] Up_Income; // 금액대 리스트
    public double[] Income_Upgrade_Price;

    [Space(10)]
    public int Max_Obj_Level = 5;
    public int Object_Level;

    public double[] Obj_Upgrade_Price;
    public GameObject[] Add_Object; // 맵 오브젝트
    public GameObject[] Off_Object;



    [Space(10)]
    [Title("UI")]
    public Text Money_Text;
    public Button[] Upgrade_Button;
    public Button Auto_Button;
    public Button Cam_Rotate_Button;

    public GameObject Rot_Cam;
    public GameObject Full_Cam;


    [Space(10)]
    [Title("Floating_Money")]
    public int Start_Pool_Size = 100;
    public int Add_Pool_Size = 50;
    public Transform Floating_Waiting_Pool;
    public Transform Floating_Using_Pool;
    public Spawner _spawner;
    public float Fever_time;
    public float Max_Fever_time = 300f;


    [Space(10)]
    [Title("CPI")]
    public Vector3 Explosion_pos;
    public float Explosion_power;
    public float Explosion_radius;
    // Private

    string p;
    //GameObject[] Goals;
    /// //////////////////////////////////////

    [Space(10)]
    [Title("Money Value")]
    public double Money;
    static readonly string[] CurrencyUnits = new string[] { "", "K", "M", "B", "T", "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj", "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at", "au", "av", "aw", "ax", "ay", "az", "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj", "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt", "bu", "bv", "bw", "bx", "by", "bz", "ca", "cb", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "ck", "cl", "cm", "cn", "co", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cw", "cx", };


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




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // Application.targetFrameRate = 60;
        }

        SetButton();



    }

    private void Start()
    {
        DataManager.instance.Load_Data();
        Check_Data();

        Check_Level_Price();

    }

    void Update()
    {
        Money_Text.text = ToCurrencyString(Money);
        Check_Button();


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Off_Object[1].SetActive(false);
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
            Off_Object[1].SetActive(true);
        }

        current_save_time += Time.deltaTime;
        if (current_save_time >= Save_Interval)
        {
            current_save_time = 0f;
            DataManager.instance.Save_Data();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Off_Object[2].SetActive(false);
            Off_Object[3].SetActive(true);
            Off_Object[4].SetActive(false);
            Full_Cam.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Off_Object[2].SetActive(true);
            Off_Object[3].SetActive(false);
            Off_Object[4].SetActive(true);
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

    // --------------------------------------------

    public void Add_Pool(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            GameObject _floating = Instantiate(Resources.Load("Prefab/Floating_Money") as GameObject);
            _floating.transform.SetParent(Floating_Waiting_Pool);
            _floating.transform.position = Floating_Waiting_Pool.position;
            _floating.SetActive(false);
        }
    }



    public void Theorem(int[] _money_list, ref int _money_index)
    {

        for (int i = 0; i < 5; i++)
        {
            if (_money_list[i] > 0)
            {
                _money_index = i;
            }
        }

        for (int i = 0; i <= _money_index; i++)
        {
            if (_money_list[i] >= 1000)
            {
                _money_list[i] -= 1000;
                _money_list[i + 1] += 1;
            }

            if (_money_list[i] < 0)
            {
                if (_money_index > i)
                {
                    _money_list[i + 1] -= 1;
                    _money_list[i] += 1000;
                }
            }
        }
    }



    public void ManagerAddMoney()
    {
        Money += Up_Income[Income_Level];
    }



    void Check_Button()
    {

        if (Increase_Popcorn_Level < Popcorn_Upgrade_Price.Length)
        {
            if (Money > Popcorn_Upgrade_Price[Increase_Popcorn_Level])
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


        if (Income_Level < Income_Upgrade_Price.Length)
        {

            if (Money > Income_Upgrade_Price[Income_Level])
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

        if (Object_Level < Max_Obj_Level)
        {
            if (Money > Obj_Upgrade_Price[Object_Level])
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
            Increase_Popcorn_Level >= Popcorn_Upgrade_Price.Length ? "Max" : ToCurrencyString(Popcorn_Upgrade_Price[Increase_Popcorn_Level]);

        Upgrade_Button[1].transform.GetChild(0).GetComponent<Text>().text =
            Income_Level >= Income_Upgrade_Price.Length ? "Max" : ToCurrencyString(Income_Upgrade_Price[Income_Level]);

        Upgrade_Button[2].transform.GetChild(0).GetComponent<Text>().text =
           Object_Level >= Max_Obj_Level ? "Max" : ToCurrencyString(Obj_Upgrade_Price[Object_Level]);
        DataManager.instance.Save_Data();
    }



    public void Popcorn_Upgrade()
    {
        Money -= Popcorn_Upgrade_Price[Increase_Popcorn_Level];

        Increase_Popcorn_Level++;
        Check_Level_Price();

    }
    public void Income_Upgrade()
    {
        Money -= Income_Upgrade_Price[Income_Level];
        Income_Level++;
        Check_Level_Price();
    }
    public void Obj_Upgrade()
    {
        Money -= Obj_Upgrade_Price[Object_Level];
        Object_Level++;

        if (Object_Level == 4)
        {
            Off_Object[0].SetActive(false);
        }
        Add_Object[Object_Level - 1].SetActive(true);
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

        for (int i = 1; i <= Object_Level; i++)
        {
            Add_Object[i - 1].SetActive(true);

            if (i == 4)
            {
                Off_Object[0].SetActive(false);
            }
        }

    }


    void Cam_Rot()
    {
        Rot_Cam.SetActive(!Rot_Cam.activeSelf);
    }



}
