using Canon;
using Data;
using Explosion;
using GameCamera;
using GameInput;
using PoolsSystem;
using Projectiles;
using Screens.GameHud;
using UnityEditor;
using UnityEngine;
using Utils.MeshGenerator;

namespace StateMachine
{
    public class TempMainSceneInstaller : MonoBehaviour
    {
        [SerializeField] private GameInputSystem gameInputSystem;
        [SerializeField] private CanonView canonView;
        [SerializeField] private GameHugView hudView;
        [SerializeField] private PoolConfig poolSettings;
        [SerializeField] private CameraController cameraController;

        private CanonPresenter _canonPresenter;
        private CanonSettingsProxy _canonSettingsProxy;
        private HudPresenter _hudPresenter;
        private PoolsSystem.PoolsSystem _poolsSystem;
        private ProjectilesFactory _projectilesFactory;
        private ExplosionsFactory _explosionsFactory;

        private void Awake()
        {
            _canonSettingsProxy = new CanonSettingsProxy();
            _poolsSystem = new PoolsSystem.PoolsSystem(poolSettings);
            _projectilesFactory = new ProjectilesFactory(_poolsSystem);
            _explosionsFactory = new ExplosionsFactory(_poolsSystem);
            _canonPresenter = new CanonPresenter(canonView, gameInputSystem, _canonSettingsProxy, _projectilesFactory,
                cameraController, _explosionsFactory);
            _hudPresenter = new HudPresenter(hudView, _canonSettingsProxy);
        }

        private void Start()
        {
            _poolsSystem.Initialize();
            _canonPresenter.Initialize();
            _hudPresenter.Initialize();
        }

        private void OnDestroy()
        {
            _canonPresenter.Dispose();
            _hudPresenter.Dispose();
        }
    }
}