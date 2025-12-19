using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Targeting
{
    public class TurretTargeting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TurretTargetingStrategy m_targetingStrategy;
        [SerializeField] private Transform m_objectToRotateTowardsTarget;
        [SerializeField] private SphereCollider m_collider;
        
        [Header("Settings")]
        [SerializeField] private bool m_limitToYRotation;
        [SerializeField] private float m_rotationSpeed = 10f;

        [Header("Debug")]
        [SerializeField] private List<Transform> m_targets;
        public List<Transform> Targets => m_targets;
        public Action OnTargetCountChanged;

        public void ChangeStrategy(TurretTargetingStrategy newStrategy)
        {
            m_targetingStrategy = newStrategy;
        }

        public void SetTargetingRadius(float radius)
        {
            m_collider.radius = radius;
        }

        private void FixedUpdate()
        {
            if (m_targets.Count > 0 && (m_targets[0] == null || !m_targets[0].gameObject.activeInHierarchy))
            {
                m_targets.RemoveAt(0);
                OnTargetCountChanged?.Invoke();
                
                if (m_targets.Count > 0)
                {
                    m_targetingStrategy?.SelectTarget(this, m_targets);
                }
            }
            
            if (m_targets.Count > 0)
            {
                RotateObjectTowardTarget();
            }
        }

        private void RotateObjectTowardTarget()
        {
            if (m_objectToRotateTowardsTarget == null)
                return;

            Vector3 dir = m_objectToRotateTowardsTarget.position - m_targets[0].position;

            if (dir.sqrMagnitude < 0.0001f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(dir, Vector3.up);

            float t = m_rotationSpeed * Time.fixedDeltaTime;
            m_objectToRotateTowardsTarget.rotation =
                Quaternion.Slerp(m_objectToRotateTowardsTarget.rotation, targetRotation, t);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Entity"))
                return;

            if (!m_targets.Contains(other.transform))
            {
                m_targets.Add(other.transform);
            }
            
            OnTargetCountChanged?.Invoke();
            m_targetingStrategy?.SelectTarget(this , m_targets);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Entity"))
                return;
            
            if (m_targets.Contains(other.transform))
            {
                m_targets.Remove(other.transform);
            }

            if (m_targets.Count > 0)
            {
                OnTargetCountChanged?.Invoke();
                m_targetingStrategy?.SelectTarget(this, m_targets);
                return;
            }
            
            OnTargetCountChanged?.Invoke();
        }
    }
}