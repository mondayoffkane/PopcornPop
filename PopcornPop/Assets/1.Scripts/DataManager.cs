using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

public class DataManager : MonoBehaviour
{

    #region Singleton
    public static DataManager instance = null;
    public GameManager _gamemanager;
    private void Awake() => Init();
    private void Init()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);

        Load_Data();

    }
    #endregion

    private void Start()
    {
        _gamemanager = GetComponent<GameManager>();
    }

    private bool isData;
    public bool IsData
    {
        get { return IsData; }
        set { IsData = value; }
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
            Save_Data();
    }

    [ContextMenu("Save")]
    public void Save_Data()
    {
        _gameData.Increase_Popcorn_Level = _gamemanager.Increase_Popcorn_Level;
        _gameData.Popcorn_Upgrade_index = _gamemanager.Popcorn_Upgrade_index;
        _gameData.Income_Level = _gamemanager.Income_Level;
        _gameData.Income_Index = _gamemanager.Income_Index;
        _gameData.Income_Upgrade_Index = _gamemanager.Income_Upgrade_Index;
        _gameData.Object_Level = _gamemanager.Object_Level;
        _gameData.Obj_Upgrade_Index = _gamemanager.Obj_Upgrade_Index;
        _gameData.Money_list = _gamemanager.Money_list;
        _gameData.Money_index = _gamemanager.Money_index;



        string jsonData = JsonUtility.ToJson(_gameData, true);
        string path = Path.Combine(Application.persistentDataPath, "Load.json");
        File.WriteAllText(path, jsonData);
    }

    public void Load_Data()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "Load.json")))
            Save_Data();

        string path = Path.Combine(Application.persistentDataPath, "Load.json");
        string jsondata = File.ReadAllText(path);
        _gameData = JsonUtility.FromJson<GameData>(jsondata);


        _gamemanager.Increase_Popcorn_Level = _gameData.Increase_Popcorn_Level;
        _gamemanager.Popcorn_Upgrade_index = _gameData.Popcorn_Upgrade_index;
        _gamemanager.Income_Level = _gameData.Income_Level;
        _gamemanager.Income_Index = _gameData.Income_Index;
        _gamemanager.Income_Upgrade_Index = _gameData.Income_Upgrade_Index;
        _gamemanager.Object_Level = _gameData.Object_Level;
        _gamemanager.Obj_Upgrade_Index = _gameData.Obj_Upgrade_Index;
        _gamemanager.Money_list = _gameData.Money_list;
        _gamemanager.Money_index = _gameData.Money_index;


    }


    public GameData _gameData;

    [System.Serializable]
    public class GameData
    {

        public int Increase_Popcorn_Level; //  스폰갯수 증가
        public int Popcorn_Upgrade_index;

        public int Income_Level; // 업그레이드 레벨
        public int Income_Index; // 금액 리스트의 인덱스
        public int Income_Upgrade_Index;
        public int Object_Level;
        public int Obj_Upgrade_Index;

        public int[] Money_list;
        public int Money_index;
    }

    [Button]
    public void Init_Data()
    {
        _gamemanager.Increase_Popcorn_Level = 0;
        _gamemanager.Popcorn_Upgrade_index = 0;
        _gamemanager.Income_Level = 0;
        _gamemanager.Income_Index = 0;
        _gamemanager.Income_Upgrade_Index = 0;
        _gamemanager.Object_Level = 0;
        _gamemanager.Obj_Upgrade_Index = 0;
        _gamemanager.Money_index = 0;
        for (int i = 0; i < _gamemanager.Money_list.Length; i++)
        {
            _gamemanager.Money_list[i] = 0;
        }

        Save_Data();
    }

}
