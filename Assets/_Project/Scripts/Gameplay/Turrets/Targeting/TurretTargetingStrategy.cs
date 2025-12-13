using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Targeting
{
    public abstract class TurretTargetingStrategy : ScriptableObject
    {
        [Min(1)]
        public int TargetCount = 1;
        public abstract void Initialize(TurretTargeting turretTargeting);
        /// <summary>
        /// Sort the list of potential targets to have the ideal target first.
        /// Each strategy determines the exact prioritization behaviour.
        /// </summary>
        /// <param name="potentialTargets"></param>
        /// <returns></returns>
        public abstract List<Transform> SelectTarget(List<Transform> potentialTargets);
    }
}