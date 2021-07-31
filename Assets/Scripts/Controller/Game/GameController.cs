using Enumeral;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller.Game
{
    public class GameController : RootController
    {
        //Private-props
        private GameStatus _gameStatus;
        //Start
        private new void Start()
        {
            base.Start();
            Resume();
        }
        //Getter & Setter
        public void SetStatus(GameStatus status)
        {
            _gameStatus = status;
        }
        public GameStatus GetStatus()
        {
            return _gameStatus;
        }
        //Public-methods
        public void HandlePause(InputValue value)
        {
            if (_gameStatus.Equals(GameStatus.pause))
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        //Private-methods
        private void Pause()
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0f;
            SetStatus(GameStatus.pause);
        }
        private void Resume()
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1f;
            SetStatus(GameStatus.play);
        }
    }
}