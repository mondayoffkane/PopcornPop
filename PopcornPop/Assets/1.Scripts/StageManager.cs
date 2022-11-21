using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class StageManager : MonoBehaviour
{
    public Spawner _spawner;
    //[FoldoutGroup("Upgrade_Value")]    public int Stage_Level;
    [FoldoutGroup("Upgrade_Value")] public int Popcorn_Level; //  스폰갯수 증가    
    [FoldoutGroup("Upgrade_Value")] public double[] Popcorn_Upgrade_Price;

    [Space(10)]
    [FoldoutGroup("Upgrade_Value")] public int Income_Level; // 업그레이드 레벨   
    [FoldoutGroup("Upgrade_Value")] public double[] Up_Income; // 금액대 리스트
    [FoldoutGroup("Upgrade_Value")] public double[] Income_Upgrade_Price;

    [Space(10)]
    [FoldoutGroup("Upgrade_Value")] public int Max_Obj_Level = 5;
    [FoldoutGroup("Upgrade_Value")] public int Object_Level;

    [FoldoutGroup("Upgrade_Value")] public double[] Obj_Upgrade_Price;
    public GameObject[] Add_Object; // 맵 오브젝트
    public GameObject[] Off_Object;


    public Transform Waiting_Pool, Using_Pool;

    public Color32 BackGround_Color;



    private void Awake()
    {
        Waiting_Pool = new GameObject("Waiting_Pool").transform;
        Waiting_Pool.SetParent(transform);

        Using_Pool = new GameObject("Using_Pool").transform;
        Using_Pool.SetParent(transform);

    }


    private void OnEnable()
    {
        
    }




}
