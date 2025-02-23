using System;
using Utils;

namespace Data
{
    public class CanonSettingsProxy: IDisposable
    {
        private readonly ObservableProperty<int> _power = new(ProjectConstants.DefaultPower);
        private readonly ObservableProperty<float> _sensitivity = new(ProjectConstants.DefaultSensitivity);
        
        private readonly IDisposable _sensitivityListener;

        public IReadOnlyObservableProperty<int> Power => _power;
        public IReadOnlyObservableProperty<float> Sensitivity => _sensitivity;

        public CanonSettingsProxy()
        {
            _sensitivity.Value = PlayerPrefsHelper.Sensitivity;
            _sensitivityListener = _sensitivity.Subscribe(value =>
            {
                PlayerPrefsHelper.Sensitivity = value;
            });
        }
        
        public void SetPower(int value)
        {
            _power.Value = value;
        }

        public void SetSensitivity(float value)
        {
            _sensitivity.Value = value;
        }

        public void Dispose()
        {
            _sensitivityListener?.Dispose();
        }
    }
}