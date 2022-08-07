namespace AobTool.Cli;

/// <summary>
/// Handles CLI commands.
/// </summary>
public static class CommandHandler
{
    /// <summary>
    /// Handles count command.
    /// </summary>
    /// <param name="aob">The user specified AOB.</param>
    /// <returns>Count of AOB.</returns>
    public static int HandleCount(string aob)
    {
        return AobHelper.ParseAob(aob).ToList().Count;
    }

    /// <summary>
    /// Handles format command.
    /// </summary>
    /// <param name="aob">The user specified AOB.</param>
    /// <returns>Formatted AOB.</returns>
    public static string HandleFormat(string aob)
    {
        return string.Join(" ", AobHelper.ParseAob(aob));
    }

    /// <summary>
    /// Handles diff command.
    /// </summary>
    /// <param name="aobs">The user specified list of AOB to compare.</param>
    /// <param name="wildcard">The user specified wildcard character.</param>
    /// <returns>The result of comparison.</returns>
    public static string HandleDiff(IEnumerable<string> aobs, char wildcard)
    {
        var aobsList = aobs.ToList();
        if (aobsList.Count == 0)
            return string.Empty;

        var current = AobHelper.ParseAob(aobsList[0]);
        foreach (var aob in aobsList)
            current = AobHelper.CompareAob(current, AobHelper.ParseAob(aob), wildcard);
        return string.Join(" ", current);
    }

    /// <summary>
    /// Reads strings from stdin.
    /// </summary>
    /// <returns>The input entered by user.</returns>
    public static IEnumerable<string> ReadStdin()
    {
        var result = new List<string>();
        var text = Console.ReadLine();

        while (!string.IsNullOrEmpty(text))
        {
            result.Add(text);
            text = Console.ReadLine();
        }
        return result;
    }
}
