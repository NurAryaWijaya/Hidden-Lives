using System.IO;
using UnityEngine;

namespace Game.Flow
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private SaveData saveData = new();

        private string SavePath =>
            Path.Combine(
                Application.persistentDataPath,
                "save.json");

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        #region Save

        public void SaveCheckpoint(CheckpointInfo checkpoint)
        {
            int index =
                saveData.checkpoints.FindIndex(
                    c => c.CheckpointId == checkpoint.CheckpointId);

            if (index >= 0)
            {
                saveData.checkpoints[index] = checkpoint;
            }
            else
            {
                saveData.checkpoints.Add(checkpoint);
            }

            WriteFile();
        }

        #endregion

        #region Load

        public void Load()
        {
            if (!File.Exists(SavePath))
            {
                saveData = new SaveData();
                return;
            }

            string json =
                File.ReadAllText(SavePath);

            saveData =
                JsonUtility.FromJson<SaveData>(json);

            if (saveData == null)
                saveData = new SaveData();

            foreach (CheckpointInfo checkpoint in saveData.checkpoints)
            {
                Debug.Log(CheckpointManager.Instance);
                CheckpointManager.Instance.Register(checkpoint);
            }
        }

        #endregion

        #region Reset

        public void Delete()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);

            saveData = new SaveData();

            CheckpointManager.Instance.Clear();
        }

        #endregion

        #region Helper

        private void WriteFile()
        {
            string json =
                JsonUtility.ToJson(
                    saveData,
                    true);

            File.WriteAllText(
                SavePath,
                json);
        }

        public bool HasCheckpoint(string checkpointId)
        {
            return saveData.checkpoints.Exists(
                c => c.CheckpointId == checkpointId);
        }

        public bool TryGetCheckpoint(
            string checkpointId,
            out CheckpointInfo checkpoint)
        {
            checkpoint =
                saveData.checkpoints.Find(
                    c => c.CheckpointId == checkpointId);

            return checkpoint != null;
        }

        #endregion
    }
}