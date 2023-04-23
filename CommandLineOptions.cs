using CommandLine;

namespace PrintThatCard;

internal class CommandLineOptions
{
    [Option(shortName: 'l', longName: "lines", Required = false, HelpText = "Print lines.", Default = false)]
    public bool Lines { get; set; }
    
    [Option(shortName: 'c', longName: "columns", Required = false, HelpText = "Columns per page.", Default = 2)]
    public int Columns { get; set; }
    
    [Option(shortName: 'r', longName: "rows", Required = false, HelpText = "Rows per page.", Default = 3)]
    public int Rows { get; set; }
    
    [Option(shortName: 'd', longName: "dpi", Required = false, HelpText = "Dpi of output image.", Default = 300)]
    public int Dpi { get; set; }
    
    [Option(longName: "pagewidth", Required = false, HelpText = "Page width in millimeters.", Default = 210)]
    public int PageWidth { get; set; }
    
    [Option(longName: "pageheight", Required = false, HelpText = "Page height in millimeters.", Default = 297)]
    public int PageHeight { get; set; }
    
    [Option(longName: "pagewidth", Required = false, HelpText = "Card width in millimeters.", Default = 63)]
    public int CardWidth { get; set; }
    
    [Option(longName: "pageheight", Required = false, HelpText = "Card height in millimeters.", Default = 88)]
    public int CardHeight { get; set; }

    [Value(index: 0, Required = true, HelpText = "Input directory.")]
    public string Input { get; set; } = string.Empty;

    [Value(index: 1, Required = true, HelpText = "Output file name.")]
    public string Output { get; set; } = string.Empty;
}