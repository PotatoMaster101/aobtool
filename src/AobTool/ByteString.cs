namespace AobTool;

/// <summary>
/// Represents a byte in an AOB string.
/// </summary>
public class ByteString
{
    /// <summary>
    /// Valid wildcard characters.
    /// </summary>
    public const string ValidWildcards = "?*x";

    /// <summary>
    /// Gets the byte string value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets whether the first hex character is a wildcard.
    /// </summary>
    public bool IsFirstCharWildcard => ValidWildcards.Contains(Value[0]);

    /// <summary>
    /// Gets whether the second hex character is a wildcard.
    /// </summary>
    public bool IsSecondCharWildcard => ValidWildcards.Contains(Value[1]);

    /// <summary>
    /// Gets whether the byte string has a wildcard character.
    /// </summary>
    public bool HasWildcard => IsFirstCharWildcard || IsSecondCharWildcard;

    /// <summary>
    /// Gets whether the byte string contains only wildcard.
    /// </summary>
    public bool AllWildcard => IsFirstCharWildcard && IsSecondCharWildcard;

    /// <summary>
    /// Constructs a new instance of <see cref="ByteString"/>.
    /// </summary>
    /// <param name="byteString">The byte string.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="byteString"/> is invalid.</exception>
    public ByteString(string byteString)
    {
        if (string.IsNullOrEmpty(byteString))
            throw new ArgumentException(nameof(string.IsNullOrEmpty), nameof(byteString));

        if (byteString.Length > 2 && byteString.StartsWith("0x"))
            byteString = byteString.Remove(0, 2);       // remove starting '0x'
        if (byteString.Length == 1)
            byteString = '0' + byteString;
        if (byteString.Length != 2)
            throw new ArgumentException($"Byte string has invalid length: {byteString}", nameof(byteString));

        if (IsHexOrWildcardChar(byteString[0]) && IsHexOrWildcardChar(byteString[1]))
            Value = byteString;
        else
            throw new ArgumentException($"Byte string contains invalid character: {byteString}", nameof(byteString));
    }

    /// <summary>
    /// Constructs a new instance of <see cref="ByteString"/>.
    /// </summary>
    /// <param name="value">The byte value.</param>
    /// <param name="isFirstCharWildcard">Whether the first hex character is a wildcard.</param>
    /// <param name="isSecondCharWildcard">Whether the second hex character is a wildcard.</param>
    /// <param name="wildcard">The wildcard character.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="wildcard"/> is invalid.</exception>
    public ByteString(byte value, bool isFirstCharWildcard = false, bool isSecondCharWildcard = false, char wildcard = '?')
    {
        if (!ValidWildcards.Contains(wildcard))
            throw new ArgumentException($"Wildcard {wildcard} is invalid", nameof(wildcard));

        var byteString = value.ToString("X2");
        if (isFirstCharWildcard)
            byteString = $"{wildcard}{byteString[1]}";
        if (isSecondCharWildcard)
            byteString = $"{byteString[0]}{wildcard}";
        Value = byteString;
    }

    /// <summary>
    /// Compares this <see cref="ByteString"/> with another, and returns the comparison result.
    /// </summary>
    /// <param name="another">Another <see cref="ByteString"/> to compare.</param>
    /// <param name="wildcard">The wildcard character to use if a hex character differ.</param>
    /// <returns>The result of comparison.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="wildcard"/> is invalid.</exception>
    public ByteString GetComparisonResult(ByteString another, char wildcard = '?')
    {
        if (!ValidWildcards.Contains(wildcard))
            throw new ArgumentException($"Wildcard {wildcard} is invalid", nameof(wildcard));

        // get standard format string first so comparison will be easier
        var standardCurrent = GetStandardFormat(Value);
        var standardAnother = GetStandardFormat(another.Value);

        // result will use same hex char from current byte string, but will use wildcard char passed in for wildcards
        char first = Value[0], second = Value[1];
        if (standardCurrent[0] != standardAnother[0] || IsFirstCharWildcard)
            first = wildcard;
        if (standardCurrent[1] != standardAnother[1] || IsSecondCharWildcard)
            second = wildcard;
        return new ByteString($"{first}{second}");
    }

    /// <summary>
    /// Returns the actual byte value.
    /// </summary>
    /// <returns>The actual byte value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a wildcard character exists.</exception>
    public byte GetByteValue()
    {
        if (HasWildcard)
            throw new InvalidOperationException(nameof(HasWildcard));
        return Convert.ToByte(Value, 16);
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Value;
    }

    /// <summary>
    /// Converts a byte string to a standard format, where wildcards are using '?' and hex are capital.
    /// </summary>
    /// <param name="byteString">The byte string to convert.</param>
    /// <returns>The result of conversion.</returns>
    private static string GetStandardFormat(string byteString)
    {
        var upper = byteString.ToUpper();
        char first = upper[0], second = upper[1];
        if (ValidWildcards.Contains(byteString[0]))
            first = '?';
        if (ValidWildcards.Contains(byteString[1]))
            second = '?';
        return $"{first}{second}";
    }

    /// <summary>
    /// Determines whether a character is a valid hex character.
    /// </summary>
    /// <param name="ch">The character to test.</param>
    /// <returns>Whether a character is a valid hex character.</returns>
    private static bool IsHexChar(char ch)
    {
        return ch is >= '0' and <= '9' or >= 'a' and <= 'f' or >= 'A' and <= 'F';
    }

    /// <summary>
    /// Determines whether a character is a valid hex or wildcard character.
    /// </summary>
    /// <param name="ch">The character to test.</param>
    /// <returns>Whether a character is a valid hex or wildcard character.</returns>
    private static bool IsHexOrWildcardChar(char ch)
    {
        return IsHexChar(ch) || ValidWildcards.Contains(ch);
    }
}
