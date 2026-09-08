using Game.Flow;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RenoStealthTask : MonoBehaviour, IFlowActivatable
{
    [Header("Prefab")]
    [SerializeField] private GameObject component;
    [SerializeField] private GameObject raishaSleepMesh;
    [SerializeField] private GameObject raishaCatchMesh;
    [SerializeField] private GameObject IntructionPanel;

    [Header("Point")]
    [SerializeField] private Transform pointTarget;
    [SerializeField] private Transform player;

    [Header("Icon")]
    [SerializeField] private GameObject raishaSleepIcon;
    [SerializeField] private GameObject raishaAlertIcon;

    [Header("Script")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private MouseLook mouseLookscript;

    [Header("Reset")]
    [SerializeField] private Vector3 position;
    [SerializeField] private Vector3 rotation;

    bool isGameOver = false;
    private Coroutine stealthCoroutine;
    void Start()
    {
        raishaCatchMesh.SetActive(false);
        raishaSleepMesh.SetActive(false);
        
        raishaSleepIcon.SetActive(false);
        raishaAlertIcon.SetActive(false);

        IntructionPanel.SetActive(false);
    }
    private void Update()
    {
        raishaCatchMesh.transform.LookAt(player);
    }

    private IEnumerator ShowInstruction()
    {
        IntructionPanel.SetActive(true);

        GameStateManager.Instance.SetState(GameState.Pause);

        yield return new WaitForSeconds(5f);

        GameStateManager.Instance.SetState(GameState.Exploration);
    }

    private void StartStealth()
    {
        IntructionPanel.SetActive(false);
        raishaSleepMesh.SetActive(true);

        isGameOver = false;

        if (stealthCoroutine != null)
        {
            StopCoroutine(stealthCoroutine);
        }

        stealthCoroutine = StartCoroutine(RaishaSleep());
    }

    private IEnumerator RaishaSleep()
    {
        while (!isGameOver)
        {
            raishaSleepIcon.SetActive(true);
            raishaAlertIcon.SetActive(false);

            float randomTime = Random.Range(3f, 5f);
            yield return new WaitForSeconds(randomTime);

            yield return StartCoroutine(RaishaAlert());
        }
    }

    private IEnumerator RaishaAlert()
    {
        raishaSleepIcon.SetActive(false);
        raishaAlertIcon.SetActive(true);

        float randomTime = Random.Range(3f, 5f);
        float timer = 0;

        while (timer < randomTime)
        {
            if (playerMovement.IsMoving)
            {
                yield return new WaitForSeconds(0.5f);
                if (playerMovement.IsMoving)
                {
                    GameOver();
                    break;
                }
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        if (isGameOver)
            return;
        isGameOver = true;
        raishaSleepMesh.SetActive(false);
        raishaCatchMesh.SetActive(true);

        StartCoroutine(WaitInGameOver());
    }

    private IEnumerator WaitInGameOver()
    {
        yield return StartCoroutine(mouseLookscript.LookAtTarget(pointTarget));

        yield return new WaitForSeconds(3f);

        TransitionManager.Instance.PlayTransition();
        yield return new WaitForSeconds(3f);

        PlayerMovement movement = component.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.Teleport(
                position,
                Quaternion.Euler(rotation));
        }
        GameStateManager.Instance.SetState(GameState.Exploration);
        StopQuest();
        StartStealth();
    }

    public void StopQuest()
    {
        isGameOver = true;

        if (stealthCoroutine != null)
        {
            StopCoroutine(stealthCoroutine);
            stealthCoroutine = null;
        }

        raishaCatchMesh.SetActive(false);
        raishaSleepMesh.SetActive(false);

        raishaSleepIcon.SetActive(false);
        raishaAlertIcon.SetActive(false);

        Debug.Log("Stop Task");
    }

    private IEnumerator FirstGame()
    {
        yield return StartCoroutine(ShowInstruction());
        StartStealth();
    }

    public void Activate()
    {
        StartCoroutine(FirstGame());
    }

    public void Deactivate() 
    { 

    }
}
