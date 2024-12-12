namespace CryptoExchange.WebApi.Binding;

/// <summary>
/// Minimal Web API does not support parsing enums with case-insensitivity.
/// Therefor we wrap Enum parameters with this type to customize parsing.
/// </summary>
/// <typeparam name="T">The type of the enum.</typeparam>
internal class EnumBinding<T>
    where T : struct, Enum
{
    private const bool IgnoreCase = true;
    private const bool IgnoreInt = true;

    private T _value;

    public static bool TryParse(string value, out EnumBinding<T> result)
    {
        return TryParse(value, null!, out result);
    }

    public static bool TryParse(string value, IFormatProvider provider, out EnumBinding<T> result)
    {
        result = new EnumBinding<T>();
        if (IgnoreInt && int.TryParse(value, out _))
        {
            return false;
        }

        bool success = Enum.TryParse(value, IgnoreCase, out T parsedValue);
        if (!success || !Enum.IsDefined(parsedValue))
        {
            return false;
        }

        result._value = parsedValue;
        return true;
    }

    public static implicit operator T(EnumBinding<T> e) => e._value;

    public override string ToString() => _value.ToString();
}