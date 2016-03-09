using UnityEngine;

public class Corral : MonoBehaviour
{

    public int team;
    public Game game;

    /// <summary>
    /// Collects sheep
    /// If the corral belongs to the Sheperds, they score
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sheep")
        {
            game.Score(team == 1, other.GetComponent<Sheep>().points);
            game.getDebugScore();
            Destroy(other.gameObject);
            game.CheckWin();
        }
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().player + 1 != this.team)
        {
            game.Score(team == 1, 1);
            game.getDebugScore();
            //other.transform.position = new Vector3(-2.907191f, 10f, -85f);
            other.GetComponent<PlayerController>().setGroundedTimeout();
            other.attachedRigidbody.velocity = ((other.attachedRigidbody.position - this.transform.position).normalized + new Vector3(0, 0.1f, 0)) * 45;
            game.CheckWin();
        }
    }
}
