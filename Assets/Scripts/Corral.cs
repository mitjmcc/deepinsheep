using UnityEngine;
using System.Collections;

public class Corral : MonoBehaviour {

    public int team;
    public Game game;

	void Start () {
        
	}
	
	void Update () {
	
	}

    /// <summary>
    /// Collects sheep
    /// If the corral belongs to the Sheperds, they score
    /// Vice versa with the wolves
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Sheep")
        {
            game.Score(team == 1);
            game.getDebugScore();
            Destroy(other.gameObject);
            game.CheckWin();
        }
    }
}
