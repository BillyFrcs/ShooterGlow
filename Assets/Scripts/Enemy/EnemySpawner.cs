using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Tooltip("Reference Enemy Prefabs")] [SerializeField] private GameObject[] _Enemy;
        
        [Tooltip("Minimum Object To Spawn")] [SerializeField] private int _minAmount;
        [Tooltip("Maximum Object To Spawn")] [SerializeField] private int _maxAmount;

        private int _amount;

        private bool _canSpawn;
        
        private List<GameObject> _EnemyList = new List<GameObject>();

        public static EnemySpawner Instance;

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
            SpawnEnemy();
            
            StartCoroutine(SpawnEnemy(Random.Range(1.0F, 5.0F)));
        }
        
        /// <summary>
        /// Spawn enemy with random position
        /// </summary>
        private void SpawnEnemy()
        {
            _amount = Random.Range(_minAmount, _maxAmount);

            if (_Enemy.Length > 0 && !_canSpawn)
            {
                for (uint i = 0; i < _amount; i++)
                {
                    foreach (GameObject enemy in _Enemy)
                    {
                        _EnemyList.Add(Instantiate(enemy, new Vector3(RandomGenerator().x, RandomGenerator().y, RandomGenerator().z), Quaternion.identity));
                    }
                }
            }
            else
            {
                throw new Exception("List is empty!");
            }
        }

        /// <summary>
        /// Spawn enemy with timer
        /// <param name="timer">Time to spawn every frame (float)</param>
        /// </summary>
        private IEnumerator SpawnEnemy(float timer)
        {
            while (true)
            {
                yield return new WaitForSeconds(timer);

                _amount = Random.Range(_minAmount, _maxAmount);

                if (_Enemy.Length > 0 && !_canSpawn)
                {
                    for (uint i = 0; i < _amount; i++)
                    {
                        foreach (GameObject enemy in _Enemy)
                        {
                            _EnemyList.Add(Instantiate(enemy, new Vector3(RandomGenerator().x, RandomGenerator().y, RandomGenerator().z), Quaternion.identity));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Create random generator of object position
        /// </summary>
        /// <returns>Vector3</returns>
        private static Vector3 RandomGenerator()
        {
            Vector3 spawnPosition;

            spawnPosition.x = Random.Range(-18f, 20f);
            spawnPosition.y = Random.Range(-29f, 29f);
            spawnPosition.z = -1f;
            
            return new Vector3(spawnPosition.x, spawnPosition.y, spawnPosition.z);
        }

        /// <summary>
        /// Get spawn enemy boolean value
        /// </summary>
        public bool SpawnEnemies
        {
            get
            {
                return _canSpawn;
            }
            set
            {
                _canSpawn = value;
            }
        }
    }
}