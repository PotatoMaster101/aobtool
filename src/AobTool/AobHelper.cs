using System.Text.RegularExpressions;

namespace AobTool;

/// <summary>
/// Helper methods for dealing with AOBs.
/// </summary>
public static class AobHelper
{
    /// <summary>
    /// Compares an AOB with another AOB and returns the result AOB.
    /// </summary>
    /// <param name="aob1">The first AOB to compare.</param>
    /// <param name="aob2">The second AOB to compare.</param>
    /// <param name="wildcard">The wildcard character to use.</param>
    /// <returns>The result AOB.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="wildcard"/> is invalid.</exception>
    public static IEnumerable<ByteString> CompareAob(IEnumerable<ByteString> aob1, IEnumerable<ByteString> aob2, char wildcard = '?')
    {
        var aob1List = aob1.ToList();
        var aob2List = aob2.ToList();
        var size = Math.Max(aob1List.Count, aob2List.Count);
        var result = new List<ByteString>(size);

        // compare byte strings in both AOB
        var limit = Math.Min(aob1List.Count, aob2List.Count);
        for (var i = 0; i < limit; i++)
            result.Add(aob1List[i].GetComparisonResult(aob2List[i], wildcard));

        if (size > limit)
        {
            // fill in "leftovers" with wildcard
            var wildcardByte = new ByteString(0x00, true, true, wildcard);
            result.AddRange(Enumerable.Repeat(wildcardByte, size - limit));
        }
        return result;
    }

    /// <summary>
    /// Parses a line of AOB.
    /// </summary>
    /// <param name="aob">The AOB to parse.</param>
    /// <returns>The list of byte strings parsed.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="aob"/> is null or empty, or a byte string is invalid.</exception>
    public static IEnumerable<ByteString> ParseAob(string aob)
    {
        if (string.IsNullOrEmpty(aob))
            throw new ArgumentException(nameof(string.IsNullOrEmpty), nameof(aob));

        aob = Regex.Replace(aob, @"\s", string.Empty);
        if (aob.Length % 2 == 1)
            aob = '0' + aob;

        var result = new List<ByteString>(aob.Length / 2);
        for (var i = 2; i < aob.Length + 2; i += 2)     // length +2 to include last element
            result.Add(new ByteString(aob[(i - 2)..i]));
        return result;
    }
}
