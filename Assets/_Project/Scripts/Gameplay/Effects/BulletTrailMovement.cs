using System;
using Gameplay.ObjectPool;
using UnityEngine;
using UnityServiceLocator;

namespace _Project.Scripts.Gameplay.Effects
{
    public class BulletTrailMovement : MonoBehaviour
    {
        private Vector3 _target;
        private float _speed;
        private ObjectPool _objectPool;
        
        private void Start()
        {
            _objectPool = ServiceLocator.Global.Get<ObjectPool>();
        }

        public void SetTarget(Vector3 target, float speed)
        {
            _target = target;
            _speed = speed;
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _target,
                _speed * Time.fixedDeltaTime
            );

            if (transform.position == _target)
            {
                _objectPool.ReturnGameObject(gameObject);
            }
        }
    }
}