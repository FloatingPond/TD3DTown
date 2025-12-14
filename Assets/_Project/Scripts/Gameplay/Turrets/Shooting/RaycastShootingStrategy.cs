using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Shooting
{
    [CreateAssetMenu(fileName = "Raycast", menuName = "Turret Shooting Strategies/Raycast", order = 0)]
    public class RaycastShootingStrategy : TurretShootingStrategy
    {
        private TurretShooting _turret;
    
        public override void Initialize(TurretShooting turretShooting)
        {
            _turret = turretShooting;
        }

        public override void Shoot()
        {
            Debug.Log("Shoot!");
        }
    }
}
