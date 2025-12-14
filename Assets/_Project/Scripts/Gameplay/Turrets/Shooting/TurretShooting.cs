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
        
        [Header("Settings")]
        [SerializeField] private float _cooldown;

        [Header("Debug")]
        [SerializeField] private bool _hasTarget;
        [SerializeField] private float _lastShotTime;

        private void Start()
        {
            _shootingStrategy?.Initialize(this);
        }

        private void OnEnable()
        {
            _targeting.OnTargetCountChanged += b => _hasTarget = b ;
        }

        private void OnDisable()
        {
            _targeting.OnTargetCountChanged -= b => _hasTarget = b ;
        }

        private void FixedUpdate()
        {
            if (_hasTarget)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            if (Time.time >= _lastShotTime + _cooldown)
            {
                _shootingStrategy?.Shoot();
                _lastShotTime = Time.time;
            }
        }
    }
}
