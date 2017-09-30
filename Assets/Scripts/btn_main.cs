using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class btn_main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    public void Start_Game()
    {
        SceneManager.LoadScene("game");
    }
    public void Start_Basic()
    {
        SceneManager.LoadScene("basic");
    }
    public void Goto_set()
    {
        SceneManager.LoadScene("set");
    }
    public void Goto_Main()
    {
        SceneManager.LoadScene("main");
    }
}