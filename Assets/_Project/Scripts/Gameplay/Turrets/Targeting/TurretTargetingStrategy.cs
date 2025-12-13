using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Targeting
{
    public abstract class TurretTargetingStrategy : ScriptableObject
    {
        [Min(1)]
        public int TargetCount = 1;
        public abstract void Initialize(TurretTargeting turretTargeting);
        public abstract List<Transform> GetTargets(List<Transform> potentialTargets);
    }
}