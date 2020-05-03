using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CSVToJson.Output
{
    public class ExpandoXmlOutput<T> : IOutput
    {        
        public string Output(object inputObject)
        {
            var iEnumerable = (IEnumerable)inputObject;

            var objects = iEnumerable.Cast<object>();
            if (objects.Count() == 0)
                return "";

            if (objects.First() is ExpandoObject)            
            {
                XDocument retval = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

                // Make these the same as the generic XmlSerializer outpu
                XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                XNamespace xsd = "http://www.w3.org/2001/XMLSchema";

                var element = new XElement("ArrayOf" + typeof(T).Name,
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XAttribute(XNamespace.Xmlns + "xsd", xsd));

                var expandos = iEnumerable.Cast<ExpandoObject>();

                foreach(var expando in expandos)
                {
                    element.Add(OutputExpando(expando));
                }

                retval.Add(element);

                // Just calling .ToString() on XDocument omits the Xml declaration, so explicitly output it using a streamwriter
                using (var ms = new MemoryStream())
                {
                    using (var writer = new StreamWriter(ms))
                    {
                        retval.Save(writer);
                    }

                    return new UTF8Encoding().GetString(ms.ToArray());
                }
            }            
            else
            {
                return new XmlOutput<T>().Output(iEnumerable.Cast<T>().ToArray());
            }
        }

        private XElement OutputExpando(ExpandoObject expando, string explicitName = null)
        {
            XElement element = new XElement(explicitName ?? typeof(T).Name);

            var asDict = expando as IDictionary<string, object>;

            foreach(var key in asDict.Keys)
            {
                var val = asDict[key];

                if (val == null)
                    continue;

                if (val.GetType() == typeof(ExpandoObject))
                {
                    element.Add(OutputExpando(val as ExpandoObject, key));
                }
                else
                {
                    XElement child = new XElement(key);
                    child.Value = val.ToString();
                    element.Add(child);
                }
            }

            return element;
        }
    }
}
