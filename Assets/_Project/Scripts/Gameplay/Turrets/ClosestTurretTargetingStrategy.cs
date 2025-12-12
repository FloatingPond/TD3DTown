using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets
{
    [CreateAssetMenu(fileName = "Closest", menuName = "Turret Targeting Strategies/Closest", order = 0)]
    public class ClosestTurretTargetingStrategy : TurretTargetingStrategy
    {
        private TurretTargeting _turret;
        public override void Initialize(TurretTargeting turretTargeting)
        {
            _turret = turretTargeting;
        }

        public override Transform GetSingleTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(_turret.transform.position, Range);

            Vector3 closestDistance = Vector3.zero;
            Transform closestTransform = null;
            
            foreach (Collider collider in colliders)
            {
                Vector3 candidateDistance = _turret.transform.position - collider.transform.position;
                
                if (!(candidateDistance.magnitude > closestDistance.magnitude))
                    continue;

                closestDistance = candidateDistance;
                closestTransform = collider.transform;
            }

            return closestTransform;
        }
    }
}
