using System;
using Data;

namespace Screens.GameHud
{
    public class HudPresenter : IDisposable
    {
        private readonly GameHugView _hudView;
        private readonly CanonSettingsProxy _canonSettingsProxy;
        private IDisposable _powerListener;

        public HudPresenter(GameHugView hudView, CanonSettingsProxy canonSettingsProxy)
        {
            _hudView = hudView;
            _canonSettingsProxy = canonSettingsProxy;
        }

        public void Initialize()
        {
            _hudView.PowerValueChanged += OnPowerChanged;
            _powerListener = _canonSettingsProxy.Power.Subscribe(value => { _hudView.SetPower(value, true); });
        }

        private void OnPowerChanged(int value)
        {
            _canonSettingsProxy.SetPower(value);
        }

        public void Dispose()
        {
            _hudView.PowerValueChanged -= OnPowerChanged;
            _powerListener?.Dispose();
        }
    }
}