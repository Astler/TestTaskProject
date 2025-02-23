using System;
using Data;

namespace Screens.GameHud
{
    public class HudPresenter : IDisposable
    {
        private readonly GameHugView _hudView;
        private readonly CanonSettingsProxy _canonSettingsProxy;
        private IDisposable _powerListener;
        private IDisposable _sensitivityListener;

        public HudPresenter(GameHugView hudView, CanonSettingsProxy canonSettingsProxy)
        {
            _hudView = hudView;
            _canonSettingsProxy = canonSettingsProxy;
        }

        public void Initialize()
        {
            _hudView.PowerValueChanged += OnPowerChanged;
            _hudView.SensitivityValueChanged += OnSensitivityValueChanged;
            _powerListener = _canonSettingsProxy.Power.Subscribe(value => { _hudView.SetPower(value, true); });
            _sensitivityListener = _canonSettingsProxy.Sensitivity.Subscribe(value =>
            {
                _hudView.SetSensitivity(value, true);
            });
        }

        private void OnPowerChanged(int value)
        {
            _canonSettingsProxy.SetPower(value);
        }

        private void OnSensitivityValueChanged(float value)
        {
            _canonSettingsProxy.SetSensitivity(value);
        }

        public void Dispose()
        {
            _hudView.PowerValueChanged -= OnPowerChanged;
            _hudView.SensitivityValueChanged -= OnSensitivityValueChanged;
            _powerListener?.Dispose();
            _sensitivityListener?.Dispose();
        }
    }
}