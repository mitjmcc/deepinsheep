using UnityEngine;

public class Timer : MonoBehaviour {
    private float targetTime = 60.0f;
    private float timeRemaining = 10.0f;
    private bool isStillCountingDown = false;
	// Use this for initialization
	void Start () {
        if (!isStillCountingDown)
        {
            isStillCountingDown = true;
            Invoke("tick", 1f);
        }
	}
	
     public void setTimeRemaining(float aFloat)
    {
        targetTime = aFloat;
    }
    
    public float getTimeRemaining ()
    {
        return timeRemaining;
    }
    public bool isTimeRemaining()
    {
        return isStillCountingDown;
    }

    private void tick()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0)
        {
            isStillCountingDown = false;
            CancelInvoke("tick");
        }
    }
    private void stopTimer()
    {

    }
	// Update is called once per frame
	void Update () {
        if(isStillCountingDown)
        {
            Debug.Log(timeRemaining);
            tick();
        }
	}
}
