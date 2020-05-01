using CommandLine;

namespace CSVToJson.Options
{
    /// <summary>
    /// Simple command line options, will be parsed using the CommandLine NuGet package
    /// </summary>
    public class CommandLineOptions
    {
        [Option('t', "type", Required = false, HelpText = "Sets the input type [NaiveCSVFile|NugetCSVFile|SqlServerDataSource]")]
        public DataSources.DataSourceType DataSourceType { get; set; } = DataSources.DataSourceType.NaiveCSVFile;

        [Option('i', "input", Required = true, HelpText = "Sets the datasource, either a file or a connection string if running in SQL Server mode")]
        public string DataSource { get; set; }

        [Option('c', "creator", Required = false, HelpText = "Sets the object creator type [Dynamic|StronglyTyped]")]
        public Instantiation.CreatorType CreatorType { get; set; } = Instantiation.CreatorType.Dynamic;

        [Option('o', "output", Required = false, HelpText = "Sets the output type [Json|Xml]")]
        public Output.OutputType OutputType { get; set; } = Output.OutputType.Json;
    }
}
