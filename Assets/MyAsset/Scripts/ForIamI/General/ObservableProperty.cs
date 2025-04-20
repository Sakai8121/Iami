using System;

public class ObservableProperty<T>
{
    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    public event Action<T> OnValueChanged;

    public ObservableProperty(T initialValue = default)
    {
        _value = initialValue;
    }
}
