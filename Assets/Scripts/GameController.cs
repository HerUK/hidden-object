using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text TextHint;
    public Camera MainCamera;
    public GameObject EffectSmoke;
    public AudioClip SFXClick;
    public Transform[] objectList;
    private int hiddenObjectIndex;

    //The edges of the area which the objects are scattered within
    public Transform edgeTop;
    public Transform edgeBottom;
    public Transform edgeLeft;
    public Transform edgeRight;

    //The click area of a hidden object
    public float objectClickRadius = 1;

    //How many objects we need to find to win this level
    public Vector2 hiddenObjectsRange = new Vector2(1, 3);
    internal int numberOfHiddenObjects = 1;

    //How many objects we found so far this level
    internal int foundObjects = 0;

    //The bottom bar object which contains the text, as well as the "find object" text.
    public Transform bottomBar;

    //The score bar which contains the level score, and total score
    public Transform scoreBar;
    internal int roundScore = 0; //Our score for the current round
    private int totalScore = 0; //Our total score in the game

    //The options screen shows options including Restart, Main Menu, Music, and Sound. When the options screen appears, the game is paused.
    public Transform optionsScreen;

    public float hintTime = 5;
    private float hintTimeCount = 0;

    //A list of messages that randomly appear when winning a level
    public string[] victoryMessage;


    //Various sounds
    public AudioClip audioFind;
    public AudioClip audioError;
    public AudioClip audioWin;
    public AudioClip audioTimeup;
    public AudioClip audioHint;


    //Is the game paused now?
    public bool isPaused = true;

    //A genral use index
    private int index = 0;
    private Transform newObject;




    // Use this for initialization
    void Start () {


        if (optionsScreen) optionsScreen.gameObject.SetActive(false);
        if (scoreBar) scoreBar.gameObject.SetActive(false);


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

        //Now we start placing the hidden objects
        index = 0;

        while (index < numberOfHiddenObjects)
        {
            //Create a hidden object, scale it and give it a random rotation. Then put it in the edgeTop object for easier access later
            newObject = Instantiate(objectList[hiddenObjectIndex], new Vector3(Random.Range(edgeLeft.position.x, edgeRight.position.x), Random.Range(edgeBottom.position.y, edgeTop.position.y), -3), Quaternion.identity) as Transform;

            newObject.localScale *= objectGap;

            newObject.eulerAngles = new Vector3(newObject.eulerAngles.x, newObject.eulerAngles.y, Random.Range(0, 360));

            newObject.parent = edgeTop;

            newObject.SendMessage("DelayAnimation", Random.Range(0, 0.1f));

            //Also add a collider so we can click it, and make sure it shows in front of the regular objects.
            newObject.gameObject.AddComponent<CircleCollider2D>();

            newObject.GetComponent<CircleCollider2D>().radius = objectClickRadius;// * objectGap;

            newObject.SendMessage("Hidden", true);

            newObject.position = new Vector3(newObject.position.x, newObject.position.y, newObject.position.z - 1);

            //Create the icons of the hidden object in the top bar
            if (showIconsOnTop == true)
            {
                Transform newIcon = Instantiate(objectList[hiddenObjectIndex], new Vector3(Random.Range(edgeLeft.position.x, edgeRight.position.x), Random.Range(edgeBottom.position.y, edgeTop.position.y), 0), Quaternion.identity) as Transform;

                newIcon.parent = topBar;

                newIcon.position = topBar.Find("Base/GlassIcon").position;

                newIcon.position = new Vector3(newIcon.position.x + index * iconsGap, newIcon.position.y, newIcon.position.z + 0.5f);

                newIcon.SendMessage("SetIconRotation");

                newIcon.SendMessage("ObjectIntro");

                newIcon.name = "HiddenIcon" + index;
            }

            index++;
        }

        if (showIconsOnTop == false)
        {
            //Write the find message based on the word, the article(a,an), the number of hidden objects (more than 1), etc
            if (numberOfHiddenObjects > 1)
            {
                topBar.Find("Base/FindObjectText").GetComponent<TextMesh>().text = topBar.Find("Base/FindObjectText/Shadow").GetComponent<TextMesh>().text = "FIND " + numberOfHiddenObjects.ToString() + " " + newObject.GetComponent<HOGHiddenObject>().namePlural;
            }
            else
            {
                topBar.Find("Base/FindObjectText").GetComponent<TextMesh>().text = topBar.Find("Base/FindObjectText/Shadow").GetComponent<TextMesh>().text = "FIND " + newObject.GetComponent<HOGHiddenObject>().nameArticle + " " + newObject.GetComponent<HOGHiddenObject>().objectName;
            }
        }
        else
        {
            topBar.Find("Base/FindObjectText").GetComponent<TextMesh>().text = topBar.Find("Base/FindObjectText/Shadow").GetComponent<TextMesh>().text = "";
        }


    }
			else
			{
				Debug.LogWarning("You must assign the Top/Bottom/Left/Right edges in the inspector");
			}
		}

		/// <summary>
		/// This function add 1 to the number of found objects, and then checks if we found all the hidden objects to win the level. 
		/// The function is called from the hidden object itself when you click it. The hidden object has a HOGHiddenObject script attached to it which detects the click.
		/// </summary>
		IEnumerator UpdateFoundObjects()
{
    //Remove one of the hidden object icons, if they exist
    if (showIconsOnTop == true)
    {
        //if ( topBar.Find("HiddenIcon" + (numberOfHiddenObjects - foundObjects - 1)) )    Destroy(topBar.Find("HiddenIcon" + (numberOfHiddenObjects - foundObjects - 1)).gameObject);    

        if (topBar.Find("HiddenIcon" + (numberOfHiddenObjects - foundObjects - 1)))
        {
            //Play the object's find animation
            topBar.Find("HiddenIcon" + (numberOfHiddenObjects - foundObjects - 1)).SendMessage("ObjectIcon");

            //Wait a default time of 0.1 second
            yield return new WaitForSeconds(0.1f);

            //Remove the object icon
            Destroy(topBar.Find("HiddenIcon" + (numberOfHiddenObjects - foundObjects - 1)).gameObject);
        }
    }

    //Increase the number of found objects
    foundObjects++;

    //Reset hint timer
    hintTimeCount = 0;

    //If we find all the hidden objects we win the level
    if (foundObjects < numberOfHiddenObjects)
    {
        if (GetComponent<AudioSource>()) GetComponent<AudioSource>().PlayOneShot(audioFind);
    }
    else
    {
        if (GetComponent<AudioSource>()) GetComponent<AudioSource>().PlayOneShot(audioWin);

        Win();
    }
}

/// <summary>
/// This function wins a level. It pauses the game and applies the score based on the level we are at and the number of seconds left on the timer. Then it creates the next level.
/// </summary>
void Win()
{
    PauseTimer();

    //Activate the score bar which contains the level score and totalScore
    scoreBar.gameObject.SetActive(true);

    //Play the score bar intro animation
    scoreBar.GetComponent<Animation>().Play(screenIntroAnimation.name);

    //If we have a top bar assigned, show the victory message and add bonus time to the timer
    if (topBar)
    {
        //If we assigned victory messages, choose one of them randomly and display it in the top bar
        if (victoryMessage.Length > 0) topBar.Find("Base/FindObjectText").GetComponent<TextMesh>().text = topBar.Find("Base/FindObjectText/Shadow").GetComponent<TextMesh>().text = victoryMessage[Mathf.FloorToInt(Random.Range(0, victoryMessage.Length))];

        //Show the extra time we got on the timer
        topBar.Find("Base/TimerText").GetComponent<TextMesh>().text = topBar.Find("Base/TimerText").Find("Shadow").GetComponent<TextMesh>().text = timeLeft.ToString("00") + " +" + timeChange.ToString();
    }

    //If we have a score bar, show the level score and the total score
    if (scoreBar)
    {
        //Show the level score
        scoreBar.Find("Base/RoundScore").GetComponent<TextMesh>().text = scoreBar.Find("Base/RoundScore").Find("Shadow").GetComponent<TextMesh>().text = "+" + (timeBonus * Mathf.RoundToInt(timeLeft)).ToString();

        //Add the level score to the total score
        totalScore += timeBonus * Mathf.RoundToInt(timeLeft);

        //Show the total score
        scoreBar.Find("Base/TotalScore").GetComponent<TextMesh>().text = scoreBar.Find("Base/TotalScore").Find("Shadow").GetComponent<TextMesh>().text = totalScore.ToString();

        //Increas the time bonus for the next level
        timeBonus += timeBonusChange;
    }

    //Clear the objects from the area
    ClearObjects();

    Invoke("NextRound", nextRoundDelay);
}

void NextRound()
{
    //Add to the timer
    timeLeft += timeChange;

    UpdateTimer();

    //scoreBar.animation.Play("BarOutro");
    scoreBar.GetComponent<Animation>().Play(screenOutroAnimation.name);

    objectGap -= gapChange;

    if (objectGap < gapMinimum) objectGap = gapMinimum;

    CreateLevel();

    StartTimer();
}

/// <summary>
/// This function clears all objects from the level, making them fall down and then removing them
/// </summary>
void ClearObjects()
{
    //Go through all the objects in the edgeTop. If you check out the creation of the objects ( CreateLevel() ) you'll notice that we placed all the objects in 
    //edgeTop for easier access.
    foreach (Transform fallingObject in edgeTop)
    {
        //Add a rigid body to the object
        fallingObject.gameObject.AddComponent<Rigidbody2D>();

        //Throw it in a random direction and rotation
        fallingObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(objectFallSpeedX.x, objectFallSpeedX.y), Random.Range(objectFallSpeedY.x, objectFallSpeedY.y));

        fallingObject.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(objectRotateSpeed.x, objectRotateSpeed.y);

        //Destroy the object after a few seconds
        Destroy(fallingObject.gameObject, 3);
    }
}

/// <summary>
/// This function shows a hint, shaking a hidden object
/// </summary>
void ShowHint()
{
    if (audioHint)
    {
        if (GetComponent<AudioSource>())
        {
            GetComponent<AudioSource>().PlayOneShot(audioHint);
        }
        else
        {
            Debug.LogWarning("You must add an AudioSource component to this object in order to play sounds");
        }
    }

    //Find a hidden object and run the object hint frunction from it
    if (edgeTop.Find(objectList[hiddenObjectIndex].name + "(Clone)")) edgeTop.Find(objectList[hiddenObjectIndex].name + "(Clone)").SendMessage("ObjectHint");
}

/// <summary>
/// This function pauses the game and shows the options screen
/// </summary>
IEnumerator ToggleOptions()
{
    isPaused = !isPaused;

    if (optionsScreen)
    {
        //If the options screen is not centered, center it
        optionsScreen.position = new Vector3(0, 0, optionsScreen.position.z);

        if (isPaused == true)
        {
            optionsScreen.gameObject.SetActive(true);
            optionsScreen.GetComponent<Animation>().Play(screenIntroAnimation.name);
        }
        else
        {
            optionsScreen.GetComponent<Animation>().Play(screenOutroAnimation.name);

            yield return new WaitForSeconds(screenOutroAnimation.length);

            optionsScreen.gameObject.SetActive(false);
        }


    }
    else Debug.LogWarning("You must assign a game options screen in the component");
}
		


    }

   
}
