using System.Collections;
using Xunit;

namespace AobTool.Test;

/// <summary>
/// Unit tests for <see cref="AobHelper"/>.
/// </summary>
public class AobHelperTest
{
    [Theory]
    [InlineData("aa bb", "aa", "bb")]
    [InlineData("aabb", "aa", "bb")]
    [InlineData("fff", "0f", "ff")]
    [InlineData("xa*b?c?*", "xa", "*b", "?c", "?*")]
    public void ParseAob_ReturnsCorrectValue(string aob, params string[] expected)
    {
        // act
        var result = AobHelper.ParseAob(aob).ToList();

        // assert
        Assert.Equal(expected.Length, result.Count);
        for (var i = 0; i < expected.Length; i++)
            Assert.Equal(expected[i], result[i].Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("z")]
    public void ParseAob_ThrowsOnInvalidAob(string aob)
    {
        // assert
        Assert.Throws<ArgumentException>(() => AobHelper.ParseAob(aob));
    }

    [Theory]
    [ClassData(typeof(CompareAobTestData))]
    public void CompareAob_ReturnsCorrectValue(ByteString[] aob1, ByteString[] aob2, char wildcard, ByteString[] expected)
    {
        // act
        var result = AobHelper.CompareAob(aob1, aob2, wildcard).ToList();

        // assert
        Assert.Equal(expected.Length, result.Count);
        for (var i = 0; i < expected.Length; i++)
            Assert.Equal(expected[i].Value, result[i].Value);
    }

    /// <summary>
    /// Test data for <see cref="CompareAob_ReturnsCorrectValue"/>.
    /// </summary>
    private class CompareAobTestData : IEnumerable<object[]>
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new[] { new ByteString("aa"), new ByteString("bb") },
                new[] { new ByteString("aa"), new ByteString("bb"), new ByteString("cc") },
                '?',
                new[] { new ByteString("aa"), new ByteString("bb"), new ByteString("??") }
            };
            yield return new object[]
            {
                new[] { new ByteString("aa"), new ByteString("bb"), new ByteString("cc"), new ByteString("00") },
                new[] { new ByteString("ab"), new ByteString("ab"), new ByteString("cc") },
                '*',
                new[] { new ByteString("a*"), new ByteString("*b"), new ByteString("cc"), new ByteString("**") }
            };
        }
    }
}
