using System;
using System.IO;
using System.Text;

namespace CSVToJson.Output
{
    /// <summary>
    /// Outputs to XML format
    /// </summary>
    public class XmlOutput<T> : IOutput
    {
        public string Output(object inputObject)
        {
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(T[]));

            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms))
                {
                    x.Serialize(writer, inputObject);

                    writer.Flush();

                    return new UTF8Encoding().GetString(ms.ToArray());
                }
            }
        }
    }
}
