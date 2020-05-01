using System;

namespace CSVToJson.DataSources
{
    /// <summary>
    /// Interface that defines a datasource for input. 
    /// </summary>
    public interface IDataSource : IDisposable
    {
        /// <summary>
        /// Initialises the datasource.
        /// </summary>
        /// <param name="parameters">For file based datasources, the filename. For database datasources, the connection string</param>
        void Initialise(string parameters);

        /// <summary>
        /// Can the datasource read more values?
        /// </summary>
        bool Read();

        /// <summary>
        /// The current value for the specified field
        /// </summary>
        object this[string fieldName] { get; }

        /// <summary>
        /// The list of fields in the order in which they are encountered
        /// </summary>
        string[] OrderedFields { get; }        
    }
}
