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

    /**
     public void Init"PackList()
    {
        int i = 0;
        List<Item> list = DataController.Instance.MetaData.PackList;
        foreach (Item item in list)
        {
            Debug.Log(Pack.Name);
            GameObject prefab = Resources.Load("Prefabs/StageObject") as GameObject;
            GameObject obj = Instantiate(prefab, StageContent);
            obj.name = stageNum.Name;

            obj.GetComponentInChildren<Text>().text = stageNum.Name;
            var rect = obj.GetComponent<RectTransform>();

            Vector2 pos = rect.anchoredPosition;
            pos.x = -435f + 125f*i;
            pos.y = 70f;
            rect.anchoredPosition = pos;
            i++;

            obj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + item.SpriteName); ;
            rect = obj.transform.GetChild(0).GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(item.BottomWidth, item.BottomHeight);
    rect.Rotate(new Vector3(0, 0, item.BottomRotate));

            bottomList.Add(item.Name, obj);
     **/

    public void LoadStageData()
    {
        TextAsset json = Resources.Load("MetaData/Stage"+ StageID) as TextAsset;
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