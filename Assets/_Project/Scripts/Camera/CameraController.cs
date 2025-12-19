using System.Collections;
using _Project.Scripts.Gameplay.Grid;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private UnityEngine.Camera m_camera;
        [SerializeField] private InputReader m_inputReader;
        
        [Header("Settings")] 
        [SerializeField] private float m_translateSpeed = 1f;
        [SerializeField] private float m_rotateSpeed = 5f;
        
        [Header("Zoom Settings")] 
        [SerializeField] private float m_zoomSpeed = 1f;
        [SerializeField] private float m_maxZoomIn;
        [SerializeField] private float m_maxZoomOut;
        [SerializeField] private float m_defaultZoom;
        
        private bool m_isRotating = false;

        private void OnEnable()
        {
            m_inputReader.EnablePlayerActions();
            m_inputReader.Rotate += Rotate;
            m_inputReader.Click += OnClick;
        }

        private void OnDisable()
        {
            m_inputReader.Rotate -= Rotate;
            m_inputReader.Click -= OnClick;
        }

        private void LateUpdate()
        {
            Translate(m_inputReader.MoveDirection);
            Zoom(m_inputReader.CurrentZoom);
        }

        private void Translate(Vector2 newVal)
        {
            // Adjusted translation logic for isometric camera
            Vector3 horizontalTranslation = new Vector3(-newVal.y, 0f, newVal.y) * m_translateSpeed;
            Vector3 verticalTranslation = new Vector3(newVal.x, 0f, newVal.x) * m_translateSpeed;

            Vector3 translation = (horizontalTranslation + verticalTranslation) * Time.deltaTime;

            transform.Translate(translation, Space.Self);
        }

        private void Zoom(float newVal)
        {
            float zoomDelta = m_zoomSpeed * newVal;
            float newOrthographicSize = m_camera.orthographicSize + zoomDelta;
            newOrthographicSize = Mathf.Clamp(newOrthographicSize, m_maxZoomIn, m_maxZoomOut);
            m_camera.orthographicSize = newOrthographicSize;
        }

        private void Rotate(float input)
        {
            if (m_isRotating)
                return;

            float targetRotationY = transform.eulerAngles.y;
            targetRotationY = Mathf.Round(targetRotationY + 90f * Mathf.Sign(input)); // Ensure it snaps to 90-degree increments
            StopAllCoroutines();
            StartCoroutine(SmoothRotate(targetRotationY));
        }

        private IEnumerator SmoothRotate(float targetRotationY)
        {
            m_isRotating = true;
            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.x, targetRotationY, transform.rotation.z);
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime);
                elapsedTime += Time.deltaTime * m_rotateSpeed;
                yield return null;
            }

            transform.rotation = targetRotation;
            m_isRotating = false;
        }

        public void MoveToPoint(Vector3 point)
        {
            StopAllCoroutines();
            m_inputReader.DisablePlayerActions();
            StartCoroutine(Move(point));
        }

        private IEnumerator Move(Vector3 point)
        {
            while (Vector3.Distance(transform.position, point) > 0.001f)
            {
                transform.position = Vector3.MoveTowards(transform.position, point, m_translateSpeed * Time.deltaTime);
                yield return null;
            }

            transform.position = point;
            m_inputReader.EnablePlayerActions();
        }
        
        private void OnClick()
        {
            Ray ray = m_camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Construction")))
            {
                if (hit.collider.TryGetComponent(out GridTile tile))
                {
                    tile.Clicked();
                }
            }
        }
    }
}