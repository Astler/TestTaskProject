using System;
using MarkableWall;
using PoolsSystem;
using UnityEngine;
using Utils.MeshGenerator;

namespace Projectiles
{
    public class ProjectileView : BasePoolable<ProjectileView, ProjectileSpawnInfo>
    {
        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private SphereCollider sphereCollider;
        [SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private float bounciness = 0.6f;
        [SerializeField] private int maxBounces = 2;

        private Vector3 _position;
        private Vector3 _velocity;
        private bool _isFlying;
        private int _bounces;
        private Vector3 _lastBounceNormal;
        private bool _justBounced;
        private Transform _transform;

        public Vector3 Position => _transform.position;

        public event Action<ProjectileView> OnExplode;

        private void Awake()
        {
            _transform = transform;
        }

        public override void OnSpawned(ProjectileSpawnInfo spawnInfo)
        {
            base.OnSpawned(spawnInfo);

            _position = spawnInfo.StartWorldPosition;
            _transform.position = _position;
            _transform.rotation = Quaternion.LookRotation(spawnInfo.Direction);

            meshFilter.mesh = MeshGenerator.GenerateDeformed(6, ProjectConstants.ProjectileSize);
            sphereCollider.radius = ProjectConstants.ProjectileSize;

            _velocity = spawnInfo.Direction * spawnInfo.Power;
            _isFlying = true;

            _bounces = 0;
        }

        private void FixedUpdate()
        {
            if (!_isFlying) return;

            _velocity += ProjectConstants.Gravity * Time.fixedDeltaTime;

            Vector3 nextPosition = _position + _velocity * Time.fixedDeltaTime;
            Vector3 movement = nextPosition - _position;

            if (Physics.SphereCast(_position, sphereCollider.radius, movement.normalized, out RaycastHit hit,
                    movement.magnitude, collisionMask))
            {
                float dotProduct = Vector3.Dot(_velocity.normalized, hit.normal);
                if (dotProduct < 0)
                {
                    HandleCollision(hit);
                }
            }
            else
            {
                _position = nextPosition;
            }

            _transform.position = _position;
            _transform.forward = _velocity.normalized;
        }

        private void HandleCollision(RaycastHit hit)
        {
            Vector3 reflected = Vector3.Reflect(_velocity, hit.normal);
            _velocity = reflected * (bounciness * ProjectConstants.VelocityDecreaseFactor);
            _position = hit.point + hit.normal * sphereCollider.radius;

            TryToLeaveMark(hit);

            _bounces++;

            if (_bounces < maxBounces) return;

            Explode();
        }

        private void TryToLeaveMark(RaycastHit hit)
        {
            if (!hit.collider.TryGetComponent(out MarkableWallView wallView)) return;
            wallView.AddMark(hit);
        }

        private void Explode()
        {
            _isFlying = false;
            OnExplode?.Invoke(this);
            Despawn();
        }
    }
}