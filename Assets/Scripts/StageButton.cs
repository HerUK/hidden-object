using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour {

    public string StageNum;
    public string ItemListNum;

	public void OnClickButton()
    {
        DataController.Instance.StageNum = StageNum;
        DataController.Instance.ItemListNum = ItemListNum;
        Application.LoadLevel("game");
    }
}
