using System;

namespace CSVToJson.Output
{
    public enum OutputType
    {
        Json = 0,
        Xml = 1
    }

    /// <summary>
    /// Factory for different output types
    /// </summary>
    public static class OutputFactory
    {
        public static IOutput Create<T>(OutputType outputType)
        {
            switch(outputType)
            {
                case OutputType.Json:
                    return new JsonOutput();

                case OutputType.Xml:
                    return new XmlOutput<T>();
            }

            throw new Exception("No implementation defined for " + outputType);
        }
    }
}
