using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject pausePanel;

    private PlayerInputHandler playerInput;
    private bool isPaused;

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
        FindPlayerInput();

        pausePanel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (playerInput == null)
        {
            FindPlayerInput();
            return;
        }

        if (playerInput.PausePressed)
        {
            playerInput.ConsumeInput();

            TogglePause();
        }
    }

    private void FindPlayerInput()
    {
        playerInput = FindFirstObjectByType<PlayerInputHandler>();

        if (playerInput == null)
        {
            
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        pausePanel.SetActive(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}