using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    public GameObject players, corrals;
    public string menu;
    public Camera winCam;
    public Camera winCam2;
    public Text timer;
    public Text[] playerScoreText;
    public Text[] playerWinText;
    public Image[] scoreGUI;
    public Texture2D cursor;

    public float matchTime;
    public int scoreMax;
    public static int playerCount;

    Camera cam, cam2;
    Vector3 camTarget1, camTarget2, winSpot1, winSpot2;
    GameObject player1, player2;
    int score1, score2, winTeam;
    bool split, paused, gameover;
    string nextScene;
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
        //Set cursor to not show and to the custom cursor
        Cursor.visible = false;
        Cursor.SetCursor(cursor, new Vector2(0, 0), CursorMode.ForceSoftware);
        //Get players
        player1 = players.transform.GetChild(0).gameObject;
        player2 = players.transform.GetChild(1).gameObject;
        //Get the player Cameras
        cam = player1.GetComponentInChildren<Camera>();
        cam2 = player2.GetComponentInChildren<Camera>();
        //Set up the End Game Cameras
        camTarget1 = winCam.transform.position + new Vector3(-3, 0, 0);
        winSpot1 = corrals.transform.GetChild(0).transform.position + new Vector3(10, 1.07f, 0);
        camTarget2 = winCam2.transform.position + new Vector3(3, 0, 0);
        winSpot2 = corrals.transform.GetChild(1).transform.position + new Vector3(-10, 1.07f, 0);

        //Turn split screen off/on
        SetSplitSceen(split);
        PlayerControlToggle(true);
        //TODO: Some kind of Invoke/Coroutine for the game start 
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
            PlayerControlToggle(false);
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            PlayerControlToggle(true);
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// Set if the players can move or not
    /// </summary>
    /// <param name="control"></param> True for movement
    public void PlayerControlToggle(bool control)
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
    /// Set if sheep are active or not
    /// </summary>
    /// <param name="enable"></param> True for enable
    public void SheepToggle(bool enable)
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Sheep"))
        {
            g.GetComponent<Sheep>().enabled = enable;
        }
    }

    /// <summary>
    /// Decrement the match time
    /// </summary>
    private void UpdateTime()
    {
        //http://answers.unity3d.com/questions/905990/how-can-i-make-a-timer-with-the-new-ui-system.html
        matchTime = Mathf.Clamp(matchTime, 0, matchTime - Time.deltaTime);

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
            if (!gameover)
            {
                if (score1 >= scoreMax || score1 > score2)
                {
                    Debug.Log("Team2 1 wins! Game over!");
                    winTeam = 1;

                }
                else if (score2 >= scoreMax || score1 < score2)
                {
                    Debug.Log("Team 2 wins! Game over!");
                    winTeam = 2;
                }
            }
            GameOver(winTeam);
        }
    }

    private void LoadNextScene()
    {
        if (nextScene != "")
        {
            SceneManager.LoadScene(nextScene);
        } else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    /// <summary>
    /// End Game handling
    /// </summary>
    /// <param name="winTeam"></param> 1 for team 1, 2 for 2
    void GameOver(int winTeam)
    {
        if (!gameover)
        {
            player1.transform.position = winSpot1;
            player1.transform.LookAt(new Vector3(0, 0, 0));
            player2.transform.position = winSpot2;
            player2.transform.LookAt(new Vector3(0, 0, 0));
            DisableScoreGUI();
            PlayerControlToggle(false);
            SheepToggle(false);
        }

        if (winTeam == 1)
        {
            winCam.enabled = true;
            winCam.transform.position = Vector3.Lerp(winCam.transform.position, camTarget1, .01f);
            playerWinText[0].enabled = true;
            textBounce(playerWinText[0]);
        }
        else
        {
            winCam2.enabled = true;
            winCam2.transform.position = Vector3.Lerp(winCam2.transform.position, camTarget2, .01f);
            playerWinText[1].enabled = true;
            textBounce(playerWinText[1]);
        }
        Invoke("LoadNextScene", 30f);
        gameover = true;
    }

    void DisableScoreGUI()
    {
        scoreGUI[0].enabled = false;
        scoreGUI[1].enabled = false;
        scoreGUI[2].enabled = false;
        playerScoreText[0].enabled = false;
        playerScoreText[1].enabled = false;
        timer.enabled = false;
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
            scoreGUI[1].enabled = false;
            //playerScoreText[1].enabled = false;
        } else {
            cam.pixelRect = new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2);
            cam.fieldOfView = 50;
            cam2.pixelRect = new Rect(0, 0, Screen.width, Screen.height / 2);
            scoreGUI[1].enabled = true;
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
