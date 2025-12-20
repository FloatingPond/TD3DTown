using System.Collections.Generic;
using _Project.Scripts.Gameplay.Turrets;
using _Project.Scripts.Gameplay.Turrets.Data;
using _Project.Scripts.Gameplay.Turrets.Functionality;
using _Project.Scripts.Gameplay.Turrets.Shooting;
using _Project.Scripts.Gameplay.Turrets.Targeting;
using UnityEngine;
using UnityServiceLocator;

namespace _Project.Scripts.Gameplay.Construction
{
    public class ConstructionManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ConstructionPanel m_constructionPanel;
        [SerializeField] private List<TurretData> m_availableTurrets;

        private void Start()
        {
            ServiceLocator.Global.Register(this);
            m_constructionPanel.SetupButtons(m_availableTurrets);
        }

        public void BuildTurret(TurretData data, Vector3 worldPosition)
        {
            //TODO: If (not enough resources) return false
            
            GameObject turret = Instantiate(data.ModelPrefab, worldPosition, Quaternion.identity);

            if (turret.TryGetComponent(out TurretBuilder turretBuilder))
            {
                turretBuilder.BeginConstruction(data);
            }

            if (turret.TryGetComponent(out TurretTargeting turretTargeting))
            {
                turretTargeting.SetTurretData(data);
                turretTargeting.ChangeStrategy(data.TargetingStrategy);
            }

            if (turret.TryGetComponent(out TurretShooting turretShooting))
            {
                turretShooting.ChangeStrategy(data.ShootingStrategy);
            }

            if (turret.TryGetComponent(out TurretFunctionality turretFunctionality))
            {
                turretFunctionality.ChangeStrategy(data.FunctionalityStrategy);
            }
        }
    }
}