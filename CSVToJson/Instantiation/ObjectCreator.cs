using CSVToJson.DataSources;
using CSVToJson.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSVToJson.Instantiation
{    
    /// <summary>
    /// Interface for strongly typed implementations of IObjectCreator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectCreator<T> : IObjectCreator
    {
        IEnumerable<T> GetStronglyTypedObjects(IDataSource dataSource);
    }

    /// <summary>
    /// Class that constructs an object based on a supplied Type
    /// </summary>
    public class TypeObjectCreator : IObjectCreator
    {   
        public Type Type { get; private set; }

        public TypeObjectCreator(Type type)
        {
            Type = type;
        }

        public object GetObject(IDataSource dataSource)
        {
            var classMap = MapRegistrar.MappedClasses[Type];

            var createdObject = Type.GetConstructor(Type.EmptyTypes).Invoke(null);

            foreach (var property in classMap.MappedProperties)
            {
                object dataValue = null;

                if (MapRegistrar.MappedClasses.ContainsKey(property.PropertyInfo.PropertyType))
                {
                    dataValue = new TypeObjectCreator(property.PropertyInfo.PropertyType).GetObject(dataSource);
                }
                else
                {
                    dataValue =
                        Convert.ChangeType(
                            dataSource[classMap.PrefixName + property.ColumnName],
                            property.PropertyInfo.PropertyType);
                }

                if (dataValue != null)
                {
                    property.PropertyInfo.SetValue(createdObject, dataValue);
                }
            }

            return createdObject;
        }

        public IEnumerable<object> GetObjects(IDataSource dataSource)
        {
            List<object> retval = new List<object>();

            var classMap = MapRegistrar.MappedClasses[Type];

            while(dataSource.Read())
            {
                var createdObject = Type.GetConstructor(Type.EmptyTypes).Invoke(null);

                foreach(var property in classMap.MappedProperties)
                {
                    object dataValue = null;

                    if (MapRegistrar.MappedClasses.ContainsKey(property.PropertyInfo.PropertyType))
                    {
                        dataValue = new TypeObjectCreator(property.PropertyInfo.PropertyType).GetObject(dataSource);
                    }
                    else
                    {
                        dataValue = dataSource[classMap.PrefixName + property.ColumnName];
                    }

                    if (dataValue != null)
                    {
                        property.PropertyInfo.SetValue(createdObject, dataValue);
                    }
                }

                retval.Add(createdObject);
            }

            return retval;
        }
    }

    /// <summary>
    /// Strongly typed overload of TypeObjectCreator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectCreator<T> : TypeObjectCreator, IObjectCreator<T>
        where T : class, new()
    {
        public ObjectCreator() : base(typeof(T))
        {

        }

        /// <summary>
        /// Helper for common usage
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public IEnumerable<T> GetStronglyTypedObjects(IDataSource dataSource)
        {
            return GetObjects(dataSource).Cast<T>();
        }
    }
}
