using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Grid
{
    public class GridTile : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private LayerMask m_layersToCheck;
        [SerializeField] private float m_occupiedCheckRadius = 2.5f;
        
        public event Action<GridTile> OnClicked;
        
        public bool IsOccupied()
        {
            return Physics.CheckSphere(transform.position, m_occupiedCheckRadius, m_layersToCheck, QueryTriggerInteraction.Collide);
        }

        public void Clicked()
        {
            OnClicked?.Invoke(this);
            if (IsOccupied())
            {
                //TODO: Show Build Menu
            }
        }
    }
}