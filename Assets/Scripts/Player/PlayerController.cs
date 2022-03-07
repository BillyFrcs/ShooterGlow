using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bullets;
using Enemy;
using Player.InputSystem;
using Tags;
using UI;
using UI.Score;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour, PlayerInputSystemController.IPlayerActions
    {
        private PlayerInputSystemController _PlayerInputSystemController;

        private bool _isMovementPressed;
        private bool _isMove;
        [SerializeField] private float _movementSpeed = 2f;

        private Vector2 _MovementInput;
        private Vector3 _PlayerMovement;

        [Header("Bullet Controller")] 
        [Tooltip("Spawn Bullet Prefabs")] [SerializeField] private GameObject[] _Bullet;
        
        [Tooltip("Shoot Point Position")] [SerializeField] private Transform _ShootPoint;
        
        private bool _canShoot;
        private bool _isShootPressed;
        private float _fireRate;
        private float _fireTime;

        public static PlayerController Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            _PlayerInputSystemController = new PlayerInputSystemController();
            
            this.PlayerInputSystemActionCallback();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _isMove = true;
            _canShoot = true;
            
            _fireRate = 0.09f;
            _fireTime = 0.0f;
        }

        // Update is called once per frame
        private void Update()
        {
#if ENABLE_INPUT_SYSTEM
            PlayerMovement();
            
            PlayerShoot();
#else
            // Old Unity input system
            if (Input.GetButton("Fire1") && _canShoot)
            {
                PlayerShoot();
            }
#endif
        }
        
        private void OnEnable()
        {
            _PlayerInputSystemController.Player.Enable();
        }

        private void OnDisable()
        {
            _PlayerInputSystemController.Player.Disable();
        }

        private void PlayerInputSystemActionCallback()
        {
            // Movement input action
            _PlayerInputSystemController.Player.Movement.started += OnMovement;
            _PlayerInputSystemController.Player.Movement.canceled += OnMovement;
            _PlayerInputSystemController.Player.Movement.performed += OnMovement;
            
            // Shoot input action
            _PlayerInputSystemController.Player.Shoot.started += OnShoot;
            _PlayerInputSystemController.Player.Shoot.canceled += OnShoot;
        }

        public void OnMovement(InputAction.CallbackContext movementContext)
        {
            if (_isMove)
            {
                _MovementInput = movementContext.ReadValue<Vector2>();
                
                _PlayerMovement.x = _MovementInput.x * _movementSpeed;
                _PlayerMovement.y = _MovementInput.y * _movementSpeed;

                _isMovementPressed = _MovementInput.x != 0F || _MovementInput.y != 0F;
            }
        }

        public void OnShoot(InputAction.CallbackContext shootContext)
        {
            if (_canShoot)
            {
                _isShootPressed = shootContext.ReadValueAsButton();
                    
                // Debug.Log(shootContext); // DEBUG
            }
        }

        private void PlayerMovement()
        {
            if (_isMovementPressed)
            {
                gameObject.transform.Translate(_PlayerMovement * Time.deltaTime);
                
#if !ENABLE_INPUT_SYSTEM
                // Old unity input system
                _PlayerMovement.x = Input.GetAxis("Horizontal");
                _PlayerMovement.y = Input.GetAxis("Vertical");

                _PlayerMovement = new Vector3(_PlayerMovement.x, _PlayerMovement.y, 0f) * _movementSpeed * Time.deltaTime;

                gameObject.transform.Translate(_PlayerMovement);
#endif
            }

            PlayerAimCursor();
        }
        
        /// <summary>
        /// Player shoot bullet
        /// </summary>
        private void PlayerShoot()
        {
            if (_isShootPressed)
            {
                // Limit the bullet shoot
                if (Time.time > _fireTime)
                {
                    _fireTime = Time.time + _fireRate;

                    BulletController.ShootBullet(_Bullet, _ShootPoint);
                }
            }
        }
        
        /// <summary>
        /// Player aim cursor to shoot
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PlayerAimCursor()
        {
            Vector3 mouseDirection = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
#if !ENABLE_INPUT_SYSTEM
            // Use this if we use the Unity old input system
            Vector3 mousePosition = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif
            
            Vector2 direction = mouseDirection - transform.position;

            var angle = Vector2.SignedAngle(Vector2.right, direction);

            this.gameObject.transform.eulerAngles = new Vector3(0f, 0f, angle);
        }

        /// <summary>
        /// Get player object
        /// </summary>
        public GameObject PlayerObject
        {
            get
            {
                return this.gameObject;
            }
        }

        /// <summary>
        /// Get bullet point position
        /// </summary>
        /// <returns>Bullet point (Transform)</returns>
        public Transform BulletPoint()
        {
            return _ShootPoint;
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(TagManager.Collision) || collision.gameObject.CompareTag(TagManager.Enemy))
            {
                Destroy(this.gameObject);

                GameOver.Instance.DisplayGameOver();

                EnemyAIController.Instance.EnemyMove = false;

                EnemySpawner.Instance.SpawnEnemies = true;
            }
            
            Physics.IgnoreLayerCollision(6, 7, true);
        }
    }
}
