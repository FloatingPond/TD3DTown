using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Turrets.Data;
using _Project.Scripts.Gameplay.Turrets.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityServiceLocator;

namespace _Project.Scripts.Gameplay.Construction
{
    public class ConstructionPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ConstructionManager m_constructionManager;
        [SerializeField] private ConstructionPreview m_constructionPreview;
        [SerializeField] private List<Button> m_buttons;
        [SerializeField] private GameObject m_buttonPrefab;
        
        private TurretData m_currentSelection;
        public TurretData CurrentSelection => m_currentSelection;

        private void Start()
        {
            ServiceLocator.Global.Register(this);
        }

        public void SetupButtons(List<TurretData> availableTurrets)
        {
            foreach (TurretData data in availableTurrets)
            {
                GameObject newButton = Instantiate(m_buttonPrefab, transform);
                Button buttonComponent = newButton.GetComponent<Button>();
                buttonComponent.onClick.AddListener(EnableConstructionPreview);
                TurretButton turretButton = newButton.GetComponent<TurretButton>();
                turretButton.Setup(data);
                buttonComponent.onClick.AddListener(() => m_currentSelection = turretButton.Data);
            }
        }

        private void EnablePanel()
        {
            gameObject.SetActive(true);
        }

        private void DisablePanel()
        {
            gameObject.SetActive(false);
        }

        private void EnableConstructionPreview()
        {
            m_constructionPreview.gameObject.SetActive(true);
        }
        
        private void DisableConstructionPreview()
        {
            m_constructionPreview.gameObject.SetActive(false);
        }
    }
}