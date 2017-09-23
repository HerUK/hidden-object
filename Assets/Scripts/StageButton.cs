using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour {

    public int ChapterNum;
    public int StageNum;
    public string ItemListNum;

	public void OnClickButton()
    {
        DataController.Instance.ChapterNum = ChapterNum;
        DataController.Instance.StageNum = StageNum;
        DataController.Instance.ItemListNum = ItemListNum;
        Application.LoadLevel("game");
    }
}
