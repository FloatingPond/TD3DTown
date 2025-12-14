using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Shooting
{
    public abstract class TurretShootingStrategy : ScriptableObject
    {
        public abstract void Initialize(TurretShooting turretShooting);
        public abstract void Shoot();
    }
}