using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour {

    public string stageID;

    public string ItemListNum;

	public void OnClickButton()
    {
        DataController.Instance.ItemListNum = ItemListNum;
		DataController.Instance.StageID = stageID;
		Application.LoadLevel("game");

    }
}
