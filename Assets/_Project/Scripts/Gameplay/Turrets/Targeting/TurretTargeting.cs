using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Targeting
{
    public class TurretTargeting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TurretTargetingStrategy _targetingStrategy;
        [SerializeField] private Transform _objectToRotateTowardsTarget;
        [SerializeField] private SphereCollider _collider;
        public SphereCollider Collider => _collider;
        
        [Header("Settings")]
        [SerializeField] private bool _limitToYRotation;
        [SerializeField] private float _rotationSpeed = 10f;

        [Header("Debug")]
        [SerializeField] private List<Transform> _currentTargets;

        private void Start()
        {
            _targetingStrategy?.Initialize(this);
        }

        private void FixedUpdate()
        {
            if (_currentTargets.Count > 0)
            {
                RotateObjectTowardTarget();
            }
        }

        private void RotateObjectTowardTarget()
        {
            if (_objectToRotateTowardsTarget == null)
                return;

            Vector3 dir = transform.position - _currentTargets[0].position; // inverted (away from target)

            if (_limitToYRotation)
            {
                dir.y = 0f; // only yaw (Y rotation)
            }

            if (!(dir.sqrMagnitude > 0.0001f))
                return;

            Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);
                
            float t = _rotationSpeed * Time.fixedDeltaTime;
            _objectToRotateTowardsTarget.rotation =
                Quaternion.Slerp(_objectToRotateTowardsTarget.rotation, targetRotation, t);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Entity"))
                return;

            if (!_currentTargets.Contains(other.transform))
            {
                _currentTargets.Add(other.transform);
            }

            _targetingStrategy?.GetTargets(_currentTargets);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Entity"))
                return;
            
            if (_currentTargets.Contains(other.transform))
            {
                _currentTargets.Remove(other.transform);
            }
            
            _targetingStrategy?.GetTargets(_currentTargets);
        }
    }
}