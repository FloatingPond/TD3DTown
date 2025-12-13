using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Targeting
{
    [CreateAssetMenu(fileName = "Furthest", menuName = "Turret Targeting Strategies/Furthest", order = 0)]
    public class FurthestTurretTargetingStrategy : TurretTargetingStrategy
    {
        private TurretTargeting _turret;
        public override void Initialize(TurretTargeting turretTargeting)
        {
            _turret = turretTargeting;
        }

        public override List<Transform> SelectTarget(List<Transform> potentialTargets)
        {
            potentialTargets.Sort((a, b) =>
                Vector3.Distance(_turret.transform.position, b.position).CompareTo(Vector3.Distance(_turret.transform.position, a.position)));
            return potentialTargets;
        }
    }
}