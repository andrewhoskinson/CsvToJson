using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace CSVToJson.Mapping
{
    /// <summary>
    /// ClassMap base class. Handles mapping of entity properties and input columns
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ClassMap<T> : IClassMap
        where T : class, new()
    {
        public string PrefixName { get; private set; }
        public List<MappingInfo> MappedProperties { get; set; } = new List<MappingInfo>();

        public Type MappedType
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// Register a class property as coming from the input datasource
        /// </summary>
        protected MappingInfo Map(Expression<Func<T, object>> propertyLambda)
        {
            var property = Extract(propertyLambda);

            var retval = new MappingInfo(property);
            
            MappedProperties.Add(retval);

            return retval;
        }

        /// <summary>
        /// Register a column name prefix for this mapping
        /// </summary>
        protected void Prefix(string prefixName)
        {
            PrefixName = prefixName;
        }

        /// <summary>
        /// Helper that extracts a PropertyInfo from the Linq Expression passed to the Map function.
        /// </summary>
        private PropertyInfo Extract(Expression<Func<T, object>> propertyLambda)
        {
            MemberExpression member = (propertyLambda.Body.NodeType == ExpressionType.Convert ? (propertyLambda.Body as UnaryExpression).Operand : propertyLambda.Body) as MemberExpression;

            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            Type type = typeof(T);

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType) &&
                !propInfo.ReflectedType.IsAssignableFrom(type)
                )
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a property that is not from type {1}.",
                    propertyLambda.ToString(),
                    type));

            return propInfo;
        }
    }
}
