using System.Collections.Generic;
using CSVToJson.DataSources;

namespace CSVToJson.Instantiation
{
    /// <summary>
    /// Common interface for object creator classes
    /// </summary>
    public interface IObjectCreator
    {
        object GetObject(IDataSource dataSource);
        IEnumerable<object> GetObjects(IDataSource dataSource);
    }
}
