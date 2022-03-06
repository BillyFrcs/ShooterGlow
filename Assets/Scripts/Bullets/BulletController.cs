using System.Collections;
using System.Collections.Generic;
using Player;
using Tags;
using UnityEngine;

namespace Bullets
{
    public class BulletController : MonoBehaviour
    {
        [Tooltip("Bullet Speed")] [SerializeField] private float _bulletSpeed;

        private float _destroyBullet;

        // Start is called before the first frame update
        private void Start()
        {
            if (TryGetComponent<Rigidbody2D>(out Rigidbody2D bulletRb))
            {
                bulletRb.velocity = PlayerController.Instance.BulletPoint().transform.right * _bulletSpeed;
            }
        }

        // Update is called once per frame
        private void Update()
        {
            DestroyBullet();
        }
        
        /// <summary>
        /// Destroy bullet after amount of time
        /// </summary>
        private void DestroyBullet()
        {
            _destroyBullet += Time.deltaTime;

            if (_destroyBullet > 3f)
            {
                Destroy(this.gameObject);

                // Debug.Log("Destroy bullet"); // DEBUG
            }
        }

        public void ShootBullet(GameObject[] bullet, Transform shootPoint)
        {
            foreach (GameObject newBullet in bullet)
            {
                GameObject.Instantiate(newBullet, shootPoint.transform.position, Quaternion.identity);
            }
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(TagManager.Collision))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
