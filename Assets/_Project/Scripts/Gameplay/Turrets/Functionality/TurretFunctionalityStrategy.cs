using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Functionality
{
    public abstract class TurretFunctionalityStrategy : ScriptableObject
    {
        public abstract void ExecuteFunction();
        public abstract void ExecuteFunction(GameObject hitObject);
    }
}