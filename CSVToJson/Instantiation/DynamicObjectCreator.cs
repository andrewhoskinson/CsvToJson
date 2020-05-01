using CSVToJson.DataSources;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace CSVToJson.Instantiation
{
    /// <summary>
    /// Creates dynamic objects (uses ExpandoObject as a holder), assumes '_' in a field name indicates
    /// a nested entity.
    /// </summary>
    public class DynamicObjectCreator : IObjectCreator
    {
        private string _prefix = null;
        protected int CurrentIndex { get; set; } = 0;

        /// <summary>
        /// Public constructor for top level creation
        /// </summary>
        public DynamicObjectCreator()
        {

        }

        /// <summary>
        /// Constructor that allows for pseudo-recursion
        /// </summary>
        private DynamicObjectCreator(string prefix, int currentIndex)
        {
            _prefix = prefix;
            CurrentIndex = currentIndex;
        }

        /// <summary>
        /// Returns a single object from the datasource. 
        /// </summary>
        public object GetObject(IDataSource dataSource)
        {
            var createdObject = new ExpandoObject() as IDictionary<string, object>;

            var index = CurrentIndex;

            while (index < dataSource.OrderedFields.Length)
            {
                var field = dataSource.OrderedFields[index];

                if (!string.IsNullOrEmpty(_prefix) && field.IndexOf(_prefix) != 0)
                {
                    // Finished this object
                    return createdObject;
                }

                if (!string.IsNullOrEmpty(_prefix))
                {
                    field = field.Substring(_prefix.Length, field.Length - _prefix.Length);
                }

                if (field.IndexOf("_") > -1)
                {
                    // Indicates that a new sub object should be created.

                    // Deconstruct the field name into an actual name, and a prefix for the sub fields
                    var parts = field.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    var subField = parts[0];
                    var prefix =  (!string.IsNullOrEmpty(_prefix) ? _prefix : "") + subField + "_";

                    // Essentially this is recursion
                    var creator = new DynamicObjectCreator(prefix, index);
                    createdObject.Add(subField, creator.GetObject(dataSource));

                    index = creator.CurrentIndex;
                }
                else
                {
                    // Simple property set
                    createdObject.Add(field, dataSource[dataSource.OrderedFields[index]]);
                    index++;
                }
            }

            CurrentIndex = index;
            return createdObject;
        }

        /// <summary>
        /// Top level entry point, gets a list of objects from the datasource
        /// </summary>
        public IEnumerable<object> GetObjects(IDataSource dataSource)
        {
            List<object> retval = new List<object>();

            while (dataSource.Read())
            {
                retval.Add(GetObject(dataSource));
            }

            return retval;
        }
    }
}
