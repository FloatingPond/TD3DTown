using _Project.Scripts.Gameplay.Entities;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets.Functionality
{
    [CreateAssetMenu(fileName = "Damage", menuName = "Turret/Functionality/Damage", order = 0)]
    public class TurretFunctionDamageStrategy : TurretFunctionalityStrategy
    {
        public int Damage;
        private TurretFunctionality _turretFunctionality;
        public override void Initialize(TurretFunctionality turretFunctionality)
        {
            _turretFunctionality = turretFunctionality;
        }

        public override void ExecuteFunction()
        {
            
        }

        public override void ExecuteFunction(GameObject hitObject)
        {
            if (hitObject.TryGetComponent(out EntityHealth entityHealth))
            {
                entityHealth.TakeDamage(Damage);
            }
        }
    }
}