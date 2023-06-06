using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private Transform[] m_WayPoints;
        //[SerializeField] private ProjectileController m_Projectile;
        [SerializeField] private Transform m_FiringPoint;
        [SerializeField] private float m_MinFiringCooldown;
        [SerializeField] private float m_MaxFiringCooldown;
        [SerializeField] private int m_Hp;

        private int m_CurrentHp;
        private float m_TempCooldown;
        private int m_CurrentWayPointIndex;
        private bool m_Active;
        private float m_CurMoveSpeed;
        private float m_SpeedMultiplier;
        //private SpawnManager m_SpawnManager;
        //private GameManager m_GameManager;
        //private AudioManager m_AudioManager;

        // Start is called before the first frame update
        void Start()
        {
            //m_SpawnManager = FindObjectOfType<SpawnManager>();
            //m_GameManager = FindObjectOfType<GameManager>();
            //m_AudioManager = FindObjectOfType<AudioManager>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!m_Active)
                return;

            int nextWayPoint = m_CurrentWayPointIndex + 1;
            if (nextWayPoint > m_WayPoints.Length - 1)
                nextWayPoint = 0;

            transform.position = Vector3.MoveTowards(transform.position, m_WayPoints[nextWayPoint].position, m_CurMoveSpeed * Time.deltaTime);
            if (transform.position == m_WayPoints[nextWayPoint].position)
                m_CurrentWayPointIndex = nextWayPoint;

            Vector3 direction = m_WayPoints[nextWayPoint].position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            if (m_TempCooldown <= 0)
            {
                Fire();
                m_TempCooldown = Random.Range(m_MinFiringCooldown, m_MaxFiringCooldown);
            }

            m_TempCooldown -= Time.deltaTime;
        }

        public void Init(Transform[] wayPoints, float speedMultiplier)
        {
            m_WayPoints = wayPoints;
            m_SpeedMultiplier = speedMultiplier;
            m_CurMoveSpeed = m_MoveSpeed * speedMultiplier;
            m_Active = true;
            transform.position = wayPoints[0].position;
            m_TempCooldown = Random.Range(m_MinFiringCooldown, m_MaxFiringCooldown) / speedMultiplier;
            m_CurrentHp = m_Hp;
        }

        private void Fire()
        {
            //ProjectileController projectile = Instantiate(m_Projectile, m_FiringPoint.position, Quaternion.identity, null);
            ProjectileController projectile = SpawnManager.Instance.SpawnEnemyProjectile(m_FiringPoint.position);
            projectile.Fire(m_SpeedMultiplier);

            AudioManager.Instance.PlayPlasmaSFX();
        }

        public void Hit(int damage)
        {
            m_CurrentHp -= damage;
            if (m_CurrentHp <= 0)
            {
                //Destroy(gameObject);

                SpawnManager.Instance.ReleaseEnemyController(this);

                SpawnManager.Instance.SpawnExplosionFX(transform.position);
                GameManager.Instance.AddScore(1);
                AudioManager.Instance.PlayExplosionSFX();
            }
            AudioManager.Instance.PlayHitSFX();
        }
    }
}