using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;  

    private int currentScore;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {

        currentScore = 0;
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}
