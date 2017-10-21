using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class GameData {

    public int Score = 0;
    public int Hint = 0;
    public float TimePassed = 0f;
    public string LastHintAt;
	public string CurrentPackName = "Basic";
    public List<string> ClearList;

}
