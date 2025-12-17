using _Project.Scripts.Gameplay.Turrets.Shooting;
using _Project.Scripts.Gameplay.Turrets.Targeting;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Functionality
{
    public class TurretFunctionality : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TurretFunctionalityStrategy m_functionalityStrategy;
        [SerializeField] private TurretTargeting m_targeting;
        [SerializeField] private TurretShooting m_shooting;

        private void OnEnable()
        {
            m_shooting.OnHit += OnHit;
        }

        private void OnDisable()
        {
            m_shooting.OnHit -= OnHit;
        }

        public void Start()
        {
            m_functionalityStrategy?.Initialize(this);
        }
        
        private void OnHit(GameObject hitObject)
        {
            m_functionalityStrategy?.ExecuteFunction(hitObject);
        }
    }
}