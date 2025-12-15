using _Project.Scripts.Gameplay.Turrets.Shooting;
using _Project.Scripts.Gameplay.Turrets.Targeting;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Functionality
{
    public class TurretFunctionality : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TurretFunctionalityStrategy _functionalityStrategy;
        [SerializeField] private TurretTargeting _targeting;
        [SerializeField] private TurretShooting _shooting;

        private void OnEnable()
        {
            _shooting.OnHit += OnHit;
        }

        private void OnDisable()
        {
            _shooting.OnHit -= OnHit;
        }

        public void Start()
        {
            _functionalityStrategy?.Initialize(this);
        }
        
        private void OnHit(GameObject hitObject)
        {
            _functionalityStrategy?.ExecuteFunction(hitObject);
        }
    }
}