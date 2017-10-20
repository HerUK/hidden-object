using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainController : MonoBehaviour {
    public Transform Content;
    public string CurrentPackName = "Basic";

    // Use this for initialization
    void Start()
    {
        Init();
    }

    void Init()
    {
        DataController.Instance.LoadMetaData();
        MetaData metaData = DataController.Instance.metaData;
        foreach (Pack pack in metaData.PackList)
        {
            Debug.Log(pack.PackName);
            if(CurrentPackName == pack.PackName)
			{
                LoadPackItemList(pack);
            }
        }
    }

    public void LoadPackItemList(Pack pack)
    {
		int i = 0;
        foreach(string StageID in pack.StageList)
        {
            Debug.Log(StageID);
            GameObject prefab = Resources.Load("Prefabs/StageItem") as GameObject;
            GameObject obj = Instantiate(prefab);
            obj.name = StageID;
            obj.transform.SetParent(Content);
			obj.transform.localScale = new Vector3 (1f, 1f, 1f);

			obj.GetComponentInChildren<Text> ().text = StageID;
			obj.GetComponent<StageButton> ().stageID = StageID;
			// -43, 100

			int x = i % 2;
			int y = i / 2;

            RectTransform rect = obj.GetComponent<RectTransform>();
			//rect.sizeDelta = new Vector2 (120f, 120f);
			rect.anchoredPosition = new Vector2 (-43f + x * 143f, -150f - 150f * y);

			if( DataController.Instance.gameData.ClearList.Contains(StageID)){
				obj.GetComponent<Image> ().color = Color.black;
				obj.GetComponentInChildren<Text> ().color = Color.white;
			}

			i += 1;

        }
    }

    // Update is called once per frame
    public void Start_Game()
    {
        SceneManager.LoadScene("game");
    }
    public void Start_Basic()
    {
        SceneManager.LoadScene("basic");
    }
    public void Goto_set()
    {
        SceneManager.LoadScene("set");
    }
    public void Goto_Main()
    {
        SceneManager.LoadScene("main");
    }

}
