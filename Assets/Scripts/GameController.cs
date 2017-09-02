using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text TextHint;
    public Camera MainCamera;
    public GameObject EffectSmoke;
    public AudioClip SFXClick;

    public Transform content;


    // Use this for initialization
    void Start () {

        TextHint.text = DataController.Instance.Hint.ToString();
        StartCoroutine(StartConllectHint());
        DataController.Instance.LoadStageData();

        InitStageData();
	}
	
    IEnumerator StartConllectHint()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            DataController.Instance.Hint += DataController.Instance.HintPerHour;
            TextHint.text = DataController.Instance.Hint.ToString();

 
        }

    }

    // Update is called once per frame
    void Update()
    {

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

            obj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/1/" + item.SpriteName);


            rect.sizeDelta = new Vector2(item.Width, item.Height);


        }
    }



}
