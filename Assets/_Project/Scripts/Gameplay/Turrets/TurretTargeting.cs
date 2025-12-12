using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets
{
    public class TurretTargeting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TurretTargetingStrategy _targetingStrategy;
        [SerializeField] private Transform _objectToRotateTowardsTarget;
        
        private Transform _currentTarget;

        private void Start()
        {
            _targetingStrategy.Initialize(this);
        }

        private void FixedUpdate()
        {
            if (_currentTarget == null)
            {
                _currentTarget = _targetingStrategy.GetSingleTarget();
                return;
            }

            if (IsCurrentTargetInRange())
            {
                RotateObjectTowardTarget();
            }
        }

        private bool IsCurrentTargetInRange()
        {
            Vector3 dir = transform.position - _currentTarget.position;
            if (!(dir.magnitude > _targetingStrategy.Range))
                return true;

            _currentTarget = null;
            return false;

        }

        private void RotateObjectTowardTarget()
        {
            if (_objectToRotateTowardsTarget == null)
                return;

            Vector3 dir = transform.position - _currentTarget.position; // inverted (away from target)
            dir.y = 0f; // only yaw (Y rotation)

            if (dir.sqrMagnitude > 0.0001f)
                _objectToRotateTowardsTarget.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _targetingStrategy.Range);
        }
    }
}