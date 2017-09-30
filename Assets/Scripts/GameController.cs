﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text TextHint;
    public Camera MainCamera;
    public GameObject EffectSmoke;
    public AudioClip SFXClick;

    public Transform content;
    public Transform bottomContent;

    public Image BgImg;
    public RectTransform rectContent, rectBgImg;

    public int ObjectCount = 0;

    public static GameController Instance;
    public float gameStart;

    public Dictionary<string, GameObject> hiddenList;
    public Dictionary<string, GameObject> bottomList;

    /**

    public Dictionary<string, GameObject> StageList;

    **/

    bool ObjctCheck = false;

#if UNITY_IOS
    private string gameId = "1553201";
#elif UNITY_ANDROID
    private string gameId = "1553200";
#endif



    // Use this for initialization
    void Start () {
        Instance = this;

        hiddenList = new Dictionary<string, GameObject>();
        bottomList = new Dictionary<string, GameObject>();

        //StartCoroutine(StartConllectHint());
        DataController.Instance.LoadStageData();
        DataController.Instance.LoadGameData();
        InitStageData();
        InitItemListData();
        gameStart = Time.time;


        /**
        User user = new User();
        user.FacebookID = "111";
        user.FacebookName = "Hoyean";
        user.FacebookPhotoURL = "http:/asdf";

        string body = JsonUtility.ToJson(user);

        HTTPClient.Instance.POST(
            "http://heruk.azurewebsites.net/Login/Facebook",
            body,
            delegate (WWW www) {

                Debug.Log(www.text);

                LoginResult result = JsonUtility.FromJson<LoginResult>(www.text);

                Debug.Log(result.Message);

            });
        **/

        /*
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId);
        }
        */

    }


    // Update is called once per frame
    void Update()
    {

        TextHint.text = DataController.Instance.gameData.Hint.ToString();
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Input.mousePosition);
           
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                //Debug.Log(hit.point);
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                Instantiate(EffectSmoke, hit.point, EffectSmoke.transform.rotation);
                MainCamera.gameObject.GetComponent<AudioSource>().PlayOneShot(SFXClick);
            }

        }

    }

    public void InitStageData()
    {
        
        BgImg.sprite = Resources.Load<Sprite>("Sprites/" + DataController.Instance.stageData.BgImg);

        Vector2 size = rectBgImg.sizeDelta;
        size.x = DataController.Instance.stageData.Width;
        size.y = DataController.Instance.stageData.Height;
        rectBgImg.sizeDelta = size;

        size = rectContent.sizeDelta;
        size.x = DataController.Instance.stageData.Width;
        size.y = DataController.Instance.stageData.Height;
        rectContent.sizeDelta = size;

        List<Item> list = DataController.Instance.stageData.ItemList;
        foreach(Item item in list)
        {
            Debug.Log(item.Name);
            GameObject prefab = Resources.Load("Prefabs/HiddenObject") as GameObject;
            GameObject obj = Instantiate(prefab, content);
            obj.name = item.Name;
            var rect = obj.GetComponent<RectTransform>();




            Vector2 pos = rect.anchoredPosition;
            pos.x = item.PosX;
            pos.y = item.PosY;
            rect.anchoredPosition = pos;

            obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + item.SpriteName);
            obj.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

            rect.sizeDelta = new Vector2(item.Width, item.Height);

            hiddenList.Add(item.Name, obj);

        }
        ObjectCount = list.Count;
    }

    public void FindHiddenObject()
    {
        ObjectCount--;
        if(ObjectCount == 0)
        {
            string stage_name = DataController.Instance.ChapterNum + "-" + DataController.Instance.StageNum;
            if (!DataController.Instance.gameData.ClearList.Contains(stage_name))
            {
                DataController.Instance.gameData.ClearList.Add(stage_name);
            }

            DataController.Instance.SaveGameData();

            /**

            object Get = DataController.instance.gameData.ClearList;
            if(string intance.stage_name == object);
            {
                GameController.Instance.StageList.transform.GetChild(0).gameObject.SetActive(true);
                GameController.Instance.StageList.transform.GetChild(1).gameObject.SetActive(false);
                GameController.Instance.StageList.transform.GetChild(2).gameObject.SetActive(false);
            }

            **/

            float timePassed = Time.time - gameStart;
            int minutes = Mathf.FloorToInt(timePassed / 60);
            int seconds = Mathf.FloorToInt(timePassed % 60);
            string msg = string.Format("당신 기록 : {0}분 {1}초", minutes, seconds);
            DialogDataAlert alert = new DialogDataAlert("전체 찾았습니다!", msg, delegate () {
                DataController.Instance.StageNum++;
                Application.LoadLevel("game");
            });
            DialogManager.Instance.Push(alert);
        }
    }


    public void InitItemListData()
    {
        int i = 0;
        List<Item> list = DataController.Instance.stageData.ItemList;
        foreach (Item item in list)
        {
            Debug.Log(item.Name);
            GameObject prefab = Resources.Load("Prefabs/BottomObject") as GameObject;
            GameObject obj = Instantiate(prefab, bottomContent);
            obj.name = item.Name;
            /*obj.text = item.Name;*/
            obj.GetComponentInChildren<Text>().text = item.Name;
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
            /**  
             if (!ObjctCheck)
             {
                 ObjctCheck = true;
                 Debug.Log("Check!");
                 Destroy(gameObject.Text, 1f);
                 GameObject prefab = Resources.Load("Prefabs/BottomObject.img") as GameObject;
                 GameObject obj = Instantiate(prefab, bottomContent);
                 obj.name = check;
                 obj.GetComponentInChildren<Image>().img = check;
                 img.sprite = Resources.Load<Sprite>("Sprites/Find");

                 GameController.Instance.FindHiddenObject();

             }
             **/


        }
    }

        /**
        void ShowRewardedVideo()
        {
            var options = new ShowOptions();
            options.resultCallback = HandleShowResult;

            Advertisement.Show("rewardedVideo", options);
        }

        void HandleShowResult(ShowResult result)
        {
            if (result == ShowResult.Finished)
            {
                Debug.Log("Video completed - Offer a reward to the player");
                DataController.Instance.gameData.Hint += 1;
                DataController.Instance.SaveGameData();

            }
            else if (result == ShowResult.Skipped)
            {
                Debug.LogWarning("Video was skipped - Do NOT reward the player");

            }
            else if (result == ShowResult.Failed)
            {
                Debug.LogError("Video failed to show");
            }
        }
        **/


    public void PurchaseComplete(Product p)
    {
        Debug.Log(p.metadata.localizedTitle + " purchase success!");
    }


}
