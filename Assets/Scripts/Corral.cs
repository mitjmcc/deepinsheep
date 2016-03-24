using UnityEngine;
using System.Collections;

public class Corral : MonoBehaviour
{
    public int team;
	public SheepExploder scoreEffect;
    public Game game;
    Stack sheep;   

    void Start()
    {
        sheep = new Stack(25);
    }

    /// <summary>
    /// Collects sheep
    /// If the corral belongs to the Sheperds, they score
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sheep")
        {
            game.Score(1, other.GetComponent<Sheep>().points);
			game.getDebugScore();
			Instantiate(scoreEffect.gameObject, other.transform.position, Quaternion.LookRotation(other.attachedRigidbody.velocity));
            other.transform.SetParent(transform);
            other.transform.localPosition = Vector3.zero;
            sheep.Push(other.gameObject);
   //         //Destroy(other.gameObject);
            ArrangeSheep();
            game.CheckWin();
        }
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlayerController>().player + 1 != this.team)
        {
            game.Score(1, 1);
            game.getDebugScore();
			Instantiate(scoreEffect.gameObject, other.transform.position, Quaternion.LookRotation(other.attachedRigidbody.velocity));
            //other.transform.position = new Vector3(-2.907191f, 10f, -85f);
            other.GetComponent<PlayerController>().setGroundedTimeout();
            other.attachedRigidbody.velocity = ((other.attachedRigidbody.position - this.transform.position).normalized + new Vector3(0, 0.1f, 0)) * 45;
            game.CheckWin();
        }
    }

    void ArrangeSheep()
    {
        for (int i = 0; i < sheep.Count; i++)
        {
            Debug.Log("Stack it");
            ((GameObject)sheep.ToArray()[i]).transform.localPosition
                = new Vector3(0, i + 1, 0);
            ((GameObject)sheep.ToArray()[i]).GetComponent<Sheep>().SetCorralled();
        }
    }
}
