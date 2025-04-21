using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;

    public int currentScore;

    //default, will go up based on streak
    private int scorePerNote = 10;
    private int notesHit;

    [SerializeField]
    private int[] scoreThresholds;
    [SerializeField]
    private ItemScriptableObject[] itemsPerScore;
    private int currentScoreThreshold;
    private StatManager statManager;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        currentScoreThreshold = 0;
        statManager = FindAnyObjectByType<StatManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddScore(75);
            AddItemsByScore();
        }
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

    public void AddNoteHit()
    {
        notesHit++;
        currentScore += scorePerNote;
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    private void AddItemsByScore()
    {
        while (currentScoreThreshold < scoreThresholds.Length &&
            currentScore <= scoreThresholds[currentScoreThreshold])
        {
            statManager.AddItemToInventory(itemsPerScore[currentScoreThreshold]);
            currentScoreThreshold++;
        }
    }
}
