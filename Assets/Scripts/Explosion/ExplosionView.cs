using System.Collections;
using PoolsSystem;
using UnityEngine;

namespace Explosion
{
    public class ExplosionView : BasePoolable<ExplosionView, ExplosionSpawnInfo>
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private float lifeTime = 1.5f;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        public override void OnSpawned(ExplosionSpawnInfo spawnInfo)
        {
            base.OnSpawned(spawnInfo);
            _transform.position = spawnInfo.WorldPosition;
            particle.Play(true);

            StartCoroutine(DespawnTimer());
        }

        public override void OnDespawned()
        {
            base.OnDespawned();
            particle.Clear(true);
        }

        private IEnumerator DespawnTimer()
        {
            yield return new WaitForSeconds(lifeTime);
            Despawn();
        }
    }
}