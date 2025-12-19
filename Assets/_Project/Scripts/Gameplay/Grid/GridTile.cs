using System;
using _Project.Scripts.Gameplay.Construction;
using UnityEngine;
using UnityServiceLocator;

namespace _Project.Scripts.Gameplay.Grid
{
    public class GridTile : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private LayerMask m_layersToCheck;
        [SerializeField] private float m_occupiedCheckRadius = 2.5f;
        
        public event Action<GridTile> OnClicked;

        private ConstructionManager m_constructionManager;
        private ConstructionPanel m_constructionPanel;
        
        private void Start()
        {
            m_constructionManager = ServiceLocator.Global.Get<ConstructionManager>();
            m_constructionPanel = ServiceLocator.Global.Get<ConstructionPanel>();
        }

        public bool IsOccupied()
        {
            return Physics.CheckSphere(transform.position, m_occupiedCheckRadius, m_layersToCheck, QueryTriggerInteraction.Collide);
        }

        public void Clicked()
        {
            Debug.Log("Clicked " + transform.position);
            if (m_constructionManager == null)
            {
                m_constructionManager = ServiceLocator.Global.Get<ConstructionManager>();
            }

            if (m_constructionPanel == null)
            {
                m_constructionPanel = ServiceLocator.Global.Get<ConstructionPanel>();
            }

            OnClicked?.Invoke(this);
            if (!IsOccupied())
            {
                m_constructionManager.BuildTurret(m_constructionPanel.CurrentSelection, transform.position);
            }
        }
    }
}