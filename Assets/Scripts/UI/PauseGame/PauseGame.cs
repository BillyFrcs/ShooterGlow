using System;
using System.Collections;
using System.Collections.Generic;
using Player.InputSystem;
using UI.Score;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.PauseGame
{
    public class PauseGame : MonoBehaviour
    {
        private PlayerInputSystemController _PlayerInputSystemController;
        
        [SerializeField] private GameObject[] _PauseGame;

        [SerializeField] private List<GameObject> _UIGameplay = new List<GameObject>();

        private bool _isGamePaused;
        
        private void Awake()
        {
            _PlayerInputSystemController = new PlayerInputSystemController();

            _PlayerInputSystemController.Menu.PauseGame.performed += OnPauseGame;
        }

        private void OnEnable()
        {
            _PlayerInputSystemController.Menu.Enable();
        }

        private void OnDisable()
        {
            _PlayerInputSystemController.Menu.Disable();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) 
                return;
            
            if (!_isGamePaused)
            {
                PauseGameplay();
            }
            else
            {
                ContinueGame();
            }
        }
        
        /// <summary>
        /// Pause game input action callback
        /// </summary>
        /// <param name="pauseGameContext">InputAction.CallbackContext</param>
        private void OnPauseGame(InputAction.CallbackContext pauseGameContext)
        {
            if (pauseGameContext.performed)
            {
                PauseGameplay();
                
                // Debug.LogAssertionFormat("Pause Game! " + pauseGameContext.action); // DEBUG ASSERTION FORMAT
            }
        }
        
        private void PauseGameplay()
        {
            Time.timeScale = 0f;
            
            ScoreSystem.Instance.LoadHighScoreGame();
            
            GamePausedUI(true);
            OnGameUI(false);
        }

        public void ContinueGame()
        {
            Time.timeScale = 1f;
            
            GamePausedUI(false);
            OnGameUI(true);
        }

        private void GamePausedUI(bool isActive)
        {
            foreach (var pauseGame in _PauseGame)
            {
                pauseGame.SetActive(isActive);
            }
        }

        private void OnGameUI(bool isActive)
        {
            var gameplayUI = 0;
            
            while (gameplayUI < _UIGameplay.Count)
            {
                _UIGameplay[gameplayUI].SetActive(isActive);

                gameplayUI++;
            }
        }
    }
}
