using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Gameplay.Entities
{
    public class EntityNavigation : MonoBehaviour
    {
        [SerializeField] private Transform m_target;
        [SerializeField] private NavMeshAgent m_agent;
    
        void Start()
        {
            m_agent.SetDestination(m_target.position);
        }
    }
}
