namespace StableIndexCollection;

/// <summary>
/// Simple type definition for <see cref="int"/> to help delineate between an index and an <see cref="ID"/> for methods in <see cref="StableIndexCollection{T}"/>
/// </summary>
/// <param name="value"><see cref="int"/> value this <see cref="ID"/> wraps.</param>
public class ID(int value) : IEquatable<ID>
{
    private int Value { get; init; } = value;

    /// <summary>
    /// Converts an <see cref="ID"/> instance to its underlying integer value.
    /// </summary>
    /// <remarks>This operator enables implicit conversion from an <see cref="ID"/> to an
    /// <see cref="int"/>, allowing an <see cref="ID"/> to be used wherever an <see cref="int"/> is expected.</remarks>
    /// <param name="id">The <see cref="ID"/> instance to convert.</param>
    public static implicit operator int(ID id) => id.Value;

    /// <summary>
    /// Converts an integer value to an ID instance.
    /// </summary>
    /// <remarks>This operator enables implicit conversion from an <see cref="int"/> to an 
    /// <see cref="ID"/>, allowing an <see cref="int"/> to be used wherever an <see cref="ID"/> is expected.</remarks>
    /// <param name="value">The <see cref="int"/>> value to convert.</param>
    public static implicit operator ID(int value) => new(value);

    public static ID operator +(ID left, ID right) => new(left.Value + right.Value);

    public override string ToString() => Value.ToString();

    public bool Equals(ID? other)
    {
        if (other == null)
        {
            return false;
        }

        return Value == other.Value;
    }
}
