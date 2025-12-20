using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Turrets.Data;
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
        [SerializeField] private float m_minShootingAngle;
        
        [Header("Debug")]
        [SerializeField] private List<Transform> m_targets;
        public List<Transform> Targets => m_targets;
        public Action OnTargetCountChanged;
        public bool CanShoot;

        public void ChangeStrategy(TurretTargetingStrategy newStrategy)
        {
            m_targetingStrategy = newStrategy;
        }

        public void SetTurretData(TurretData data)
        {
            m_collider.radius = data.TargetingRadius;
            m_minShootingAngle = data.TargetingMinShootingAngle;
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

        /// <summary>
        /// Angle calculations are backwards due to model pivots.
        /// TODO: Ideally in the future we can fix the models and have the calculations the correct way round. 
        /// </summary>
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
            
            Vector3 directionToTarget = m_objectToRotateTowardsTarget.position - m_targets[0].position;
            float angle = Vector3.Angle(m_objectToRotateTowardsTarget.forward, directionToTarget);
            
            CanShoot = angle < m_minShootingAngle;
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