using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Tags;
using UI.Score;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using ColorParameter = UnityEngine.Rendering.ColorParameter;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyAIController : MonoBehaviour
    {
        [Tooltip("Enemy Movement Speed")] [SerializeField] private float _movementSpeed = 2f;
        
        [Tooltip("Enemy Distance To The Target")] [SerializeField] private float _distanceToTarget = 0.1f;

        [SerializeField] private GameObject _EnemyObject;
        
        private float _distance;

        private bool _isMove;

        // Start is called before the first frame update
        private void Start()
        {
            if (TryGetComponent(out SpriteRenderer enemySprite))
            {
                enemySprite.color = Random.ColorHSV(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f, 1f, 1f, 1f, 1f);
            }

            _isMove = true;
            
            GenerateRandomSizeObject();
        }

        // Update is called once per frame
       private void Update()
       {

       }

       private void FixedUpdate()
       {
           if (PlayerController.Instance != null)
           {
               EnemyMovement();
           }
       }
       
       /// <summary>
       /// Enemy movement controller
       /// </summary>
       private void EnemyMovement()
       {
           if (_isMove)
           {
               _distance = Vector2.Distance(gameObject.transform.position, PlayerController.Instance.PlayerObject.transform.position);

               // Movement with distance specified
               if (_distance > _distanceToTarget)
               {
                   if (TryGetComponent(out Rigidbody2D enemyRb))
                   {
                       enemyRb.MovePosition(Vector3.MoveTowards(gameObject.transform.position, PlayerController.Instance.PlayerObject.transform.position, Mathf.Abs(_movementSpeed * Time.fixedDeltaTime)));
                   }
               }
           }
       }

       /// <summary>
       /// Generate random size object of enemy
       /// </summary>
       void GenerateRandomSizeObject()
       {
           var scale = Random.Range(+0.3f, +1f);

           _EnemyObject.transform.localScale = new Vector2(scale, scale);
           
           // This is just another alternative
           // gameObject.transform.localScale = new Vector2(scale, scale);
       }

       private void OnCollisionEnter2D(Collision2D collision)
       {
           if (collision.gameObject.CompareTag(TagManager.Collision) || collision.gameObject.CompareTag(TagManager.Bullet))
           {
               Destroy(this.gameObject);
           }

           if (collision.gameObject.CompareTag(TagManager.Bullet) == true)
           {
               ScoreSystem.Instance.IncreaseScorePlayer();
           }
           
           Physics.IgnoreLayerCollision(2, 3, true);
       }
    }
}
