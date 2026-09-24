using UnityEngine;
using TMPro;
using Game.Flow;
using System.Collections;

public class BumperCarMatch : MonoBehaviour, IFlowActivatable
{
    public static BumperCarMatch Instance { get; private set; }

    [Header("Match")]
    [SerializeField] private float matchDuration = 120f;

    [Header("Prefab")]
    [SerializeField] private GameObject prefabPlayer;
    [SerializeField] private GameObject arenaMatch;
    [SerializeField] private GameObject bombomcarStatic;

    [Header("UI")]
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject pointandtimer;

    private float remainingTime;

    private int playerScore;

    private bool matchFinished = false;

    private bool isStartedMatch = false;

    public bool IsMatchStarted => isStartedMatch;
    public int PlayerScore => playerScore;
    public float RemainingTime => remainingTime;
    public bool MatchFinished => matchFinished;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        arenaMatch.SetActive(false);
        bombomcarStatic.SetActive(true);
        pointandtimer.SetActive(false);
    }

    private void Update()
    {
        if (!isStartedMatch)
            return;

        if (matchFinished)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;

            UpdateTimerUI();

            FinishMatch();

            return;
        }

        UpdateTimerUI();
    }

    public void StartMatch()
    {
        remainingTime = matchDuration;

        isStartedMatch = true;

        playerScore = 0;

        matchFinished = false;

        // Pastikan panel hasil disembunyikan
        if (winPanel != null)
            winPanel.SetActive(false);

        if (losePanel != null)
            losePanel.SetActive(false);

        pointandtimer.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdatePointUI();
        UpdateTimerUI();
    }

    public void PlayerHitNPC(BumperCarCombat.HitDirection hitDirection)
    {
        if (matchFinished)
            return;

        switch (hitDirection)
        {
            case BumperCarCombat.HitDirection.Front:
                AddScore(0);
                break;

            case BumperCarCombat.HitDirection.Side:
                AddScore(2);
                break;

            case BumperCarCombat.HitDirection.Rear:
                AddScore(4);
                break;
        }
    }

    public void NPCHitPlayer(BumperCarCombat.HitDirection hitDirection)
    {
        if (matchFinished)
            return;

        switch (hitDirection)
        {
            case BumperCarCombat.HitDirection.Front:
                AddScore(0);
                break;

            case BumperCarCombat.HitDirection.Side:
                AddScore(-2);
                break;

            case BumperCarCombat.HitDirection.Rear:
                AddScore(-4);
                break;
        }
    }

    private void AddScore(int amount)
    {
        playerScore += amount;

        UpdatePointUI();

        Debug.Log(
            $"Player Score: {playerScore}"
        );
    }

    private void UpdatePointUI()
    {
        if (pointText == null)
            return;

        pointText.text = $"POINT: {playerScore}";
    }

    private void UpdateTimerUI()
    {
        if (timerText == null)
            return;

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void FinishMatch()
    {
        if (matchFinished)
            return;

        matchFinished = true;

        isStartedMatch = false;

        if (playerScore > 0)
        {
            Win();
        }
        else
        {
            Lose();
        }
    }

    private void Win()
    {
        Debug.Log("PLAYER WIN");

        if (winPanel != null)
            winPanel.SetActive(true);
        pointandtimer.SetActive(false);

        StartCoroutine(EndMatch());
    }

    private void Lose()
    {
        Debug.Log("PLAYER LOSE");

        if (losePanel != null)
            losePanel.SetActive(true);
        pointandtimer.SetActive(false);

        StartCoroutine(EndMatch());
    }

    public void Activate()
    {
        prefabPlayer.SetActive(false);
        arenaMatch.SetActive(true);
        bombomcarStatic.SetActive(false);
        tutorialPanel.SetActive(true);
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(6f);
        tutorialPanel.SetActive(false);
        StartMatch();
    }
    public void Deactivate()
    {
        arenaMatch.SetActive(false );
        bombomcarStatic?.SetActive(true); 
        pointandtimer.SetActive(false);
    }

    private IEnumerator EndMatch()
    {
        yield return new WaitForSeconds(10f);

        TransitionManager.Instance.PlayTransition();

        yield return new WaitForSeconds(2f);

        FlowEventManager.Instance.Raise("Aftermatch");
    }
}