using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;

    [Header("Endgame Canvas UI")]
    [SerializeField]
    private Canvas endgameCanvas;
    [SerializeField]
    private TextMeshProUGUI endgameTextbox;

    [HideInInspector]
    public int currentScore;
    private int totalScore;

    //default, will go up based on streak
    private int scorePerNote = 10;
    private int notesHit;

    [Header("Item Unlock Thresholds")]
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
        totalScore = 0;
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

    public void DisplayEndgameCanvas()
    {
        totalScore += currentScore;
        endgameTextbox.text = "Nightly Score: " + currentScore +
            "\n\nTotal Score: " + totalScore + "\n\n";
        AddItemsByScore();
        endgameCanvas.enabled = true;
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
        string newItemText = "";
        while (currentScoreThreshold < scoreThresholds.Length &&
            totalScore <= scoreThresholds[currentScoreThreshold])
        {
            statManager.AddItemToInventory(itemsPerScore[currentScoreThreshold]);
            newItemText += itemsPerScore[currentScoreThreshold].unlockFlavorText + "\n";
            currentScoreThreshold++;
        }
        endgameTextbox.text += newItemText;
    }
}
