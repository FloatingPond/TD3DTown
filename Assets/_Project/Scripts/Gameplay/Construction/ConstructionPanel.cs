using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.Construction
{
    public class ConstructionPanel : MonoBehaviour
    {
        [SerializeField] private List<Button> m_buttons;
        [SerializeField] private ConstructionPreview m_constructionPreview;
        
        private void Awake()
        {
            foreach (Button button in m_buttons)
            {
                button.onClick.AddListener(EnableConstructionPreview);
                button.onClick.AddListener(DisablePanel);
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