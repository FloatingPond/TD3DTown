using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Shooting
{
    public abstract class TurretShootingStrategy : ScriptableObject
    {
        public abstract void Shoot(TurretShooting turretShooting);
    }
}