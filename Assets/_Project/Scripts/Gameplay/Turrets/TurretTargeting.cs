using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets
{
    public class TurretTargeting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TurretTargetingStrategy _targetingStrategy;
        [SerializeField] private Transform _objectToRotateTowardsTarget;
        
        private Transform _currentTarget;

        private void FixedUpdate()
        {
            if (_currentTarget == null)
            {
                _targetingStrategy.GetTarget();
            }
        
            Vector3 dir = transform.position - _currentTarget.position; // inverted (away from target)
            dir.y = 0f; // only yaw (Y rotation)

            if (dir.sqrMagnitude > 0.0001f)
                _objectToRotateTowardsTarget.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}