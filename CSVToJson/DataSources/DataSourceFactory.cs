using System;

namespace CSVToJson.DataSources
{
    public enum DataSourceType
    {
        NaiveCSVFile = 0,
        NugetCSVFile = 1,
        SqlServerDataSource = 2
    }

    /// <summary>
    /// Factory for IDataSource implementations
    /// </summary>
    public static class DataSourceFactory
    {
        public static IDataSource Create(DataSourceType dataSourceType)
        {
            switch(dataSourceType)
            {
                case DataSourceType.NaiveCSVFile:
                    return new NaiveCSVDataSource();

                case DataSourceType.NugetCSVFile:
                    return new NuGetCSVDataSource();

                case DataSourceType.SqlServerDataSource:
                    return new SqlServerDataSource();
            }

            throw new Exception("No implementation for" + dataSourceType);
        }
    }
}
