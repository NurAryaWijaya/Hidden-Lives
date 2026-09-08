using Game.Audio;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class QuizManager : MonoBehaviour
{
    [SerializeField] private GameObject[] questionPanels;

    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private GameObject progressPanel;
    [SerializeField] private GameObject finishPanel;

    [SerializeField] private ComputerProgressBar progressBar;
    [SerializeField] private OnFocusToCOmputer onFocusToCOmputer;

    public AudioClip correctSound;
    public AudioClip FailedSound;

    private int currentQuestion = 0;

    public void StartQuestion()
    {
        StartCoroutine(BarProgress(false, true, false));
    }

    public void AnswerCorrect()
    {
        questionPanels[currentQuestion].SetActive(false);
        StartCoroutine(BarProgress(true, false, false));
    }

    public void AnswerWrong()
    {
        questionPanels[currentQuestion].SetActive(false);
        StartCoroutine(BarProgress(false, false, false));
    }

    public void NextQuestion()
    {
        successPanel.SetActive(false);
        failPanel.SetActive(false);

        currentQuestion++;

        if (currentQuestion >= questionPanels.Length)
        {
            FinishQuestion();
            return;
        }
        StartCoroutine(BarProgress(false, false, true));
    }

    private IEnumerator BarProgress(bool isCorrect, bool isFirstQuestion, bool isNext)
    {
        progressPanel.SetActive(true);
        progressBar.StartProgress();
        yield return new WaitUntil(() => progressBar.IsReady);

        progressBar.ResetProgress();
        progressBar.IsReady = false;
        progressPanel.SetActive(false);

        if (isFirstQuestion)
        {
            questionPanels[currentQuestion].SetActive(true);
            yield break;
        }

        if(isNext)
        {
            ShowQuestion();
            yield break;
        }

        if (isCorrect)
        {
            AudioManager.Instance.PlaySFX(correctSound);
            successPanel.SetActive(true);
        }
        else
        {
            AudioManager.Instance.PlaySFX(FailedSound);
            failPanel.SetActive(true);
        }
    }

    private void ShowQuestion()
    {
        questionPanels[currentQuestion].SetActive(true);
    }
    private void FinishQuestion()
    {
        finishPanel.SetActive(true);
        onFocusToCOmputer.StopQuestion();
    }
}
