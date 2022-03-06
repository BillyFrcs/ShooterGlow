using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Player;
using TMPro;
using UnityEngine;

namespace UI.Score
{
    public static class SaveSystem
    {
        /// <summary>
        /// Save player data
        /// </summary>
        /// <param name="scoreGame">ScoreSystem</param>
        /// <param name="key">String</param>
        /// <param name="playerHighScore">TextMeshProUGUI[]</param>
        public static void SaveData(ScoreSystem scoreGame, String key, TextMeshProUGUI[] playerHighScore)
        {
            try
            {
                if (scoreGame.score > PlayerPrefs.GetInt(key, 0))
                {
                    PlayerPrefs.SetInt("High Score", scoreGame.score);

                    foreach (TextMeshProUGUI HighScore in playerHighScore)
                    {
                        HighScore.SetText(String.Concat($"High Score {Convert.ToString(scoreGame.highScore)}"));
                    }
                }
            }
            finally
            {
                PlayerPrefs.Save();
            }
        }
        
        /// <summary>
        /// Load player data
        /// </summary>
        /// <param name="scoreGame">ScoreSystem</param>
        /// <param name="key">String</param>
        /// <param name="playerHighScore">TextMeshProUGUI[]</param>
        public static void LoadData(ScoreSystem scoreGame, String key, TextMeshProUGUI[] playerHighScore)
        {
            try
            {
                if (PlayerPrefs.HasKey(key))
                {
                    scoreGame.highScore = PlayerPrefs.GetInt(key, 0);

                    foreach (TextMeshProUGUI highScore in playerHighScore)
                    {
                        highScore.SetText(String.Concat($"High Score {Convert.ToString(scoreGame.highScore)}"));
                    }
                }
            }
            catch (Exception error)
            {
                if (!PlayerPrefs.HasKey(key))
                {
                    throw new Exception(error.Message);
                }
            }
        }

        /// <summary>
        /// Delete player data
        /// </summary>
        /// <param name="scoreGame">ScoreSystem</param>
        /// <param name="key">String</param>
        /// <param name="playerHighScore">TextMeshProUGUI[]</param>
        public static void DeleteData(ScoreSystem scoreGame, String key, TextMeshProUGUI[] playerHighScore)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey("High Score");
                
                scoreGame.highScore = Convert.ToInt32(ResetScore.SCORE);
            
                foreach (var highScore in playerHighScore)
                {
                    highScore.SetText(String.Concat($"High Score {Convert.ToString(scoreGame.highScore)}"));
                }
            }
        }
    }
}