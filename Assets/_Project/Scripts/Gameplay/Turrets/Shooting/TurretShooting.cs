using System;
using _Project.Scripts.Gameplay.Turrets.Targeting;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Shooting
{
    public class TurretShooting : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TurretTargeting _targeting;
        [SerializeField] private TurretShootingStrategy _shootingStrategy;
        public Transform ShootPoint;
        
        [Header("Settings")]
        [SerializeField] private float _cooldown;

        [Header("Debug")]
        public Transform Target;
        private float _lastShotTime;

        public Action<GameObject> OnHit;

        private void Start()
        {
            _shootingStrategy?.Initialize(this);
        }

        private void OnEnable()
        {
            _targeting.OnTargetCountChanged += OnTargetCountChanged;
        }

        private void OnDisable()
        {
            _targeting.OnTargetCountChanged -= OnTargetCountChanged;
        }

        private void OnTargetCountChanged()
        {
            Target = _targeting.Targets.Count > 0 ? _targeting.Targets[0] : null;
        }

        private void FixedUpdate()
        {
            if (Target)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            if (Time.time > _lastShotTime + _cooldown)
            {
                _shootingStrategy?.Shoot();
                _lastShotTime = Time.time;
            }
        }
    }
}