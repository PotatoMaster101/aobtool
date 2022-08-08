using Xunit;

namespace AobTool.Test;

/// <summary>
/// Unit tests for <see cref="ByteString"/>.
/// </summary>
public class ByteStringTest
{
    [Theory]
    [InlineData("00", "00")]
    [InlineData("0", "00")]
    [InlineData("9", "09")]
    [InlineData("aa", "aa")]
    [InlineData("Ab", "Ab")]
    [InlineData("0xaa", "aa")]
    [InlineData("0xa", "0a")]
    public void StringCtor_SetsValue(string byteString, string expected)
    {
        // act
        var bs = new ByteString(byteString);

        // assert
        Assert.Equal(expected, bs.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("aaa")]
    [InlineData("g")]
    [InlineData("0xg")]
    public void StringCtor_ThrowsOnInvalidString(string byteString)
    {
        // assert
        Assert.Throws<ArgumentException>(() => new ByteString(byteString));
    }

    [Theory]
    [InlineData(0x00, false, false, '?', "00")]
    [InlineData(0xAA, false, false, '?', "AA")]
    [InlineData(0xff, false, false, '?', "FF")]
    [InlineData(0xAA, true, false, '?', "?A")]
    [InlineData(0xAA, false, true, '?', "A?")]
    [InlineData(0xAA, true, true, '?', "??")]
    [InlineData(0xAA, true, false, 'x', "xA")]
    [InlineData(0xAA, false, true, 'x', "Ax")]
    [InlineData(0xAA, true, true, 'x', "xx")]
    [InlineData(0xAA, true, false, '*', "*A")]
    [InlineData(0xAA, false, true, '*', "A*")]
    [InlineData(0xAA, true, true, '*', "**")]
    public void ByteCtor_SetsValue(byte value, bool isFirstCharWildcard, bool isSecondCharWildcard, char wildcard, string expected)
    {
        // act
        var bs = new ByteString(value, isFirstCharWildcard, isSecondCharWildcard, wildcard);

        // assert
        Assert.Equal(expected, bs.Value);
    }

    [Theory]
    [InlineData('X')]
    [InlineData('A')]
    public void ByteCtor_ThrowsOnInvalidWildcard(char wildcard)
    {
        // assert
        Assert.Throws<ArgumentException>(() => new ByteString(0x00, false, false, wildcard));
    }

    [Theory]
    [InlineData("?f", true)]
    [InlineData("*f", true)]
    [InlineData("xf", true)]
    [InlineData("f?", false)]
    [InlineData("f*", false)]
    [InlineData("fx", false)]
    [InlineData("??", true)]
    [InlineData("**", true)]
    [InlineData("xx", true)]
    [InlineData("ff", false)]
    [InlineData("f", false)]
    public void IsFirstCharWildcard_GetsCorrectValue(string byteString, bool expected)
    {
        // arrange
        var bs = new ByteString(byteString);

        // act
        var actual = bs.IsFirstCharWildcard;

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("?f", false)]
    [InlineData("*f", false)]
    [InlineData("xf", false)]
    [InlineData("f?", true)]
    [InlineData("f*", true)]
    [InlineData("fx", true)]
    [InlineData("??", true)]
    [InlineData("**", true)]
    [InlineData("xx", true)]
    [InlineData("ff", false)]
    [InlineData("f", false)]
    public void IsSecondCharWildcard_GetsCorrectValue(string byteString, bool expected)
    {
        // arrange
        var bs = new ByteString(byteString);

        // act
        var actual = bs.IsSecondCharWildcard;

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("?f", true)]
    [InlineData("*f", true)]
    [InlineData("xf", true)]
    [InlineData("f?", true)]
    [InlineData("f*", true)]
    [InlineData("fx", true)]
    [InlineData("??", true)]
    [InlineData("**", true)]
    [InlineData("xx", true)]
    [InlineData("ff", false)]
    [InlineData("f", false)]
    public void HasWildcard_GetsCorrectValue(string byteString, bool expected)
    {
        // arrange
        var bs = new ByteString(byteString);

        // act
        var actual = bs.HasWildcard;

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("?f", false)]
    [InlineData("*f", false)]
    [InlineData("xf", false)]
    [InlineData("f?", false)]
    [InlineData("f*", false)]
    [InlineData("fx", false)]
    [InlineData("??", true)]
    [InlineData("**", true)]
    [InlineData("xx", true)]
    [InlineData("ff", false)]
    [InlineData("f", false)]
    public void AllWildcard_GetsCorrectValue(string byteString, bool expected)
    {
        // arrange
        var bs = new ByteString(byteString);

        // act
        var actual = bs.AllWildcard;

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("?f", "?f", '?', "?f")]
    [InlineData("?f", "?f", '*', "*f")]
    [InlineData("?f", "f?", 'x', "xx")]
    [InlineData("?f", "xf", '?', "?f")]
    [InlineData("*f", "xF", '?', "?f")]
    [InlineData("*F", "xf", 'x', "xF")]
    [InlineData("ab", "CD", '*', "**")]
    public void GetComparisonResult_ReturnsCorrectValue(string byteString, string another, char wildcard, string expected)
    {
        // arrange
        var bs1 = new ByteString(byteString);
        var bs2 = new ByteString(another);

        // act
        var actual = bs1.GetComparisonResult(bs2, wildcard);

        // assert
        Assert.Equal(expected, actual.Value);
    }

    [Theory]
    [InlineData('X')]
    [InlineData('A')]
    public void GetComparisonResult_ThrowsOnInvalidWildcard(char wildcard)
    {
        // arrange
        var bs1 = new ByteString(0x00);
        var bs2 = new ByteString(0x00);

        // assert
        Assert.Throws<ArgumentException>(() => bs1.GetComparisonResult(bs2, wildcard));
    }

    [Theory]
    [InlineData("0", 0x00)]
    [InlineData("ff", 0xFF)]
    [InlineData("ab", 0xAB)]
    public void GetByteValue_ReturnsCorrectValue(string byteString, byte expected)
    {
        // arrange
        var bs = new ByteString(byteString);

        // act
        var actual = bs.GetByteValue();

        // assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("?f")]
    [InlineData("*f")]
    [InlineData("xf")]
    [InlineData("f?")]
    [InlineData("f*")]
    [InlineData("fx")]
    [InlineData("??")]
    [InlineData("**")]
    [InlineData("xx")]
    public void GetByteValue_ThrowsOnWildcard(string byteString)
    {
        // arrange
        var bs = new ByteString(byteString);

        // assert
        Assert.Throws<InvalidOperationException>(() => bs.GetByteValue());
    }

    [Theory]
    [InlineData("0xAb", "Ab")]
    [InlineData("aB", "aB")]
    [InlineData("??", "??")]
    [InlineData("0xxx", "xx")]
    [InlineData("0x", "0x")]
    public void ToString_ReturnsCorrectValue(string byteString, string expected)
    {
        // arrange
        var bs = new ByteString(byteString);

        // act
        var actual = bs.ToString();

        // assert
        Assert.Equal(expected, actual);
    }
}
