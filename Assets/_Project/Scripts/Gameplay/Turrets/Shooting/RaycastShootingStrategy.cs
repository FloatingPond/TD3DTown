using _Project.Scripts.Gameplay.Effects;
using Gameplay.ObjectPool;
using UnityEngine;
using UnityServiceLocator;

namespace _Project.Scripts.Gameplay.Turrets.Shooting
{
    [CreateAssetMenu(fileName = "Raycast", menuName = "Turret Shooting Strategies/Raycast", order = 0)]
    public class RaycastShootingStrategy : TurretShootingStrategy
    {
        private TurretShooting _turret;
        private ObjectPool _objectPool;
        
        public GameObject BulletTrail;
    
        public override void Initialize(TurretShooting turretShooting)
        {
            _turret = turretShooting;
            _objectPool = ServiceLocator.Global.Get<ObjectPool>();
        }

        public override void Shoot()
        {
            if (_turret.ShootPoint == null || _turret.Target == null)
                return;

            Vector3 origin = _turret.ShootPoint.position;
            Vector3 toTarget = _turret.Target.position - origin;
            float distance = toTarget.magnitude;

            if (distance <= 0.001f)
                return;

            Vector3 dir = toTarget / distance;

            int entityMask = LayerMask.GetMask("Entity");
            if (!Physics.Raycast(origin, dir, out RaycastHit hit, distance, entityMask))
                return;

            if (hit.transform == _turret.Target)
            {
                GameObject bulletTrailObject = _objectPool.GetPooledObject(BulletTrail);
                bulletTrailObject.transform.position = _turret.ShootPoint.position;
                bulletTrailObject.TryGetComponent(out BulletTrailMovement bulletTrailMovement);
                bulletTrailMovement.SetTarget(_turret.Target.position, 100f);
            }
        }
    }
}
