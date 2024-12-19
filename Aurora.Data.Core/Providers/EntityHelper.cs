namespace Autota.Data.Core.Providers;

public static class EntityHelper<T> where T : struct, Enum
{
    public static T ToEnumNullable( string value, T defaultValue)
    {
        if (string.IsNullOrEmpty(value))
        {
            return defaultValue;
        }

        return Enum.TryParse(value, true, out T result) ? result : defaultValue;
    }
}