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
var diffArg = new Argument<string>("filename", "Compares AOB difference");
var diffOpt = new Option<string>("--wildcard", "Wildcard character");
diffOpt.AddAlias("-w");
diffOpt.SetDefaultValue('?');

var diffCmd = new Command("diff", "AOB difference");
diffCmd.AddArgument(diffArg);
diffCmd.AddOption(diffOpt);
diffCmd.SetHandler((filename, wildcard) =>
{
    Console.WriteLine(CommandHandler.HandleDiff(File.ReadAllLines(filename), wildcard[0]));
}, diffArg, diffOpt);

// root
var rootCmd = new RootCommand("Small tool to help with AOB");
rootCmd.AddCommand(countCmd);
rootCmd.AddCommand(formatCmd);
rootCmd.AddCommand(diffCmd);

await rootCmd.InvokeAsync(args);
