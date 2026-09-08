using Game.Flow;
using Game.Localization;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Cutscene
{
    public class TimelineBranchSignal : MonoBehaviour
    {
        [SerializeField]
        private PlayableDirector director;

        [SerializeField]
        private LocalizedBranchData branchData;

        public bool Result { get; private set; }

        public void ShowBranch()
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0);

            GameStateManager.Instance.SetState(GameState.Branch);

            TimelineBranchController.Instance.Selected -= HandleSelected;
            TimelineBranchController.Instance.Selected += HandleSelected;

            if (branchData == null)
            {
                Debug.LogWarning("LocalizedBranchData belum diisi.");
                return;
            }

            TimelineBranchController.Instance.Show(
                branchData.TrueText,
                branchData.FalseText);
        }

        private void HandleSelected(bool result)
        {
            TimelineBranchController.Instance.Selected -= HandleSelected;

            Result = result;

            GameStateManager.Instance.SetState(GameState.Cutscene);

            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }
}