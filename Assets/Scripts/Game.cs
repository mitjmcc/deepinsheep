using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour {

    public GameObject spawns;
    public GameObject players;
    public Text[] playerScoreText;

    public int scoreMax = 9;
    public static int playerCount;
    int score1;
    int score2;

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
        SetPlayerNumbers();
        
	}
	
	void Update () {
	}

    /// <summary>
    /// If a team scores by placing a sheep in their corral
    /// increment there score
    /// </summary>
    /// <param name="team"></param> True for team 1, false for team2
    public void Score(bool team) {
        if (team)
        {
            score1++;
            playerScoreText[0].text = "Score: " + score1;
        }
        else
        {
            score2++;
            playerScoreText[1].text = "Score: " + score2;
        }
    }

    private void textBounce(Text t)
    {
        Vector2 old = t.rectTransform.sizeDelta;
        Vector2 n = new Vector2(t.rectTransform.rect.width + 5,
            t.rectTransform.rect.height + 5);
        t.rectTransform.sizeDelta = Vector2.Lerp(t.rectTransform.sizeDelta, n, .1f);
        t.rectTransform.sizeDelta = Vector2.Lerp(n, old, .1f);
    }

    /// <summary>
    /// Call to check win condition after scores have been updated
    /// </summary>
    public void CheckWin()
    {
        if (score1 == scoreMax)
        {
            Debug.Log("Team2 1 wins! Game over!");
        } else if (score2 == scoreMax)
        {
            Debug.Log("Team 2 wins! Game over!");
        }
    }

    public void SetPlayerNumbers()
    {
        playerCount = players.transform.childCount;
        for (int i = 0; i < playerCount; i++)
        {   
            players.transform.GetChild(i).GetComponent<PlayerController>().setPlayerNum(i);
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
