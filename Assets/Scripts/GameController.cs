using System.Collections;
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


    // Use this for initialization
    void Start () {

        //StartCoroutine(StartConllectHint());
        DataController.Instance.LoadStageData();
        DataController.Instance.LoadGameData();
        InitStageData();
        InitItemListData();

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


            rect.sizeDelta = new Vector2(item.Width, item.Height);


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
            var rect = obj.GetComponent<RectTransform>();

            Vector2 pos = rect.anchoredPosition;
            pos.x = -435f + 125f*i;
            pos.y = 70f;
            rect.anchoredPosition = pos;
            i++;
            obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + item.SpriteName);


            rect.sizeDelta = new Vector2(item.Width, item.Height);


        }
    }

    public void PurchaseComplete(Product p)
    {
        Debug.Log(p.metadata.localizedTitle + " purchase success!");
    }


}
