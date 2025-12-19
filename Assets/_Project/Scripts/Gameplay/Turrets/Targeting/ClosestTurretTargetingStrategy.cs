using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Targeting
{
    [CreateAssetMenu(fileName = "Closest", menuName = "Turret/Targeting/Closest", order = 0)]
    public class ClosestTurretTargetingStrategy : TurretTargetingStrategy
    {
        public override List<Transform> SelectTarget(TurretTargeting targeting, List<Transform> potentialTargets)
        {
            potentialTargets.Sort((a, b) =>
                Vector3.Distance(targeting.transform.position, a.position).CompareTo(Vector3.Distance(targeting.transform.position, b.position)));
            return potentialTargets;
        }
    }
}