using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Flow
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        public event Action SceneLoaded;

        private bool isLoading;

        public bool IsLoading => isLoading;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void LoadScene(string sceneName)
        {
            if (isLoading)
            {
                Debug.LogWarning("SceneLoader : Scene sedang dimuat.");
                return;
            }

            StartCoroutine(LoadRoutine(sceneName));
        }

        private IEnumerator LoadRoutine(string sceneName)
        {
            isLoading = true;

            AsyncOperation operation =
                SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                yield return null;
            }

            isLoading = false;

            SceneLoaded?.Invoke();
        }
    }
}