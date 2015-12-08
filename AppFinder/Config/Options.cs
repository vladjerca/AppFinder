using CommandLine;
using CommandLine.Text;

namespace AppFinder.Config
{
    class Options
    {
        [Option('f', "file", Required = true, 
            HelpText = "Portable exe you are looking for. eg: RufusPortable.exe")]
        public string ExeName { get; set; }

        [Option('d', "dir", Required = false, DefaultValue = null,
            HelpText = @"Directory in which the exe can be found. NOTE: the directory can be of any parent level, 
                        this options speeds up searches and can be used if multiple 'exe' files exist with the same name.")]
        public string Folder { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
