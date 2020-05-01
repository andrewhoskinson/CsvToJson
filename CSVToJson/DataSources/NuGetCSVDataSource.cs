using Csv; // NuGet CSV library
using System.IO;
using System.Linq;

namespace CSVToJson.DataSources
{
    /// <summary>
    /// Example of a datasource using a NuGet CSV library. 
    /// </summary>
    public class NuGetCSVDataSource : CSVDataSourceBase
    {        
        protected override void BaseInitialisation(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                _csvLines = CsvReader.Read(
                    reader,
                    new CsvOptions
                    {
                        HeaderMode = HeaderMode.HeaderPresent
                    }
                ).ToArray(); // The library doesn't seem to like leaving the IEnumerable attached to an open stream. As this
                // is an example only I'm not going to spend time debugging it.
            }
         }
            
        private ICsvLine[] _csvLines = null;
        private int _lineCounter = 0;

        protected override bool EOF
        {
            get
            {
                return _lineCounter >= _csvLines.Length;
            }
        }

        public override void Dispose()
        {
            // Nothing to do
        }

        protected override void ReadHeaders()
        {
            // With this CSV library, is HeaderMode.Present is used then the _csvLines only contain the data lines
            if (_csvLines.Length > 0)
            {
                OrderedFields = _csvLines[0].Headers;
            }
        }

        protected override string[] ReadLine()
        {
            return _csvLines[_lineCounter++].Values;
        }
    }
}
