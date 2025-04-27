using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI showScoreText;

    [Header("Endgame Canvas UI")]
    [SerializeField]
    private GameObject endgameCanvas;
    [SerializeField]
    private TextMeshProUGUI endgameTextbox;

    [HideInInspector]
    public int currentScore;
    public int showScore;
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
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    AddScore(75);
        //    AddItemsByScore();
        //}
    }

    private void Start()
    {
        currentScore = 0;
        totalScore = 0;
        UpdateScoreText();
        endgameCanvas.SetActive(false);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
    }

    public void AddNoteHit()
    {
        notesHit++;
        showScore += scorePerNote;
        UpdateShowScoreText();
    }

    public void DisplayEndgameCanvas()
    {
        totalScore += currentScore + showScore;
        endgameTextbox.text = "Nightly Score: " + currentScore +
            "\n\nTotal Score: " + totalScore + "\n\n";
        AddItemsByScore();
        endgameCanvas.SetActive(true);
    }

    public void ResetDailyScore()
    {
        currentScore = 0;
        showScore = 0;
        endgameCanvas.SetActive(false);
        FindAnyObjectByType<PauseMenu>().IncrementPhase();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }

    private void UpdateShowScoreText()
    {
        if (showScoreText != null)
        {
            showScoreText.text = "Show Score: " + showScore;
        }
        else
        {
            Debug.Log("SHOW SCORE TEXT IS NULL");
        }
    }

    private void AddItemsByScore()
    {
        string newItemText = "";
        while (currentScoreThreshold < scoreThresholds.Length &&
            totalScore >= scoreThresholds[currentScoreThreshold])
        {
            statManager.AddItemToInventory(itemsPerScore[currentScoreThreshold]);
            newItemText += itemsPerScore[currentScoreThreshold].unlockFlavorText + "\n";
            currentScoreThreshold++;
        }
        endgameTextbox.text += newItemText;
    }
}
