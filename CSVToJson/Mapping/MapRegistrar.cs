using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSVToJson.Mapping
{
    /// <summary>
    /// This class finds all the ClassMap<T> implementations and registers them.
    /// </summary>
    public static class MapRegistrar
    {
        public static Dictionary<Type, IClassMap> MappedClasses;
        
        static MapRegistrar()
        {
            MappedClasses = new Dictionary<Type, IClassMap>();
        }

        public static void Register(Assembly assembly)
        {
            var iClassMapType = typeof(IClassMap);

            foreach(var map in assembly.GetExportedTypes()
                .Where(x => x.IsClass && iClassMapType.IsAssignableFrom(x) && !x.IsAbstract))
            {
                var classMap = map.GetConstructor(Type.EmptyTypes).Invoke(null) as IClassMap;

                // Check that the type has not already been registered
                if (MappedClasses.ContainsKey(classMap.MappedType))
                {
                    throw new Exception("A ClassMap for " + classMap.MappedType.FullName + " has already been registered");
                }

                MappedClasses[classMap.MappedType] = classMap; 
            }
        }        
    }
}
