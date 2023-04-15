using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameManager.IncrementScore(10);
        }
    }

}
