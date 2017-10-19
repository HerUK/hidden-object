using System;
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

    public string StageID = "1-1";

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

    MetaData _metaData;
    public MetaData metaData
    {
        get
        {
            if(_metaData == null)
            {
                LoadMetaData();
            }
            return _metaData;

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

    public void LoadMetaData()
    {
        TextAsset json = Resources.Load("MetaData/MetaData") as TextAsset;
        Debug.Log(json.text);
        _metaData = JsonUtility.FromJson<MetaData>(json.text);

        foreach (Pack item in _metaData.PackList)
        {
            Debug.Log(item.PackName);
        }
    }

   

    public void LoadStageData()
    {
		Debug.Log ("MetaData/Stage" + StageID);
        TextAsset json = Resources.Load("MetaData/Stage"+ StageID) as TextAsset;
        Debug.Log(json.text);
        _stageData = JsonUtility.FromJson<StageData>(json.text);

        foreach (Item item in _stageData.ItemList)
        {
            Debug.Log(item.Name);
        }

    }




    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + gameDataProjectFilePath;

        Debug.Log(filePath);
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
        CheckDailyHint();
    }

    public void CheckDailyHint()
    {
        if(gameData.LastHintAt == null)
        {
            gameData.Hint += 2;
            gameData.LastHintAt = DateTime.Now.ToString();
            SaveGameData();
        }else if(DateTime.Now.Day != DateTime.Parse(gameData.LastHintAt).Day)
        {
            gameData.Hint += 2;
            gameData.LastHintAt = DateTime.Now.ToString();
            SaveGameData();

        }
    }


    public void SaveGameData()
    {

        string dataAsJson = JsonUtility.ToJson(gameData);

        string filePath = Application.persistentDataPath + gameDataProjectFilePath;
        Debug.Log(filePath);
        File.WriteAllText(filePath, dataAsJson);

    }
}