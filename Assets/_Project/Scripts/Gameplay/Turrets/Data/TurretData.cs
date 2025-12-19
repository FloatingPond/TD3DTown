using _Project.Scripts.Gameplay.Turrets.Functionality;
using _Project.Scripts.Gameplay.Turrets.Shooting;
using _Project.Scripts.Gameplay.Turrets.Targeting;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Data
{
    [CreateAssetMenu(fileName = "TurretData", menuName = "Turret/TurretData", order = 0)]
    public class TurretData : ScriptableObject
    {
        public TurretTargetingStrategy TargetingStrategy;
        public TurretShootingStrategy ShootingStrategy;
        public TurretFunctionalityStrategy FunctionalityStrategy;
        public float TargetingRadius;
        public GameObject ModelPrefab;
    }
}