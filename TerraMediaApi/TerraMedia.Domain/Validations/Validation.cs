
using TerraMedia.Domain.Exceptions;

namespace TerraMedia.Domain.Validations;

public static class Validation
{
    public static void ValidateIfEqual(object obj1, object obj2, string message)
    {
        if (obj1.Equals(obj2))
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateLength(string value, int min, int max, string message)
    {
        var length = value.Trim().Length;
        if (length < min || length > max)
        {
            throw new DomainException(string.Format(message, min, max));
        }
    }

    public static void ValidateIfEmpty(string value, string message)
    {
        if (value == null || value.Trim().Length == 0)
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateIfNull(object obj, string message)
    {
        if (obj == null)
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateMinMax(int value, int min, int max, string message)
    {
        if (value < min || value > max)
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateMinMax(decimal value, decimal min, decimal max, string message)
    {
        if (value < min || value > max)
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateIfFalse(bool boolValue, string message)
    {
        if (!boolValue)
        {
            throw new DomainException(message);
        }
    }

    public static void ValidateIfTrue(bool boolValue, string message)
    {
        if (boolValue)
        {
            throw new DomainException(message);
        }
    }

}
