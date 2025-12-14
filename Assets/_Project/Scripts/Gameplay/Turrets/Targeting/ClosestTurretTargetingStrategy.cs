using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Targeting
{
    [CreateAssetMenu(fileName = "Closest", menuName = "Turret Targeting Strategies/Closest", order = 0)]
    public class ClosestTurretTargetingStrategy : TurretTargetingStrategy
    {
        private TurretTargeting _turret;
        public override void Initialize(TurretTargeting turretTargeting)
        {
            _turret = turretTargeting;
        }

        public override List<Transform> SelectTarget(List<Transform> potentialTargets)
        {
            potentialTargets.Sort((a, b) =>
                Vector3.Distance(_turret.transform.position, a.position).CompareTo(Vector3.Distance(_turret.transform.position, b.position)));
            return potentialTargets;
        }
    }
}