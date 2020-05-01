using System.Collections.Generic;

namespace CSVToJson.DataSources
{
    /// <summary>
    /// Common base class for IDataSource implementations that read data from a CSV file
    /// </summary>
    public abstract class CSVDataSourceBase : IDataSource
    {
        ///////////////////////////////////////////////////////////////////////////
        // IDataSource interface
        public string[] OrderedFields { get; protected set; }

        public void Initialise(string fileName)
        {
            // Implementation specific call
            BaseInitialisation(fileName);

            ReadHeaders();
        }

        public abstract void Dispose();

        public object this[string fieldName]
        {
            get
            {
                return _currentLineValuesLookup[fieldName];
            }
        }

        public bool Read()
        {
            if (EOF)
            {
                return false;
            }

            var items = ReadLine();

            if (items.Length != OrderedFields.Length)
            {
                // e.g. whitespace at the end of the file
                return false;
            }

            _currentLineValuesLookup = new Dictionary<string, string>();

            for (var index = 0; index < items.Length; index++)
            {
                _currentLineValuesLookup[OrderedFields[index]] = items[index];
            }

            return true;
        }

        ///////////////////////////////////////////////////////////////////////////
        // Abstract members to be implemented by derived classes
        protected abstract void ReadHeaders();

        protected abstract string[] ReadLine();

        protected abstract bool EOF { get; }

        protected abstract void BaseInitialisation(string fileName);

        ///////////////////////////////////////////////////////////////////////////
        // Class members


        protected Dictionary<string, string> _currentLineValuesLookup = null;


        

    }
}
