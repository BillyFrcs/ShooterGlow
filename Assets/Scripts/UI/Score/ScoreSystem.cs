using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BayatGames.SaveGameFree;
using Player;
using UnityEngine;
using TMPro;

namespace UI.Score
{
    public interface IScoreSystem
    {
        public int IncreaseScorePlayer(in int score);

        public void SaveHighScoreGame();
        public void LoadHighScoreGame();
        public void ResetScoreGame();
    }

    public enum Reset
    {
        Score = 0
    }

    public class ScoreSystem : MonoBehaviour, IScoreSystem
    {
        [Header("Score Game")] 
        [Tooltip("Score Text Reference")] [SerializeField] private List<TextMeshProUGUI> _Score = new List<TextMeshProUGUI>();

        [HideInInspector] public int score;
        
        [Header("Player Score")]
        [Tooltip("Player Score Text Reference")] [SerializeField] private TextMeshProUGUI[] _PlayerScore;
        
        [Header("High Score")]
        [Tooltip("High Score Text Reference")] [SerializeField] private TextMeshProUGUI[] _HighScore;

        [HideInInspector] public int highScore;

        private const string IDENTIFIER = "High Score";
        private const string KEY = "Player Score";
        
        public static ScoreSystem Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
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
        /// <param name="newScore"></param>
        /// <returns>int</returns>
        public int IncreaseScorePlayer(in int newScore)
        {
            return this.score += newScore;
        }

        /// <summary>
        /// Save high score game 
        /// </summary>
        public void SaveHighScoreGame()
        {
            SaveSystem.SaveData(this, IDENTIFIER, _PlayerScore, true);
            SaveSystem.SaveData(this, KEY, _HighScore);

            // Debug.Log("Save"); // DEBUG
        }
        
        /// <summary>
        /// Load high score game
        /// </summary>
        public void LoadHighScoreGame()
        {
            SaveSystem.LoadData(this, IDENTIFIER, _PlayerScore, true);
            SaveSystem.LoadData(this, KEY, _HighScore);

            // Debug.Log("Load"); // DEBUG
        }

        /// <summary>
        /// Reset the current player score when restart the game
        /// </summary>
        public void ResetScoreGame()
        {
            score = (int) Reset.Score;
        }

        public void DeleteScoreGame()
        {
            SaveSystem.DeleteData(this, IDENTIFIER, _PlayerScore, true);
            SaveSystem.DeleteData(this, KEY, _HighScore);
        }
    }
}