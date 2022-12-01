using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;


public class StageManager : MonoBehaviour
{
    public Spawner _spawner;
    //[FoldoutGroup("Upgrade_Value")]    public int Stage_Level;
    [FoldoutGroup("Upgrade_Value")] public int Popcorn_Level = 10; //  스폰갯수 증가
    [FoldoutGroup("Upgrade_Value")] public int Popcorn_Max_Level;
    [FoldoutGroup("Upgrade_Value")] public double Popcorn_Upgrade_Base_Price = 100;
    [FoldoutGroup("Upgrade_Value")] public float Popcorn_Upgrade_Scope = 5f;
    //[FoldoutGroup("Upgrade_Value")] public double[] Popcorn_Upgrade_Price;

    [Space(10)]
    [FoldoutGroup("Upgrade_Value")] public int Income_Level; // 업그레이드 레벨
    [FoldoutGroup("Upgrade_Value")] public int Income_Max_Level = 100;
    [FoldoutGroup("Upgrade_Value")] public double Up_Income;
    [FoldoutGroup("Upgrade_Value")] public double Up_Income_Base_Price = 1;
    [FoldoutGroup("Upgrade_Value")] public float Up_Income_Scope = 1.1f;
    [FoldoutGroup("Upgrade_Value")] public double Income_Upgrade_Base_Price = 100;
    [FoldoutGroup("Upgrade_Value")] public float Income_Upgrade_Scope = 1.2f;
    //[FoldoutGroup("Upgrade_Value")] public double[] Up_Income; // 금액대 리스트
    //[FoldoutGroup("Upgrade_Value")] public double[] Income_Upgrade_Price;

    [Space(10)]
    [FoldoutGroup("Upgrade_Value")] public int Obj_Max_Level = 5;
    [FoldoutGroup("Upgrade_Value")] public int Object_Level;
    [FoldoutGroup("Upgrade_Value")] public double Object_Upgrade_Base_Price = 500;
    [FoldoutGroup("Upgrade_Value")] public float Object_Upgrade_Scope = 10f;
    [FoldoutGroup("Upgrade_Value")] public int Off_Num = 0;
    //[FoldoutGroup("Upgrade_Value")] public double[] Obj_Upgrade_Price;
    public GameObject[] Add_Object; // 맵 오브젝트
    public GameObject[] Off_Object;
    public double NextLevel_Price;

    public Transform Waiting_Pool, Using_Pool;

    public Color32 BackGround_Color;

    public bool isRot = false;
    public Vector3 Rot_Speed = new Vector3(0f, 20f, 0f);



    private void Awake()
    {
        Waiting_Pool = new GameObject("Waiting_Pool").transform;
        Waiting_Pool.SetParent(transform);

        Using_Pool = new GameObject("Using_Pool").transform;
        Using_Pool.SetParent(transform);

    }


    //Coroutine _cor;

    //private void OnEnable()
    //{
    //    if (_cor != null)
    //    {
    //        StopCoroutine(_cor);

    //        _cor = null;

    //    }
    //    transform.rotation = Quaternion.Euler(Vector3.zero);
    //    _cor = StartCoroutine(Cor_Rotate());

    //}

    //IEnumerator Cor_Rotate()
    //{

    //    while (true)
    //    {
    //        yield return null;

    //        if (isRot)
    //        {
    //            transform.Rotate(Rot_Speed * Time.deltaTime);
    //        }

    //    }
    //}

    //public void Rotate_Button()
    //{
    //    isRot = !isRot;

    //    if (isRot == false)
    //    {
    //        transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.Linear);
    //    }
    //}


}
