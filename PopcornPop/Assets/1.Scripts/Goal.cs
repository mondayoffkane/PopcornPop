using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class Goal : MonoBehaviour
{
    public Material Base_Mat;
    public Mesh Base_Mesh;

        
    public Transform _waiting_Pool;

    public float add_size = 0.2f;
    public float Interval = 0.2f;

    public int Max_Count = 50;
    public int Current_Count = 0;
    public bool isFull = false;

    [SerializeField] public float Col_Radius = 3f;
    [SerializeField] Goal Front_Goal, Back_Goal;


    [SerializeField] float Floating_Current_time = 0f;
    [SerializeField] float Floating_Max_Time = 0.3f;
    [SerializeField] GameManager _gamemanager;

    [SerializeField] Vector3 Floating_Start_Offset = new Vector3(0f, 4f, -2f), Floating_End_Offset = new Vector3(0f, 2f, 0.2f);


    private void OnEnable()
    {
        StartCoroutine(Cor_Timer());
    }


    private void Start()
    {
        transform.DOScale(Vector3.one * add_size, Interval)
                .SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Yoyo);

        _gamemanager = GameManager.instance;
    }

    IEnumerator Cor_Timer()
    {
        WaitForSeconds _time = new WaitForSeconds(Time.deltaTime);
        while (true)
        {
            yield return _time;

            Floating_Current_time += Time.deltaTime;
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Popcorn"))
        {


            Popcorn _corn = other.GetComponent<Popcorn>();
            _gamemanager.ManagerAddMoney();

            Current_Count++;

            if (_gamemanager.Floating_Waiting_Pool.childCount <= 0)
            {
                _gamemanager.Add_Floating_Pool(_gamemanager.Add_Pool_Size);
            }
            if (Floating_Current_time >= Floating_Max_Time)
            {
                Floating_Current_time = 0;

                Transform _floating = _gamemanager.Floating_Waiting_Pool.GetChild(0);
                _floating.SetParent(_gamemanager.Floating_Using_Pool);
               
                _floating.transform.position = transform.position + Floating_Start_Offset;
                _floating.localScale = Vector3.one * 0.01f;
                _floating.gameObject.SetActive(true);
               
                _floating.GetChild(1).GetComponent<Text>().text = GameManager.ToCurrencyString(_gamemanager.temp_money);


                DOTween.Sequence().Append(_floating.transform.DOMove(_floating.transform.position + Floating_End_Offset, 0.75f))

                    .OnComplete(() =>
                    {
                        _floating.SetParent(_gamemanager.Floating_Waiting_Pool);
                        _floating.gameObject.SetActive(false);
                    });
            }



            

            if (isFull == false)
            {
                other.gameObject.SetActive(false);
                other.transform.SetParent(_waiting_Pool);
             
                other.GetComponent<MeshRenderer>().material = Base_Mat;
                if (other.GetComponent<MeshFilter>().sharedMesh != Base_Mesh)
                {
                    other.GetComponent<MeshFilter>().sharedMesh = Base_Mesh;
                }             
            }
            else
            {
                other.transform.SetParent(transform);
            }

        }
    }
   
}
