using System;
using System.IO;

namespace CSVToJson.DataSources
{
    /// <summary>
    /// Very naive implementation, does not cope with quoted values etc. Anybody who has ever had to
    /// code a CSV parser to take input from various sources will know exactly what a headache this is
    /// to do properly... RFC 4180 has far too much wiggle room in it!
    /// </summary>
    public class NaiveCSVDataSource : CSVDataSourceBase
    {
        private StreamReader _reader;

        protected override void BaseInitialisation(string fileName)
        {
            _reader = new StreamReader(fileName);
        }

        protected override void ReadHeaders()
        {
            OrderedFields = ReadLine();
        }

        protected override string[] ReadLine()
        {
            return _reader.ReadLine().Split(new char[] { ',' }, StringSplitOptions.None);
        }

        public override void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
            }
        }

        protected override bool EOF
        {
            get
            {
                return _reader.EndOfStream;
            }
        }
    }
}
