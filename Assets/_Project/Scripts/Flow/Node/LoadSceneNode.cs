using Game.Flow;
using UnityEngine;

public class LoadSceneNode : FlowNode
{
    [Header("Scene")]
    [SerializeField]
    private string sceneName;

    public override void Enter()
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogWarning("LoadSceneNode : Scene Name belum diisi.");

            Complete(nextNode);
            return;
        }

        SceneLoader.Instance.SceneLoaded += HandleSceneLoaded;

        SceneLoader.Instance.LoadScene(sceneName);
    }

    private void HandleSceneLoaded()
    {
        SceneLoader.Instance.SceneLoaded -= HandleSceneLoaded;

        Complete(nextNode);
    }

    public override void Exit()
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.SceneLoaded -= HandleSceneLoaded;
        }
    }
}