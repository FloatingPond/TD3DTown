using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Grid
{
    public class GridTile : MonoBehaviour
    {
        public bool Occupied = false;
        
        public event Action<GridTile> OnClicked;

        public void Clicked()
        {
            OnClicked?.Invoke(this);
            if (Occupied)
            {
                //TODO: Show Build Menu
            }
        }
    }
}