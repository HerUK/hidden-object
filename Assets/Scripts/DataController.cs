using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataController : MonoBehaviour {

    // Singleton class start
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }

    static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }

            return _instance;
        }
    }
    // Singleton class end

    public string StageNum;

    public int Hint = 5;

    public int HintPerHour = 1;

    public string ItemListNum;



    public string gameDataProjectFilePath = "/game.json";

    GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
            }
            return _gameData;
        }
    }

    StageData _stageData;
    public StageData stageData
    {
        get
        {
            if (_stageData == null)
            {
                LoadStageData();
            }
            return _stageData;
        }
    }
    

    public void LoadStageData()
    {
        TextAsset json = Resources.Load("MetaData/Stage"+StageNum) as TextAsset;
        Debug.Log(json.text);
        _stageData = JsonUtility.FromJson<StageData>(json.text);

        foreach (Item item in _stageData.ItemList)
        {
            Debug.Log(item.Name);
        }

    }


    ItemListData _itemListData;
    public ItemListData itemListData
    {
        get
        {
            if (_itemListData == null)
            {
                LoadItemListData();
            }
            return _itemListData;
        }
    }


    public void LoadItemListData()
    {
        TextAsset json = Resources.Load("MetaData/Item"+ItemListNum) as TextAsset;
        Debug.Log(json.text);
        _itemListData = JsonUtility.FromJson<ItemListData>(json.text);

        foreach (Item item in _itemListData.ItemList)
        {
            Debug.Log(item.Name);
        }

    }


    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + gameDataProjectFilePath;

        if (File.Exists(filePath))
        {
            Debug.Log("loaded!");
            string dataAsJson = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(dataAsJson);
        }
        else
        {
            Debug.Log("Create new");

            _gameData = new GameData();
            _gameData.TimePassed = 0;
            _gameData.Score = 1;
            _gameData.Hint = 0;

        }
    }

    public void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson(gameData);

        string filePath = Application.persistentDataPath + gameDataProjectFilePath;
        File.WriteAllText(filePath, dataAsJson);

    }
}