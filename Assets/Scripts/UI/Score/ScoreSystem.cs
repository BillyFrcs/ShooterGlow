using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Player;
using UnityEngine;
using TMPro;

namespace UI.Score
{
    public interface IScoreSystem
    {
        public int IncreaseScorePlayer();

        public void SaveHighScoreGame();
        public void LoadHighScoreGame();
        public void ResetHighScoreGame();
    }

    public enum ResetScore
    {
        Score = 0
    }

    public class ScoreSystem : MonoBehaviour, IScoreSystem
    {
        [Header("Score Game")] 
        [Tooltip("Score Text Reference")] [SerializeField] private List<TextMeshProUGUI> _Score = new List<TextMeshProUGUI>();

        [HideInInspector] public int score;
        
        [Header("High Score")] 
        [Tooltip("High Score Text Reference")] [SerializeField] private TextMeshProUGUI[] _HighScore;

        [HideInInspector] public int highScore;

        private const string KEY = "High Score";
        
        public static ScoreSystem Instance;

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
            LoadHighScoreGame();
        }
        
        // Update is called once per frame
        private void Update()
        {
            // Update score game
            foreach (TextMeshProUGUI scoreGame in _Score)
            {
                scoreGame.SetText(String.Concat($"Score: {Convert.ToString(score)}"));
            }
        }

        /// <summary>
        /// Increase score game 
        /// </summary>
        /// <returns>int</returns>
        public int IncreaseScorePlayer()
        {
            return score += 10;
        }

        /// <summary>
        /// Save high score game 
        /// </summary>
        public void SaveHighScoreGame()
        {
            SaveSystem.SaveData(this, KEY, _HighScore);
        }
        
        /// <summary>
        /// Load high score game
        /// </summary>
        public void LoadHighScoreGame()
        {
            SaveSystem.LoadData(this, KEY, _HighScore);
        }

        /// <summary>
        /// Reset score system when restart the game
        /// </summary>
        public void ResetHighScoreGame()
        {
            SaveSystem.DeleteData(this, KEY, _HighScore);
        }
    }
}