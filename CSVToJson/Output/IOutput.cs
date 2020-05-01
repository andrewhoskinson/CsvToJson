using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVToJson.Output
{
    /// <summary>
    /// Interface for difference output types
    /// </summary>
    public interface IOutput
    {
        string Output(object inputObject);
    }
}
