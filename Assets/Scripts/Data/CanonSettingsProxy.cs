using Utils;

namespace Data
{
    public class CanonSettingsProxy
    {
        private readonly ObservableProperty<int> _power = new(50);
        
        public IReadOnlyObservableProperty<int> Power => _power;

        public void SetPower(int value)
        {
            _power.Value = value;
        }
    }
}