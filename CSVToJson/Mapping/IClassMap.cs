using System;
using System.Collections.Generic;

namespace CSVToJson.Mapping
{
    /// <summary>
    /// Interface that defines properties exposed by ClassMap classes
    /// </summary>
    public interface IClassMap
    {
        Type MappedType { get; }
        List<MappingInfo> MappedProperties { get; }
        string PrefixName { get; }
    }
}
