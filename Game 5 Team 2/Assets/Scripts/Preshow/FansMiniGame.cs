using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
public class FansMiniGame : MonoBehaviour
{

    [Header("Instant-win item")]
    public ItemScriptableObject guaranteedItem;

    [Header("Characters")]
    public CharacterController2D allowedCharacterA;
    public CharacterController2D allowedCharacterB;

    [Header("Timers")]
    public float spawnDelay = 5f;   // how long after scene-load the box appears
    public float timeToDeal = 10f;  // countdown before auto-fail
    public float fillDuration = 15f; // bar-fill time after F press

    [Header("Scoring")]
    public int successReward = 10;
    public int failPenalty = 10;

    [Header("UI")]
    public Image fillBar;            // Filled-type Image


    SpriteRenderer sr;
    Collider2D col;

    float spawnTimer;
    float dealTimer;
    float fillTimer;

    enum State { Hidden, Countdown, Filling, Done }
    State state = State.Hidden;

    CharacterController2D actor;   // the one we freeze
    bool swapped = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        sr.enabled = false;     // hidden at scene start
        col.enabled = false;

        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(false);
            fillBar.fillAmount = 0f;
        }

        spawnTimer = spawnDelay;
    }

    void Update()
    {
        switch (state)
        {
            case State.Hidden:
                HiddenTick();
                break;
            case State.Countdown:
                CountdownTick();
                break;
            case State.Filling:
                FillingTick();
                break;
        }
    }

    void HiddenTick()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer > 0f) return;

        // show visuals
        sr.enabled = true;
        col.enabled = true;

        /* instant success if player already has item */
        if (PlayerHasGuaranteedItem())
        {
            Complete(success: true);
            return;
        }

        // start main countdown
        dealTimer = timeToDeal;
        state = State.Countdown;

        if (fillBar != null)
        {
            fillBar.gameObject.SetActive(true);
            fillBar.fillAmount = 1f;          // bar shows time left
        }
    }

    void CountdownTick()
    {
        // update bar
        dealTimer -= Time.deltaTime;
        if (fillBar != null)
            fillBar.fillAmount = Mathf.Clamp01(dealTimer / timeToDeal);

        if (dealTimer <= 0f)
        {
            Complete(success: false);          // time ran out
            return;
        }

        // check the one active character
        if (allowedCharacterA && allowedCharacterA.IsActive)
            TryPressF(allowedCharacterA);
        else if (allowedCharacterB && allowedCharacterB.IsActive)
            TryPressF(allowedCharacterB);
    }

    void TryPressF(CharacterController2D chr)
    {
        if (Vector2.Distance(transform.position, chr.transform.position) > 2f) return;
        if (!Input.GetKeyDown(KeyCode.F)) return;

        actor = chr;
        actor.FreezeForTask();            // sets IsBusy = true
        swapped = false;

        CharacterController2D other =
            (actor == allowedCharacterA) ? allowedCharacterB : allowedCharacterA;

        bool otherBusy = false;

        // is the other char already filling another FansMiniGame?
        foreach (FansMiniGame box in FindObjectsOfType<FansMiniGame>())
        {
            if (box != this && box.state == State.Filling && box.actor == other)
            {
                otherBusy = true;
                break;
            }
        }

        if (!otherBusy && other != null)
        {
            FindObjectOfType<GameManager>()?.SwitchActiveCharacter();
            swapped = true;
        }


        fillTimer = 0f;
        state = State.Filling;

        if (fillBar != null)
            fillBar.fillAmount = 0f;   // bar grows from 0 ¡ú 1
    }


    void FillingTick()
    {
        fillTimer += Time.deltaTime;
        if (fillBar != null)
            fillBar.fillAmount = Mathf.Clamp01(fillTimer / fillDuration);

        if (fillTimer >= fillDuration)
            Complete(success: true);
    }

    void Complete(bool success)
    {
        state = State.Done;

        fillBar?.gameObject.SetActive(false);

        if (actor != null)
            actor.Unfreeze(makeActive: !swapped);

        int delta = success ? successReward : -failPenalty;
        ScoreManager.Instance.AddScore(delta);

        // hide forever
        sr.enabled = false;
        col.enabled = false;
        gameObject.SetActive(false);
    }


    bool PlayerHasGuaranteedItem()
    {
        if (guaranteedItem == null) return false;
        foreach (var it in StatManager.Instance.ManagerItems)
            if (it == guaranteedItem) return true;
        return false;
    }
}
