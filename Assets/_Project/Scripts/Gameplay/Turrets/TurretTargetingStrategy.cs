using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets
{
    public abstract class TurretTargetingStrategy : ScriptableObject
    {
        public float Range;

        public abstract void Initialize(TurretTargeting turretTargeting);
        
        public abstract Transform GetSingleTarget();
    }
}