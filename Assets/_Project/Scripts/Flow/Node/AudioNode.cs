using Game.Audio;
using UnityEngine;

namespace Game.Flow
{
    public class AudioNode : FlowNode
    {
        public enum AudioChannel
        {
            Music,
            SFX
        }

        public enum AudioAction
        {
            Play,
            Stop,
            Pause,
            Resume
        }

        [Header("Audio")]
        [SerializeField] private AudioChannel channel;

        [SerializeField] private AudioAction action;

        [SerializeField] private AudioClip clip;

        [SerializeField] private bool loop = true;

        public override void Enter()
        {
            AudioManager manager = AudioManager.Instance;

            switch (channel)
            {
                case AudioChannel.Music:
                    HandleMusic(manager);
                    break;

                case AudioChannel.SFX:
                    HandleSFX(manager);
                    break;
            }

            Complete(nextNode);
        }

        private void HandleMusic(AudioManager manager)
        {
            switch (action)
            {
                case AudioAction.Play:
                    manager.PlayMusic(clip, loop);
                    break;

                case AudioAction.Stop:
                    manager.StopMusic();
                    break;

                case AudioAction.Pause:
                    manager.PauseMusic();
                    break;

                case AudioAction.Resume:
                    manager.ResumeMusic();
                    break;
            }
        }

        private void HandleSFX(AudioManager manager)
        {
            switch (action)
            {
                case AudioAction.Play:
                    manager.PlaySFX(clip);
                    break;

                // sementara diabaikan
                case AudioAction.Stop:
                case AudioAction.Pause:
                case AudioAction.Resume:
                    break;
            }
        }
    }
}