using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Section3
{
    [System.Serializable]
    public class EnemiesPool
    {
        public EnemyController prefab;
        public List<EnemyController> activeObjects;
        public List<EnemyController> inactiveObjects;
        public EnemyController Spawn(Vector3 pos, Transform parent)
        {
            if (inactiveObjects.Count == 0)
            {
                EnemyController obj = GameObject.Instantiate(prefab, parent);
                obj.transform.position = pos;
                activeObjects.Add(obj);
                return obj;
            }
            else
            {
                EnemyController obj = inactiveObjects[0];
                obj.gameObject.SetActive(true);
                obj.transform.SetParent(parent);
                obj.transform.position = pos;
                inactiveObjects.RemoveAt(0);
                activeObjects.Add(obj);
                return obj;
            }
        }
        public void Release(EnemyController obj)
        {
            if (activeObjects.Contains(obj))
            {
                activeObjects.Remove(obj);
                inactiveObjects.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }
        public void Clear()
        {
            while (activeObjects.Count > 0)
            {
                EnemyController obj = activeObjects[0];
                obj.gameObject.SetActive(false);
                activeObjects.RemoveAt(0);
                inactiveObjects.Add(obj);
            }
        }
    }

    [System.Serializable]
    public class ProjectilesPool
    {
        public ProjectileController prefab;
        public List<ProjectileController> activeObjects;
        public List<ProjectileController> inactiveObjects;
        public ProjectileController Spawn(Vector3 pos, Transform parent)
        {
            if (inactiveObjects.Count == 0)
            {
                ProjectileController obj = GameObject.Instantiate(prefab, parent);
                obj.transform.position = pos;
                activeObjects.Add(obj);
                return obj;
            }
            else
            {
                ProjectileController obj = inactiveObjects[0];
                obj.gameObject.SetActive(true);
                obj.transform.SetParent(parent);
                obj.transform.position = pos;
                inactiveObjects.RemoveAt(0);
                activeObjects.Add(obj);
                return obj;
            }
        }
        public void Release(ProjectileController obj)
        {
            if (activeObjects.Contains(obj))
            {
                activeObjects.Remove(obj);
                inactiveObjects.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }
        public void Clear()
        {
            while (activeObjects.Count > 0)
            {
                ProjectileController obj = activeObjects[0];
                obj.gameObject.SetActive(false);
                activeObjects.RemoveAt(0);
                inactiveObjects.Add(obj);
            }
        }
    }

    [System.Serializable]
    public class ParticleFXsPool
    {
        public ParticleFX prefab;
        public List<ParticleFX> activeObjects;
        public List<ParticleFX> inactiveObjects;
        public ParticleFX Spawn(Vector3 pos, Transform parent)
        {
            if (inactiveObjects.Count == 0)
            {
                ParticleFX obj = GameObject.Instantiate(prefab, parent);
                obj.transform.position = pos;
                activeObjects.Add(obj);
                return obj;
            }
            else
            {
                ParticleFX obj = inactiveObjects[0];
                obj.gameObject.SetActive(true);
                obj.transform.SetParent(parent);
                obj.transform.position = pos;
                inactiveObjects.RemoveAt(0);
                activeObjects.Add(obj);
                return obj;
            }
        }
        public void Release(ParticleFX obj)
        {
            if (activeObjects.Contains(obj))
            {
                activeObjects.Remove(obj);
                inactiveObjects.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }
        public void Clear()
        {
            while (activeObjects.Count > 0)
            {
                ParticleFX obj = activeObjects[0];
                obj.gameObject.SetActive(false);
                activeObjects.RemoveAt(0);
                inactiveObjects.Add(obj);
            }
        }
    }

    public class SpawnManager : MonoBehaviour
    {
        private static SpawnManager m_Instance;
        public static SpawnManager Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = FindObjectOfType<SpawnManager>();
                return m_Instance;
            }
        }

        [SerializeField] private bool m_Active;
        //[SerializeField] private EnemyController m_EnemyPrefab;
        [SerializeField] private EnemiesPool m_EnemiesPool;
        [SerializeField] private int m_MinTotalEnemies;
        [SerializeField] private int m_MaxTotalEnemies;
        [SerializeField] private float m_EnemySpawnInterval;
        [SerializeField] private EnemyPath[] m_Paths;
        [SerializeField] private int m_TotalGroups;
        [SerializeField] private ProjectilesPool m_EnemyProjectilesPool;
        [SerializeField] private ProjectilesPool m_PlayerProjectilesPool;
        [SerializeField] private ParticleFXsPool m_HitFXsPool;
        [SerializeField] private ParticleFXsPool m_ShootingFXsPool;
        [SerializeField] private ParticleFXsPool m_ExplosionFXsPool;
        [SerializeField] private PlayerController m_PlayerControllerPrefab;

        public PlayerController Player => m_Player;

        private bool m_IsSpawningEnemies;
        private PlayerController m_Player;
        private WaveData m_CurWave;

        private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
            else if (m_Instance != this)
                Destroy(gameObject);
        }

        public void StartBattle(WaveData wave, bool resetPosition)
        {
            m_CurWave = wave;
            m_MinTotalEnemies = m_CurWave.minTotalEnemies;
            m_MaxTotalEnemies = m_CurWave.maxTotalEnemies;
            m_TotalGroups = m_CurWave.totalGroups;
            if (m_Player == null)
                m_Player = Instantiate(m_PlayerControllerPrefab);
            if (resetPosition)
                m_Player.transform.position = Vector3.zero;
            //StartCoroutine(IETestCoroutine());
            StartCoroutine(IESpawnGroups(m_TotalGroups));
        }

        private IEnumerator IESpawnGroups(int pGroups)
        {
            m_IsSpawningEnemies = true;

            for (int i = 0; i < pGroups; i++)
            {
                int totalEnemies = Random.Range(m_MinTotalEnemies, m_MaxTotalEnemies);
                EnemyPath path = m_Paths[Random.Range(0, m_Paths.Length)];
                yield return StartCoroutine(IESpawnEnemies(totalEnemies, path));
                if (i < pGroups - 1)
                    yield return new WaitForSeconds(3f / m_CurWave.speedMultiplier);
            }

            m_IsSpawningEnemies = false;
        }

        private IEnumerator IESpawnEnemies(int totalEnemies, EnemyPath path)
        {

            for (int i = 0; i < totalEnemies; i++)
            {
                yield return new WaitUntil(() => m_Active);
                yield return new WaitForSeconds(m_EnemySpawnInterval / m_CurWave.speedMultiplier);

                //EnemyController enemy = Instantiate(m_EnemyPrefab, transform);
                EnemyController enemy = m_EnemiesPool.Spawn(path.WayPoints[0].position, transform);
                enemy.Init(path.WayPoints, m_CurWave.speedMultiplier);
            }
        }

        public void ReleaseEnemyController(EnemyController enemy)
        {
            m_EnemiesPool.Release(enemy);
        }

        public ProjectileController SpawnEnemyProjectile(Vector3 position)
        {
            ProjectileController obj = m_EnemyProjectilesPool.Spawn(position, transform);
            obj.SetFromPlayer(false);
            return obj;
        }

        public ProjectileController SpawnPlayerProjectile(Vector3 position)
        {
            ProjectileController obj = m_PlayerProjectilesPool.Spawn(position, transform);
            obj.SetFromPlayer(true);
            return obj;
        }

        public void ReleaseEnemyProjectile(ProjectileController projectile)
        {
            m_EnemyProjectilesPool.Release(projectile);
        }

        public void ReleasePlayerProjectile(ProjectileController projectile)
        {
            m_PlayerProjectilesPool.Release(projectile);
        }

        public ParticleFX SpawnHitFX(Vector3 position)
        {
            ParticleFX fx = m_HitFXsPool.Spawn(position, transform);
            fx.SetPool(m_HitFXsPool);
            return fx;
        }

        public void ReleaseHitFX(ParticleFX obj)
        {
            m_HitFXsPool.Release(obj);
        }

        public ParticleFX SpawnShootingFX(Vector3 position)
        {
            ParticleFX fx = m_ShootingFXsPool.Spawn(position, transform);
            fx.SetPool(m_ShootingFXsPool);
            return fx;
        }

        public void ReleaseShootingFX(ParticleFX obj)
        {
            m_ShootingFXsPool.Release(obj);
        }

        public ParticleFX SpawnExplosionFX(Vector3 position)
        {
            ParticleFX fx = m_ExplosionFXsPool.Spawn(position, transform);
            fx.SetPool(m_ExplosionFXsPool);
            return fx;
        }

        public bool IsClear()
        {
            if (m_IsSpawningEnemies || m_EnemiesPool.activeObjects.Count > 0)
                return false;
            return true;
        }

        public void Clear()
        {
            m_EnemiesPool.Clear();
            m_EnemyProjectilesPool.Clear();
            m_ExplosionFXsPool.Clear();
            m_HitFXsPool.Clear();
            m_PlayerProjectilesPool.Clear();
            m_ShootingFXsPool.Clear();
            StopAllCoroutines();
        }

        //private IEnumerator IETestCoroutine()
        //{
        //    yield return new WaitUntil(() => m_Active);
        //    for (int i = 0; i < 5; i++)
        //    {
        //        Debug.Log(i);
        //        yield return new WaitForSeconds(1f);
        //    }
        //}
    }
}