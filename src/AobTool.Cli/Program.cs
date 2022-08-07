using System.CommandLine;
using AobTool.Cli;

// count
var countArg = new Argument<string>("aob", "Counts the number of bytes in an AOB");
var countCmd = new Command("count", "AOB counter");
countCmd.AddArgument(countArg);
countCmd.SetHandler(aob =>
{
    Console.WriteLine(CommandHandler.HandleCount(aob));
}, countArg);

// format
var formatArg = new Argument<string>("aob", "Formats the AOB");
var formatCmd = new Command("format", "AOB formatter");
formatCmd.AddArgument(formatArg);
formatCmd.SetHandler(aob =>
{
    Console.WriteLine(CommandHandler.HandleFormat(aob));
}, formatArg);

// diff
var diffWildcardOpt = new Option<string>("--wildcard", "Wildcard character") { IsRequired = false };
diffWildcardOpt.AddAlias("-w");
var diffFileOpt = new Option<string>("--file", "AOB diff file, 1 AOB per line") { IsRequired = false };
diffFileOpt.AddAlias("-f");
var diffStdinOpt = new Option<bool>("--stdin", "Read AOB from stdin, empty line to stop") { IsRequired = false };

var diffCmd = new Command("diff", "AOB difference");
diffCmd.AddOption(diffFileOpt);
diffCmd.AddOption(diffWildcardOpt);
diffCmd.AddOption(diffStdinOpt);
diffCmd.SetHandler((filename, wildcard, stdin) =>
{
    if (string.IsNullOrEmpty(wildcard))
        wildcard = "?";

    var lines = new List<string>();
    if (string.IsNullOrEmpty(filename))
        stdin = true;
    else
        lines.AddRange(File.ReadAllLines(filename));

    if (stdin)
    {
        Console.WriteLine("Reading AOB from stdin...");
        lines.AddRange(CommandHandler.ReadStdin());
    }

    Console.WriteLine(CommandHandler.HandleDiff(lines, wildcard[0]));
}, diffFileOpt, diffWildcardOpt, diffStdinOpt);

// root
var rootCmd = new RootCommand("Small tool to help with AOB");
rootCmd.AddCommand(countCmd);
rootCmd.AddCommand(formatCmd);
rootCmd.AddCommand(diffCmd);

await rootCmd.InvokeAsync(args);
