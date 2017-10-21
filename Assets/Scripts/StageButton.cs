using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour {

    public string stageID;

    public string ItemListNum;

	public void OnClickButton()
    {
        DataController.Instance.ItemListNum = ItemListNum;
		DataController.Instance.StageID = stageID;
		SceneManager.LoadScene("game");

    }
}
