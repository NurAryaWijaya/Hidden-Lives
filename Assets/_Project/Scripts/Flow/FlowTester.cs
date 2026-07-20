using Game.Audio;
using Unity.VectorGraphics;
using UnityEngine;

namespace Game.Flow
{
    public class FlowTester : MonoBehaviour
    {
        public void GoToMainMenu()
        {
            AudioManager.Instance.StopMusic();
            SceneLoader.Instance.LoadScene("MainMenu");
        }
    }
}