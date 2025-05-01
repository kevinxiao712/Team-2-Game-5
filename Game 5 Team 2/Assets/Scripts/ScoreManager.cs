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
    public int preshowScore;
    public int showScore;
    public int postshowScore;
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
        preshowScore = 0;
        totalScore = 0;
        UpdateScoreText();
        endgameCanvas.SetActive(false);
    }

    public void AddScore(int amount)
    {
        preshowScore += amount;
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
        totalScore += preshowScore + showScore + postshowScore;
        endgameTextbox.text = "Preshow Score: " + preshowScore +
            "\nShow score: " + showScore +
            "\nPostshow score: " + postshowScore +
            "\n\nTotal Score: " + totalScore + "\n\n";
        AddItemsByScore();
        endgameCanvas.SetActive(true);
    }

    public void ResetDailyScore()
    {
        preshowScore = 0;
        showScore = 0;
        postshowScore = 0;
        endgameCanvas.SetActive(false);
        FindAnyObjectByType<PauseMenu>().IncrementPhase();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            if (preshowScore <= 10)
            {
                scoreText.text = "Prep: BAD";
            }

            if (preshowScore > 10 && preshowScore <= 20)
            {
                scoreText.text = "Prep: PASSABLE";
            }

            if (preshowScore > 20 && preshowScore <= 50)
            {
                scoreText.text = "Prep: GOOD";
            }

            if (preshowScore > 50)
            {
                scoreText.text = "Prep: PERFECT";
            }
        }
    }

    private void UpdateShowScoreText()
    {
        if (showScoreText != null)
        {
            if (showScore <= 90)
            {
                showScoreText.text = "Performance: NAUSEATING";
            }

            if (showScore > 90 && showScore < 180)
            {
                showScoreText.text = "Performance: PASSABLE";
            }

            if (showScore > 180 && showScore < 270)
            {
                showScoreText.text = "Performance: GOOD";
            }

            if (showScore > 270)
            {
                showScoreText.text = "Performance: PERFECT";
            }
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
