using _Project.Scripts.Gameplay.Grid;
using Input;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Construction
{
    public class ConstructionPreview : MonoBehaviour
    {
        [SerializeField] private MeshRenderer m_meshRenderer;
        [SerializeField] private Color m_availableColor = Color.green;
        [SerializeField] private Color m_unavailableColor = Color.red;
        [SerializeField] private InputReader m_inputReader;
        [SerializeField] private UnityEngine.Camera m_camera;
        
        
        private void OnEnable()
        {
            //TODO: Snap the preview to grid tiles when hovering over them
            m_inputReader.Point += OnPoint;
        }

        private void OnDisable()
        {
            m_inputReader.Point -= OnPoint;
        }

        private void OnPoint(Vector2 point)
        {
            Ray ray = m_camera.ScreenPointToRay(point);

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Construction")))
                return;

            if (!hit.collider.TryGetComponent(out GridTile tile))
                return;

            transform.position = tile.transform.position;

            m_meshRenderer.material.color = !tile.IsOccupied() ? m_availableColor : m_unavailableColor;
        }
    }
}