using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text TextHint;
    public Camera MainCamera;
    public GameObject EffectSmoke;
    public AudioClip SFXClick;


    // Use this for initialization
    void Start () {

        TextHint.text = DataController.Instance.Hint.ToString();
        StartCoroutine(StartConllectHint());
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

           
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log(hit.point);
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                Instantiate(EffectSmoke, hit.point, EffectSmoke.transform.rotation);
                MainCamera.gameObject.GetComponent<AudioSource>().PlayOneShot(SFXClick);
            }

        }

    }

   
}
