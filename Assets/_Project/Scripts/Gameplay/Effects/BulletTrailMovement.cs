using UnityEngine;
using UnityServiceLocator;

namespace _Project.Scripts.Gameplay.Effects
{
    public class BulletTrailMovement : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private TrailRenderer _trailRenderer;
        
        private Vector3 _target;
        private float _speed;
        private ObjectPool _objectPool;
        
        private void Start()
        {
            _objectPool = ServiceLocator.Global.Get<ObjectPool>();
        }

        private void OnEnable()
        {
            _trailRenderer.emitting = false;
        }

        private void OnDisable()
        {
            _trailRenderer.emitting = false;
        }

        public void SetTarget(Vector3 startPosition, Vector3 target, float speed)
        {
            transform.position = startPosition;
            _target = target;
            _speed = speed;
            
            _trailRenderer.Clear();
            _trailRenderer.emitting = true;
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _target,
                _speed * Time.fixedDeltaTime
            );

            if (transform.position != _target)
                return;

            _trailRenderer.emitting = false;
            _objectPool.ReturnGameObject(gameObject);
        }
    }
}