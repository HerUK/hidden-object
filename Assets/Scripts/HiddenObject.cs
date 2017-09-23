using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenObject : MonoBehaviour {

    bool IsFound = false;

    public void OnClickObject()
    {
        if (!IsFound)
        {
            IsFound = true;
            Debug.Log("Find it!");
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

            rt.sizeDelta = new Vector2(150f, 150f);

            GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

            Destroy(gameObject, 1f);
            Destroy(circle, 1f);

        }

    }

}
