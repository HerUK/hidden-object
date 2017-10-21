using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenObject : MonoBehaviour {

    public bool IsFound = false;
	/*
	public AudioClip SFXClick;
	*/

    public void OnClickObject()
    {
        if (!IsFound)
        {
            IsFound = true;
            Debug.Log("Find it!");

			/**

			Instantiate(EffectSmoke, hit.point, EffectSmoke.transform.rotation);

            **/

            GameObject circle = new GameObject();
            RectTransform rt = circle.AddComponent<RectTransform>();
            Image img = circle.AddComponent<Image>();
            circle.name = "Found";
            circle.transform.parent = transform.parent;
            circle.transform.SetSiblingIndex(1);
            circle.transform.localPosition = new Vector3(0f, 0f, 0f);
            circle.transform.localScale = new Vector3(1f, 1f, 1f);

            img.sprite = Resources.Load<Sprite>("Sprites/Find");

            Vector2 pos = rt.anchoredPosition;
            pos.x = GetComponent<RectTransform>().anchoredPosition.x;
            pos.y = GetComponent<RectTransform>().anchoredPosition.y;
            rt.anchoredPosition = pos;

            rt.sizeDelta = new Vector2(100f, 100f);

            GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

            GameController.Instance.FindHiddenObject();

            GameController.Instance.bottomList[name].transform.GetChild(1).gameObject.SetActive(true);
            GameController.Instance.bottomList[name].transform.GetChild(2).gameObject.SetActive(false);

            Destroy(gameObject, 1f);
			/*
			Destroy(cirlcle, 1f);
			*/
        }

    }

}
