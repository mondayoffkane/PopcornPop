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

    public int Max_Popcorn_Level = 10;
    public int Increase_Popcorn_Level; //  스폰갯수 증가
    public int Popcorn_Upgrade_index;
    public int[] Popcorn_Upgrade_Price;

    [Space(10)]
    public int Max_Income_Level = 10;
    public int Income_Level; // 업그레이드 레벨
    public int Income_Index; // 금액 리스트의 인덱스
    public int[] Up_Income; // 금액대 리스트
    public int Income_Upgrade_Index;
    public int[] Income_Upgrade_Price;

    [Space(10)]
    public int Max_Obj_Level = 5;
    public int Object_Level;
    public int Obj_Upgrade_Index;
    public int[] Obj_Upgrade_Price;
    public GameObject[] Add_Object; // 맵 오브젝트
    public GameObject[] Off_Object;



    [Space(10)]
    [Title("Money Value")]
    public int[] Money_list;
    public int Money_index;
    public char[] Price_Unit;


    [Space(10)]
    [Title("UI")]
    public Text Money_Text;
    public Button[] Upgrade_Button;
    public Button Auto_Button;


    [Space(10)]
    [Title("Floating_Money")]
    public int Start_Pool_Size = 100;
    public int Add_Pool_Size = 50;
    public Transform Floating_Waiting_Pool;
    public Transform Floating_Using_Pool;
    public Spawner _spawner;
    public float _fever_time;


    [Space(10)]
    [Title("CPI")]
    public Vector3 Explosion_pos;
    public float Explosion_power;
    public float Explosion_radius;
    // Private

    string p;
    //GameObject[] Goals;
    /// //////////////////////////////////////

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


        // add data load ()
        MymoneyToString(Money_list, ref Money_index);
        Check_Level_Price();




    }

    void Update()
    {
        Money_Text.text = p;
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
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Off_Object[2].SetActive(true);
            Off_Object[3].SetActive(false);
        }

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

    //


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
    public void MymoneyToString(int[] _money_list, ref int _money_index)
    {

        float a = _money_list[_money_index];
        if (_money_index > 0)
        {
            float b = _money_list[_money_index - 1];
            a += b / 1000;
        }

        if (_money_index == 0)
        {
            a += 0;
        }

        char unit = Price_Unit[_money_index];
        p = (float)(Math.Truncate(a * 100) / 100) + unit.ToString();



    }




    public void ManagerAddMoney(int _price, int _price_index)
    {
        Money_list[_price_index] += _price;


        Theorem(Money_list, ref Money_index);
        MymoneyToString(Money_list, ref Money_index);
    }

    public void PopcornAddMoney(ref int _price, ref int _price_index, int[] add_price, int _add_index)
    {
        if (_price_index == _add_index)
        {
            _price += add_price[_add_index];
        }
        else
        {
            _price = add_price[_add_index];
            _price_index = _add_index;
        }
    }




    public void SetPopcorn_Price(ref int _price, ref int _price_index)
    {
        _price = Up_Income[Income_Index];
        _price_index = Income_Index;

    }

    void Check_Button()
    {

        if (Increase_Popcorn_Level < Max_Popcorn_Level)
        {
            if (Money_index > Popcorn_Upgrade_index)
            {
                Upgrade_Button[0].interactable = true;
            }
            else if (Money_index == Popcorn_Upgrade_index)
            {
                Upgrade_Button[0].interactable = Money_list[Money_index] >= Popcorn_Upgrade_Price[Increase_Popcorn_Level] ? true : false;
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


        if (Income_Level < Max_Income_Level)
        {

            if (Money_index > Income_Upgrade_Index)
            {
                Upgrade_Button[1].interactable = true;
            }
            else if (Money_index == Income_Upgrade_Index)
            {
                Upgrade_Button[1].interactable = Money_list[Money_index] >= Income_Upgrade_Price[Income_Level] ? true : false;
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
            if (Money_index > Obj_Upgrade_Index)
            {
                Upgrade_Button[2].interactable = true;
            }
            else if (Money_index == Obj_Upgrade_Index)
            {
                Upgrade_Button[2].interactable = Money_list[Money_index] >= Obj_Upgrade_Price[Object_Level] ? true : false;

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


        Auto_Button.transform.GetChild(0).GetComponent<Image>().DOFillAmount(_fever_time / 300f, 0.1f).SetEase(Ease.Linear);

        Auto_Button.interactable = _fever_time >= 300f ? true : false;


    }

    void Check_Level_Price()
    {
        Theorem(Money_list, ref Money_index);
        MymoneyToString(Money_list, ref Money_index);



        switch (Increase_Popcorn_Level)
        {
            case int n when (n >= 0 && n < 1):
                Popcorn_Upgrade_index = 0;
                break;

            case int n when (n >= 1 && n < 4):
                Popcorn_Upgrade_index = 1;
                break;
            case int n when (n >= 4 && n < 9):
                Popcorn_Upgrade_index = 2;
                break;
            case int n when (n >= 9):
                Popcorn_Upgrade_index = 3;
                break;
        }


        switch (Income_Level)
        {
            case int n when (0 <= n && n < 6):
                Income_Index = 0;
                break;

            case int n when (6 <= n && n < 9):
                Income_Index = 1;
                break;
            case int n when (9 <= n):
                Income_Index = 2;
                break;
        }

        switch (Income_Level)
        {
            case int n when (0 <= n && n < 1):
                Income_Upgrade_Index = 0;
                break;
            case int n when (1 <= n && n < 4):
                Income_Upgrade_Index = 1;
                break;

            case int n when (4 <= n && n < 7):
                Income_Upgrade_Index = 2;
                break;
            case int n when (7 <= n):
                Income_Upgrade_Index = 3;
                break;
        }

        switch (Object_Level)
        {
            case int n when (0 <= n && n < 1):
                Obj_Upgrade_Index = 0;
                break;
            case int n when (1 <= n && n < 3):
                Obj_Upgrade_Index = 1;
                break;
            case int n when (3 <= n):
                Obj_Upgrade_Index = 2;
                break;

        }

        Upgrade_Button[0].transform.GetChild(0).GetComponent<Text>().text =
            Increase_Popcorn_Level >= Max_Popcorn_Level ? "Max" : Popcorn_Upgrade_Price[Increase_Popcorn_Level] + Price_Unit[Popcorn_Upgrade_index].ToString();
        Upgrade_Button[1].transform.GetChild(0).GetComponent<Text>().text =
            Income_Level >= Max_Income_Level ? "Max" : Income_Upgrade_Price[Income_Level] + Price_Unit[Income_Upgrade_Index].ToString();
        Upgrade_Button[2].transform.GetChild(0).GetComponent<Text>().text =
           Object_Level >= Max_Obj_Level ? "Max" : Obj_Upgrade_Price[Object_Level] + Price_Unit[Obj_Upgrade_Index].ToString();
        DataManager.instance.Save_Data();
    }



    public void Popcorn_Upgrade()
    {
        Money_list[Popcorn_Upgrade_index] -= Popcorn_Upgrade_Price[Increase_Popcorn_Level];
        Increase_Popcorn_Level++;
        Check_Level_Price();

    }
    public void Income_Upgrade()
    {
        Money_list[Income_Upgrade_Index] -= Income_Upgrade_Price[Income_Level];
        Income_Level++;
        Check_Level_Price();
    }
    public void Obj_Upgrade()
    {
        Money_list[Obj_Upgrade_Index] -= Obj_Upgrade_Price[Object_Level];
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
                DOTween.To(() => _fever_time, x => _fever_time = x, 0, 20f).SetEase(Ease.Linear);

            }).AppendInterval(20f)
        .OnComplete(() => _spawner.isFever = false);

    }


    void SetButton()
    {
        Upgrade_Button[0].onClick.AddListener(() => Popcorn_Upgrade());
        Upgrade_Button[1].onClick.AddListener(() => Income_Upgrade());
        Upgrade_Button[2].onClick.AddListener(() => Obj_Upgrade());

        Auto_Button.onClick.AddListener(() => Auto_Tap());
    }

    void Check_Data()
    {
        //for (int i = 1; i <= 5; i++)
        //{
        //    Add_Object[i - 1].SetActive(false);
        //}

        for (int i = 1; i <= Object_Level; i++)
        {
            Add_Object[i - 1].SetActive(true);

            if (i == 4)
            {
                Off_Object[0].SetActive(false);
            }
        }

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position + Explosion_pos, Explosion_radius);
    //}




}
