using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Targeting
{
    [CreateAssetMenu(fileName = "Furthest", menuName = "Turret/Targeting/Furthest", order = 0)]
    public class FurthestTurretTargetingStrategy : TurretTargetingStrategy
    {
        public override List<Transform> SelectTarget(TurretTargeting targeting, List<Transform> potentialTargets)
        {
            potentialTargets.Sort((a, b) =>
                Vector3.Distance(targeting.transform.position, b.position).CompareTo(Vector3.Distance(targeting.transform.position, a.position)));
            return potentialTargets;
        }
    }
}