using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private Vector2 m_Direction;
        [SerializeField] private int m_Damage;

        private bool m_FromPlayer;
        //private SpawnManager m_SpawnManager;
        private float m_LifeTime;
        private float m_CurMoveSpeed;

        // Start is called before the first frame update
        void Start()
        {
            //m_SpawnManager = FindObjectOfType<SpawnManager>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(m_Direction * Time.deltaTime * m_CurMoveSpeed);

            m_LifeTime -= Time.deltaTime;
            if (m_LifeTime <= 0)
            {
                if (m_FromPlayer)
                    SpawnManager.Instance.ReleasePlayerProjectile(this);
                else
                    SpawnManager.Instance.ReleaseEnemyProjectile(this);
            }

        }

        public void Fire(float speedMultiplier)
        {
            m_LifeTime = 10f / speedMultiplier;
            m_CurMoveSpeed = m_MoveSpeed * speedMultiplier;
            //Destroy(gameObject, 10f);
        }

        public void SetFromPlayer(bool fromPlayer)
        {
            m_FromPlayer = fromPlayer;
        }

        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    Debug.Log(collision.gameObject.name);
        //}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (m_FromPlayer)
                    SpawnManager.Instance.ReleasePlayerProjectile(this);
                else
                    SpawnManager.Instance.ReleaseEnemyProjectile(this);

                Vector3 hitPos = collision.ClosestPoint(transform.position);
                SpawnManager.Instance.SpawnHitFX(hitPos);

                EnemyController enemy;
                collision.gameObject.TryGetComponent(out enemy);
                enemy.Hit(m_Damage);
            }

            if (collision.gameObject.CompareTag("Player"))
            {
                if (m_FromPlayer)
                    SpawnManager.Instance.ReleasePlayerProjectile(this);
                else
                    SpawnManager.Instance.ReleaseEnemyProjectile(this);

                Vector3 hitPos = collision.ClosestPoint(transform.position);
                SpawnManager.Instance.SpawnHitFX(hitPos);

                PlayerController player;
                collision.gameObject.TryGetComponent(out player);
                player.Hit(m_Damage);
            }
        }
    }
}