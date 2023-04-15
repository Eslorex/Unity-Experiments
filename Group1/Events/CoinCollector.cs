using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public delegate void CoinCollected(int score);
    public static event CoinCollected OnCoinCollected;

    private int score = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            score++;
            Debug.Log("Score: " + score);
            OnCoinCollected?.Invoke(score);
            Destroy(other.gameObject);
        }
    }
}
