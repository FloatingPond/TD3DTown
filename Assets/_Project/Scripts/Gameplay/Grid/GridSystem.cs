using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Grid
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private int m_length;
        [SerializeField] private int m_width;
        [SerializeField] private int m_spacing;
        
        [SerializeField] private GridTile m_gridTilePrefab;
        [SerializeField] private List<GridTile> m_tiles;
        [SerializeField] private GridTile m_currentSelected;

        private void Start()
        {
            for(int x = 0; x < m_length; x++)
            {
                for(int y = 0; y < m_width; y++)
                {
                    GameObject gridTile = Instantiate(m_gridTilePrefab.gameObject, transform, true);
                    gridTile.transform.position = new Vector3(x, 0, y) * m_spacing;
                }
            }
        }

        public bool IsCurrentSelectedTileOccupied()
        {
            return m_currentSelected.IsOccupied();
        }
    }
}