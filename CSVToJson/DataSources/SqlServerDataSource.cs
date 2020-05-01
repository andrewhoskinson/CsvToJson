using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CSVToJson.DataSources
{
    /// <summary>
    /// An example of a datasource that doesn't use a CSV file - read in data from a Sql Server database
    /// </summary>
    public class SqlServerDataSource : IDataSource
    {
        public object this[string fieldName]
        {
            get
            {
                return _dataReader[fieldName];
            }
        }

        public string[] OrderedFields { get; private set; }

        public void Dispose()
        {
            if (_dataReader != null)
            {
                _dataReader.Dispose();
            }

            if (_dbConnection != null)
            {
                _dbConnection.Dispose();
            }
        }

        private IDbConnection _dbConnection = null;
        private IDataReader _dataReader = null;
        public void Initialise(string connectionString)
        {
            _dbConnection = new SqlConnection(connectionString);
            _dbConnection.Open();

            var cmd = _dbConnection.CreateCommand();
            cmd.CommandText = "select * from RawAddressView";
            _dataReader = cmd.ExecuteReader();

            OrderedFields = Enumerable.Range(0, _dataReader.FieldCount).Select(_dataReader.GetName).ToArray();
        }

        public bool Read()
        {
            return _dataReader.Read();
        }
    }
}
