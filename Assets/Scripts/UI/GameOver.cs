using System.Collections;
using System.Collections.Generic;
using UI.Score;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private GameObject[] _GameOver;
        
        [SerializeField] private List<GameObject> _UIGameplay = new List<GameObject>();
        
        public static GameOver Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

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
