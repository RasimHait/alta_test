using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AltaTestWork
{
    public class HUD : SceneEntity
    {
        protected override void OnReady()
        {
            Debug.Log(1);
            if (TryGetBehaviour("PlayerSizeFillbar", out UIFillBar bar))
            {
                MessageBroker.Default
                    .Receive<Messages.Player.PlayerSizePercentChanged>()
                    .Subscribe(x => bar.ChangeValue(x.Percent / 100f))
                    .AddTo(bar.Container);
            }

            if (TryGetBehaviour("LoseScreen", out UIPopupScreen loseScreen))
            {
                MessageBroker.Default
                    .Receive<Messages.Player.PlayerFail>()
                    .Subscribe(x => loseScreen.Show())
                    .AddTo(loseScreen.Container);

                loseScreen.OnActionButton
                    .Subscribe(x => Restart())
                    .AddTo(loseScreen.Container);
            }

            if (TryGetBehaviour("WinScreen", out UIPopupScreen winScreen))
            {
                MessageBroker.Default
                    .Receive<Messages.Player.PlayerHasEnteredFinish>()
                    .Subscribe(x => winScreen.Show())
                    .AddTo(winScreen.Container);

                winScreen.OnActionButton
                    .Subscribe(x => Restart())
                    .AddTo(winScreen.Container);
            }
        }

        private void Restart()
        {
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}