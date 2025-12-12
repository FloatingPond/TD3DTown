using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets
{
    public abstract class TurretTargetingStrategy : ScriptableObject
    {
        public abstract void GetTarget();
    }
}