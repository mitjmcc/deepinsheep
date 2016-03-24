using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour {

    public GameObject spawns;
    public GameObject players;
    public Text[] playerScoreText;
    public Text timer;
    public Texture2D cursor;

    public float matchTime;
    public int scoreMax;
    public static int playerCount;

    Image bluePlank;
    Camera cam, cam2;
    int score1;
    int score2;
    bool split;
    bool paused;
    private static Game instance;

    private Game() { }

    public static Game inst {
        get {
            if (instance == null)
                instance = new Game();
            return inst;
        }
    }

	void Start() {
        Cursor.visible = false;
        Cursor.SetCursor(cursor, new Vector2(0, 0), CursorMode.ForceSoftware);
        bluePlank = GameObject.Find("blueScorePlank").GetComponent<Image>();
        cam = players.transform.GetChild(0).GetComponentInChildren<Camera>();
        cam2 = players.transform.GetChild(1).GetComponentInChildren<Camera>();
        SetPlayerNumbers();
        SetSplitSceen(split);
	}

	void FixedUpdate () {
        if (Input.GetKeyDown("escape"))
        {
            PauseGame(!paused);
            paused = !paused;
        }
        if (!paused) {
            UpdateTime();

            if (Input.GetKeyDown("/"))
            {
                SetSplitSceen(split);
                split = !split;
            }
            CheckWin();
        }
    }

    public void PauseGame(bool paused)
    {
        if (paused)
        {
            Time.timeScale = 0;
            SetPlayerControl(false);
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            SetPlayerControl(true);
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// Set if the players can move or not
    /// </summary>
    /// <param name="control"></param> True for movement
    public void SetPlayerControl(bool control)
    {
        if (control)
        {
            foreach (PlayerController player in players.GetComponentsInChildren<PlayerController>())
            {
                player.setPlayerControl(1);
            }
        } else
        {
            foreach (PlayerController player in players.GetComponentsInChildren<PlayerController>())
            {
                player.setPlayerControl(0);
            }
        }
    }

    /// <summary>
    /// Decrement the match time
    /// </summary>
    private void UpdateTime()
    {
        //http://answers.unity3d.com/questions/905990/how-can-i-make-a-timer-with-the-new-ui-system.html
        matchTime -= Time.deltaTime;

        //Divide the guiTime by sixty to get the minutes.
        var minutes = matchTime / 60;
        //Use the euclidean division for the seconds.
        var seconds = matchTime % 60;
        var fraction = (matchTime * 100) % 100;

        //update the label value
        timer.text = string.Format("{0:0}:{1:00}", minutes, seconds, fraction);
        if (matchTime - (int)matchTime <= 0.01f)
        {
            //timer.GetComponent<Animation>().wrapMode = WrapMode.Loop;
            timer.GetComponent<Animation>().Play();
        }
    }

    /// <summary>
    /// If a team scores by placing a sheep in their corral
    /// increment there score
    /// </summary>
    /// <param name="team"></param> True for team 1, false for team 2
    public void Score(int team, int amt) {
        if (team == 1)
        {
            score1 = Mathf.Clamp(score1 + amt, 0, score1 + amt);
            playerScoreText[0].text = "" + score1;
            textBounce(playerScoreText[0]);
        }
        else
        {
            score2 = Mathf.Clamp(score2 + amt, 0, score2 + amt);
            playerScoreText[1].text = "" + score2;
            textBounce(playerScoreText[1]);
        }
    }

    /// <summary>
    /// Activate text animation
    /// </summary>
    /// <param name="text"></param> a text object with an animation component
    private void textBounce(Text text)
    {
        text.transform.GetComponent<Animation>().Play();
    }

    /// <summary>
    /// Call to check win condition after scores have been updated
    /// </summary>
    public void CheckWin()
    {
        if (matchTime <= 0 || score1 == scoreMax || score2 == scoreMax)
        {
            if (score1 >= scoreMax)
            {
                Debug.Log("Team2 1 wins! Game over!");
            }
            else if (score2 >= scoreMax)
            {
                Debug.Log("Team 2 wins! Game over!");
            }
            PauseGame(true);
        }
    }

    /// <summary>
    /// Gives each local player a number
    /// </summary>
    public void SetPlayerNumbers()
    {
        playerCount = players.transform.childCount;
        for (int i = 0; i < playerCount; i++)
        {
            players.transform.GetChild(i).GetComponent<PlayerController>().setPlayerNum(i);
        }
    }

    /// <summary>
    /// Switches between splitscreen and single player
    /// </summary>
    /// <param name="split"></param> Splitscreen or not
    void SetSplitSceen(bool split)
    { 
        if (!split) {
            cam.pixelRect = new Rect(0, 0, Screen.width, Screen.height);
            cam.fieldOfView = 70;
            cam2.pixelRect = new Rect(0, 0, 0, 0);
            bluePlank.enabled = false;
            //playerScoreText[1].enabled = false;
        } else {
            cam.pixelRect = new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2);
            cam.fieldOfView = 50;
            cam2.pixelRect = new Rect(0, 0, Screen.width, Screen.height / 2);
            bluePlank.enabled = true;
            //playerScoreText[1].enabled = true;
        }
    }

    /// <summary>
    /// Get the current scores printed to the console
    /// </summary>
    public void getDebugScore()
    {
        Debug.Log("The score is Sheperds 1: " + score1 + " Shpepherds 2: " + score2);
    }
}
