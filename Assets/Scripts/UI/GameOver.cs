using System.Collections;
using System.Collections.Generic;
using Player.InputSystem;
using UI.Score;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOver : MonoBehaviour, PlayerInputSystemController.IMenuActions
    {
        [SerializeField] private GameObject[] _GameOver;
        
        [SerializeField] private List<GameObject> _UIGameplay = new List<GameObject>();

        private PlayerInputSystemController _PlayerInputSystemController;
        
        public static GameOver Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _PlayerInputSystemController = new PlayerInputSystemController();

            _PlayerInputSystemController.Menu.QuitGame.performed += OnQuitGame;
        }

        private void OnEnable()
        {
            _PlayerInputSystemController.Menu.Enable();
        }

        private void OnDisable()
        {
            _PlayerInputSystemController.Menu.Disable();
        }

        public void OnQuitGame(InputAction.CallbackContext quitGameContext)
        {
            if (quitGameContext.performed)
            {
                Application.Quit();
                
                // Debug.LogAssertionFormat("Quit Game! " + quitGameContext.action); // DEBUG ASSERTION FORMAT
            }
        }

        /// <summary>
        /// Display game over screen
        /// </summary>
        public void DisplayGameOver()
        {
            foreach (var gameOver in _GameOver)
            {
                gameOver.SetActive(true);
            }

            var gameplayUI = 0;
            
            while (gameplayUI < _UIGameplay.Count)
            {
                _UIGameplay[gameplayUI].SetActive(false);

                gameplayUI++;
            }
            
            // Save & load high score player
            ScoreSystem.Instance.SaveHighScoreGame();
            ScoreSystem.Instance.LoadHighScoreGame();
        }

        /// <summary>
        /// Start new game
        /// </summary>
        public void StartGame()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Quit game
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
            
            // Debug.LogAssertionFormat("Quit Game!"); // DEBUG ASSERTION FORMAT
        }
    }
}
