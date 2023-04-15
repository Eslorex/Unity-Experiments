using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private int score;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameManager Instance
    {
        get { return instance; }
    }

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public void IncrementScore(int amount)
    {
        score += amount;
        Debug.Log("New score is: " + score);
    }

    // Other methods and variables go here
}
