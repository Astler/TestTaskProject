using System;

namespace Utils
{
    public interface IReadOnlyObservableProperty<out T>
    {
        public T Value { get; }
        IDisposable Subscribe(Action<T> observer);
    }

    public class ObservableProperty<T> : IReadOnlyObservableProperty<T>
    {
        private T _value;
        private event Action<T> _onValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value))
                    return;

                _value = value;
                _onValueChanged?.Invoke(_value);
            }
        }

        public ObservableProperty(T initialValue = default)
        {
            _value = initialValue;
        }

        public IDisposable Subscribe(Action<T> observer)
        {
            _onValueChanged += observer;
            observer?.Invoke(_value);

            return new Subscription(() => _onValueChanged -= observer);
        }

        private class Subscription : IDisposable
        {
            private readonly Action _unsubscribe;
            private bool _isDisposed;

            public Subscription(Action unsubscribe)
            {
                _unsubscribe = unsubscribe;
            }

            public void Dispose()
            {
                if (_isDisposed)
                    return;

                _unsubscribe?.Invoke();
                _isDisposed = true;
            }
        }
    }
}