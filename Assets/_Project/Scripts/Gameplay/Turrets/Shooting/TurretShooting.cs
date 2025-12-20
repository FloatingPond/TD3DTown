using System;
using _Project.Scripts.Gameplay.Turrets.Targeting;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Shooting
{
    public class TurretShooting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TurretTargeting m_targeting;
        [SerializeField] private TurretShootingStrategy m_shootingStrategy;
        public Transform ShootPoint;
        
        [Header("Settings")]
        [SerializeField] private float m_cooldown;

        [Header("Debug")]
        public Transform Target;
        private float m_lastShotTime;

        public Action<GameObject> OnHit;

        private void OnEnable()
        {
            m_targeting.OnTargetCountChanged += OnTargetCountChanged;
        }

        private void OnDisable()
        {
            m_targeting.OnTargetCountChanged -= OnTargetCountChanged;
        }

        public void ChangeStrategy(TurretShootingStrategy newStrategy)
        {
            m_shootingStrategy = newStrategy;
        }

        private void OnTargetCountChanged()
        {
            Target = m_targeting.Targets.Count > 0 ? m_targeting.Targets[0] : null;
        }

        private void FixedUpdate()
        {
            if (Target && m_targeting.CanShoot)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            if (Time.time > m_lastShotTime + m_cooldown)
            {
                m_shootingStrategy?.Shoot(this);
                m_lastShotTime = Time.time;
            }
        }
    }
}