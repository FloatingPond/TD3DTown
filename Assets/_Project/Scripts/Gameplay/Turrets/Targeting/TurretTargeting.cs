using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Targeting
{
    public class TurretTargeting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TurretTargetingStrategy _targetingStrategy;
        [SerializeField] private Transform _objectToRotateTowardsTarget;
        
        [Header("Settings")]
        [SerializeField] private bool _limitToYRotation;
        [SerializeField] private float _rotationSpeed = 10f;

        [Header("Debug")]
        [SerializeField] private List<Transform> _targets;
        public List<Transform> Targets => _targets;
        public Action OnTargetCountChanged;

        private void Start()
        {
            _targetingStrategy?.Initialize(this);
        }

        private void FixedUpdate()
        {
            if (_targets.Count > 0 && (_targets[0] == null || !_targets[0].gameObject.activeInHierarchy))
            {
                _targets.RemoveAt(0);
                OnTargetCountChanged?.Invoke();
                
                if (_targets.Count > 0)
                {
                    _targetingStrategy?.SelectTarget(_targets);
                }
            }
            
            if (_targets.Count > 0)
            {
                RotateObjectTowardTarget();
            }
        }

        private void RotateObjectTowardTarget()
        {
            if (_objectToRotateTowardsTarget == null)
                return;

            Vector3 dir = _objectToRotateTowardsTarget.position - _targets[0].position;

            if (dir.sqrMagnitude < 0.0001f)
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

            if (!_targets.Contains(other.transform))
            {
                _targets.Add(other.transform);
            }
            
            OnTargetCountChanged?.Invoke();
            _targetingStrategy?.SelectTarget(_targets);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Entity"))
                return;
            
            if (_targets.Contains(other.transform))
            {
                _targets.Remove(other.transform);
            }

            if (_targets.Count > 0)
            {
                OnTargetCountChanged?.Invoke();
                _targetingStrategy?.SelectTarget(_targets);
                return;
            }
            
            OnTargetCountChanged?.Invoke();
        }
    }
}