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

        [Header("Raycast Settings")]
        [SerializeField] private float _range = 100f;
        [SerializeField] private LayerMask _hitMask = ~0; // default: everything

        // TODO(piercing): Add settings for piercing (enabled, maxPierceCount, whether world geometry stops the shot,
        // TODO(piercing): damage falloff per pierce, and/or penetration depth rules).

        public override void Initialize(TurretShooting turretShooting)
        {
            _turret = turretShooting;
            _objectPool = ServiceLocator.Global.Get<ObjectPool>();
        }

        public override void Shoot()
        {
            if (_turret.ShootPoint == null)
                return;

            Vector3 origin = _turret.ShootPoint.position;
            Vector3 dir = _turret.ShootPoint.forward;

            Vector3 trailTargetPoint = origin + dir * _range;

            // TODO(piercing): Replace single-hit raycast with RaycastNonAlloc / RaycastAll.
            // - Collect all hits along the ray, sort by distance.
            // - Iterate through hits applying damage to each valid Entity until max pierce reached.
            // - Decide if/when the ray stops (e.g., stop on World layer, or after first non-trigger collider).
            // - Pick trailTargetPoint as:
            //     * the first blocking surface hit (non-pierceable), OR
            //     * the furthest hit point if only pierceables were hit, OR
            //     * max range point if nothing hit.

            bool hasHit = Physics.Raycast(
                origin,
                dir,
                out RaycastHit hit,
                _range,
                _hitMask,
                QueryTriggerInteraction.Ignore
            );

            if (hasHit)
            {
                trailTargetPoint = hit.point;

                // TODO(piercing): When piercing, you may need to spawn multiple trail segments (origin->hit1, hit1->hit2, ...),
                // TODO(piercing): or adjust your trail VFX to accept a polyline instead of a single end point.
            }

            GameObject bulletTrailObject = _objectPool.GetPooledObject(BulletTrail);
            bulletTrailObject.transform.position = origin;
            bulletTrailObject.TryGetComponent(out BulletTrailMovement bulletTrailMovement);
            bulletTrailMovement.SetTarget(trailTargetPoint, 100f);

            // TODO(piercing): Move "damage / hit reaction" logic into a dedicated method/service so
            // TODO(piercing): the non-pierce and pierce paths can share it (ApplyHit(hit)).
            if (!hasHit || _turret.Target == null)
                return;

            if (hit.transform == _turret.Target)
            {
                // TODO(piercing): Apply damage/effects here. For piercing, this would happen inside the hit-iteration loop.
            }
        }
    }
}
