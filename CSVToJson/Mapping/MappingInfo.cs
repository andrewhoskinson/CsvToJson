using System.Reflection;

namespace CSVToJson.Mapping
{
    /// <summary>
    /// Information about a mapping between an entity property and a datasource column. 
    /// </summary>
    public class MappingInfo
    {
        public string ColumnName { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }

        public MappingInfo(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            ColumnName = propertyInfo.Name;
        }

        public MappingInfo Column(string columnName)
        {
            ColumnName = columnName;
            return this;
        }
    }
}
