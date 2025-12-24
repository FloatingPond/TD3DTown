using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityServiceLocator;

namespace _Project.Scripts.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelData m_levelData;
        [SerializeField] private Transform m_spawnPoint;
        [SerializeField] private Transform m_endPoint;
        
        private ObjectPool m_objectPool;
        private int m_currentWave;
        private float m_timeToNextWave;
        
        [Header("Settings")]
        [SerializeField] private float m_spawnCooldown = 1f;
        
        private void Start()
        {
            m_objectPool = ServiceLocator.Global.Get<ObjectPool>();
        }
        
        public void BeginLevel()
        {
            StartCoroutine(SpawnWave());
        }

        private IEnumerator SpawnWave()
        {
            foreach (GameObject entity in m_levelData.Waves[m_currentWave].Entities)
            {
                yield return new WaitForSeconds(m_spawnCooldown);
                
                GameObject spawnedEntity = m_objectPool.GetPooledObject(entity);
                if (spawnedEntity.TryGetComponent(out NavMeshAgent navMeshAgent))
                {
                    navMeshAgent.SetDestination(m_endPoint.position);
                }
            }
            
            m_currentWave++;
            yield return null;
        }
    }
}