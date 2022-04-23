using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BayatGames.SaveGameFree;
using Player;
using TMPro;
using UnityEngine;

namespace UI.Score
{
    public static class SaveSystem
    {
        /// <summary>
        /// Save player's data game
        /// </summary>
        /// <param name="scoreGame">ScoreSystem</param>
        /// <param name="identifier">String</param>
        /// <param name="playerData">TextMeshProUGUI[]</param>
        /// <param name="isSave">Boolean</param>
        public static void SaveData(ScoreSystem scoreGame, String identifier, TextMeshProUGUI[] playerData, Boolean isSave)
        {
            if (isSave && scoreGame.score > 0)
            {
                scoreGame.highScore = scoreGame.score;

                SaveGame.Save<int>(identifier, scoreGame.highScore, true);

                foreach (TextMeshProUGUI playerScore in playerData)
                {
                    playerScore.SetText(String.Concat($"Your Score {Convert.ToString(scoreGame.highScore)}"));
                }
            }
        }
        
        /// <summary>
        /// Save player data with PlayerPrefs
        /// </summary>
        /// <param name="scoreGame">ScoreSystem</param>
        /// <param name="key">String</param>
        /// <param name="playerData">TextMeshProUGUI[]</param>
        public static void SaveData(ScoreSystem scoreGame, String key, TextMeshProUGUI[] playerData)
        {
            try
            {
                if (scoreGame.score > PlayerPrefs.GetInt(key, 0))
                {
                    PlayerPrefs.SetInt(key, scoreGame.score);
                    
                    scoreGame.highScore = scoreGame.score;

                    foreach (TextMeshProUGUI highScore in playerData)
                    {
                        highScore.SetText(String.Concat($"High Score {Convert.ToString(scoreGame.highScore)}"));
                    }
                }
            }
            finally
            {
                PlayerPrefs.Save();
            }
        }

        /// <summary>
        /// Load player's data game
        /// </summary>
        /// <param name="scoreGame">ScoreSystem</param>
        /// <param name="identifier">String</param>
        /// <param name="playerData">TextMeshProUGUI[]</param>
        /// <param name="isLoad">Boolean</param>
        public static void LoadData(ScoreSystem scoreGame, String identifier, TextMeshProUGUI[] playerData, Boolean isLoad)
        {
            var isExists = SaveGame.Exists(identifier);
            
            try
            {
                if (isLoad && isExists)
                {
                    scoreGame.highScore = scoreGame.score;

                    SaveGame.Load<int>(identifier, scoreGame.highScore, true);

                    foreach (TextMeshProUGUI playerScore in playerData)
                    {
                        playerScore.SetText(String.Concat($"Your Score {Convert.ToString(scoreGame.highScore)}"));
                    }
                }
            }
            catch (Exception exception)
            {
                if (!isExists)
                {
                    throw new Exception(exception.Message);
                }
            }
        }
        
        /// <summary>
        /// Load player data with PlayerPrefs
        /// </summary>
        /// <param name="scoreGame">ScoreSystem</param>
        /// <param name="key">String</param>
        /// <param name="playerData">TextMeshProUGUI[]</param>
        public static void LoadData(ScoreSystem scoreGame, String key, TextMeshProUGUI[] playerData)
        {
            var hasKey = PlayerPrefs.HasKey(key);
            
            try
            {
                if (hasKey)
                {
                    scoreGame.highScore = PlayerPrefs.GetInt(key, 0);

                    foreach (TextMeshProUGUI highScore in playerData)
                    {
                        highScore.SetText(String.Concat($"High Score {Convert.ToString(scoreGame.highScore)}"));
                    }
                }
            }
            catch (Exception error)
            {
                if (!hasKey)
                {
                    throw new Exception(error.Message);
                }
            }
        }
        
        /// <summary>
        /// Delete player's data game
        /// </summary>
        /// <param name="scoreGame">ScoreSystem</param>
        /// <param name="identifier">String</param>
        /// <param name="playerData">TextMeshProUGUI[]</param>
        /// <param name="isDelete">Boolean</param>
        public static void DeleteData(ScoreSystem scoreGame, String identifier, TextMeshProUGUI[] playerData, Boolean isDelete)
        {
            if (isDelete && SaveGame.Exists(identifier))
            {
                SaveGame.Delete(identifier);

                scoreGame.highScore = Convert.ToInt32(Reset.Score);

                foreach (var highScore in playerData)
                {
                    highScore.SetText(String.Concat($"Your Score {Convert.ToString(scoreGame.highScore)}"));
                }
            }
        }

        /// <summary>
        /// Delete player data with PlayerPrefs
        /// </summary>
        /// <param name="scoreGame">ScoreSystem</param>
        /// <param name="key">String</param>
        /// <param name="playerData">TextMeshProUGUI[]</param>
        public static void DeleteData(ScoreSystem scoreGame, String key, TextMeshProUGUI[] playerData)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                
                scoreGame.highScore = Convert.ToInt32(Reset.Score);
            
                foreach (var highScore in playerData)
                {
                    highScore.SetText(String.Concat($"High Score {Convert.ToString(scoreGame.highScore)}"));
                }
            }
        }
    }
}