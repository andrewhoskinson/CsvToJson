using CommandLine;
using CSVToJson.DataSources;
using CSVToJson.Instantiation;
using CSVToJson.Mapping;
using CSVToJson.Options;
using CSVToJson.Output;
using CSVToJson.Sample;
using System;
using System.Linq;
using System.Reflection;

namespace CSVToJson
{
    class Program
    {       
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed<CommandLineOptions>(Run);            
        }
        
        static void Run(CommandLineOptions options)
        {
            if (options.CreatorType == CreatorType.StronglyTyped)
            {
                // Register all the class maps in this assembly (Person and Address objects in Sample namespace)
                MapRegistrar.Register(Assembly.GetExecutingAssembly());
            }
            //else
            //{
            //    if (options.OutputType == OutputType.Xml)
            //    {
            //        Console.WriteLine("Error: Cannot use Xml output with dynamic object creation, add -c StronglyTyped to the command line arguments.");
            //        return;
            //    }
            //}

            using (IDataSource ds = DataSourceFactory.Create(options.DataSourceType))
            {
                ds.Initialise(options.DataSource);

                dynamic people = null;

                //if (options.OutputType == OutputType.Xml)
                //{
                //    // Bit hacky this, but need to use strongly typed objects for Xml serialisation
                //    people = CreatorFactory.Create<Person>(options.CreatorType)
                //        .GetObjects(ds).Cast<Person>().ToArray();                        
                //}
                //else
                //{
                //    people = CreatorFactory.Create<Person>(options.CreatorType)
                //        .GetObjects(ds);
                //}

                people = CreatorFactory.Create<Person>(options.CreatorType)
                        .GetObjects(ds);

                var outputter = OutputFactory.Create<Person>(options.OutputType);

                Console.WriteLine(outputter.Output(people));
            }
        }
    }
}
