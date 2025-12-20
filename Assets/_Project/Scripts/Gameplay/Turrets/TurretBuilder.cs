using System.Collections.Generic;
using _Project.Scripts.Gameplay.Turrets.Data;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Turrets
{
    public class TurretBuilder : MonoBehaviour
    {
        private static readonly int Opacity = Shader.PropertyToID("_Opacity");

        [Header("References")] 
        [SerializeField] private Collider m_collider;
        [SerializeField] private List<MeshRenderer> m_meshRenderers;
        
        private float m_progress = 1;
        private float m_timeToConstruct;
        
        public void BeginConstruction(TurretData turretData)
        {
            if (turretData.TimeToConstruct <= 0)
            {
                CompleteConstruction();
                return;
            }

            m_progress = 1f;
            m_timeToConstruct = turretData.TimeToConstruct;
            m_collider.enabled = false;
            enabled = true;
            foreach (MeshRenderer meshRenderer in m_meshRenderers)
            {
                meshRenderer.material.SetFloat(Opacity, m_progress);
            }
        }

        private void FixedUpdate()
        {
            if (m_timeToConstruct > 0)
            {
                float rate = 2f / m_timeToConstruct;
                m_progress -= rate * Time.deltaTime;
            }
            else
            {
                m_progress = -1f;
            }

            if (m_progress <= -1)
            {
                CompleteConstruction();
            }
            
            foreach (MeshRenderer meshRenderer in m_meshRenderers)
            {
                meshRenderer.material.SetFloat(Opacity, m_progress);
            }
        }

        private void CompleteConstruction()
        {
            foreach (MeshRenderer meshRenderer in m_meshRenderers)
            {
                meshRenderer.material.SetFloat(Opacity, -1);
            }
            m_collider.enabled = true;
            enabled = false;
        }
    }
}