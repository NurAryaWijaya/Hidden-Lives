using Game.Audio;
using Game.Flow;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class OnFocusToCOmputer : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera sitDownCamera;
    [SerializeField] private CinemachineCamera computerCamera;
    [SerializeField] private int activePriority = 10;
    [SerializeField] private int inactivePriority = 0;

    [Header("Panel")]
    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private GameObject blurPanel;
    [SerializeField] private GameObject toScanPanel;

    [Header("Quetion")]
    [SerializeField] private QuizManager quizManager;

    public void StartComputer()
    {
        sitDownCamera.Priority = inactivePriority;
        computerCamera.Priority = activePriority;

        backgroundPanel.SetActive(true);

        StartCoroutine(ErrorShowOnScreen());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StopQuestion()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        sitDownCamera.Priority = activePriority;
        computerCamera.Priority = inactivePriority;

        FlowEventManager.Instance.Raise("PlayerFinishQuestion");
    }

    private IEnumerator ErrorShowOnScreen()
    {
        yield return new WaitForSeconds(3f);
        blurPanel.SetActive(true);
        AudioManager.Instance.PlaySFX(quizManager.FailedSound);
        toScanPanel.SetActive(true);
    }

    public void OnButtonFixIt()
    {
        StartQuestion();
    }

    private void StartQuestion()
    {
        toScanPanel.SetActive(false);
        quizManager.StartQuestion();
    }
}
