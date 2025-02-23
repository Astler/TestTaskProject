using System;
using Data;
using Explosion;
using GameCamera;
using UnityEngine;
using GameInput;
using Projectiles;

namespace Canon
{
    public class CanonPresenter : IDisposable
    {
        private readonly GameInputSystem _gameInputSystem;
        private readonly CanonSettingsProxy _canonSettingsProxy;
        private readonly ProjectilesFactory _projectilesFactory;
        private readonly CameraController _cameraController;
        private readonly ExplosionsFactory _explosionsFactory;
        private readonly CanonView _canonView;
        private IDisposable _powerListener;
        private int _currentPower;

        public CanonPresenter(CanonView canonView, GameInputSystem gameInputSystem,
            CanonSettingsProxy canonSettingsProxy, ProjectilesFactory projectilesFactory,
            CameraController cameraController, ExplosionsFactory explosionsFactory)
        {
            _gameInputSystem = gameInputSystem;
            _canonSettingsProxy = canonSettingsProxy;
            _projectilesFactory = projectilesFactory;
            _cameraController = cameraController;
            _explosionsFactory = explosionsFactory;
            _canonView = canonView;
        }

        public void Initialize()
        {
            _gameInputSystem.AimDeltaChanged += HandleAimDeltaChanged;
            _gameInputSystem.ShotClicked += HandleShotClicked;
            _powerListener = _canonSettingsProxy.Power.Subscribe(value =>
            {
                _currentPower = value;
                UpdateTrajectory();
            });
        }

        private void HandleShotClicked()
        {
            float powerCoefficient = _currentPower / 100f;
            _cameraController.RequestShake(0.2f, Mathf.Lerp(0.05f, 0.35f, powerCoefficient));
            _canonView.PlayRecoilAnimation(Mathf.Lerp(0.25f, 0.75f, powerCoefficient));
            ProjectileView projectile = _projectilesFactory.Create(_canonView.ShotPosition.position,
                _canonView.ShotPosition.forward, _currentPower);

            projectile.OnExplode += OnProjectileExplode;
            return;

            void OnProjectileExplode(ProjectileView _)
            {
                projectile.OnExplode -= OnProjectileExplode;
                _explosionsFactory.Create(projectile.Position);
                _cameraController.RequestShake();
            }
        }

        private void HandleAimDeltaChanged(Vector2 aimDelta)
        {
            _canonView.UpdateAim(aimDelta * _canonSettingsProxy.Sensitivity.Value);
            UpdateTrajectory();
        }

        private void UpdateTrajectory()
        {
            Vector3 velocity = _canonView.ShotPosition.forward * _currentPower;
            _canonView.TrajectoryRenderer.UpdateTrajectory(_canonView.ShotPosition.position, velocity);
        }

        public void Dispose()
        {
            _powerListener?.Dispose();
            _gameInputSystem.AimDeltaChanged -= HandleAimDeltaChanged;
        }
    }
}