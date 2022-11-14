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

        _gameData.Income_Level = _gamemanager.Income_Level;

        _gameData.Object_Level = _gamemanager.Object_Level;
        _gameData.Money = _gamemanager.Money;



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

        _gamemanager.Income_Level = _gameData.Income_Level;

        _gamemanager.Object_Level = _gameData.Object_Level;
        _gamemanager.Money = _gameData.Money;


    }


    public GameData _gameData;

    [System.Serializable]
    public class GameData
    {

        public int Increase_Popcorn_Level; //  스폰갯수 증가      
        public int Income_Level; // 업그레이드 레벨     
        public int Object_Level;
        public double Money;
    }

    [Button]
    public void Init_Data()
    {
        _gamemanager.Increase_Popcorn_Level = 0;

        _gamemanager.Income_Level = 0;

        _gamemanager.Object_Level = 0;
        _gamemanager.Money = 0;

        Save_Data();
    }

}
