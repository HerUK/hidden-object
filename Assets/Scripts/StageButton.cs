using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour {

    public string StageNum;

	public void OnClickButton()
    {
        DataController.Instance.StageNum = StageNum;
        Application.LoadLevel("game");
    }
}
