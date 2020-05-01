using System;

namespace CSVToJson.Instantiation
{
    public enum CreatorType
    {
        Dynamic = 0,
        StronglyTyped = 1
    }

    /// <summary>
    /// Class faactory for different instantiation options
    /// </summary>
    public static class CreatorFactory
    {
        public static IObjectCreator Create<T>(CreatorType creatorType)
            where T : class, new()
        {
            switch(creatorType)
            {
                case CreatorType.Dynamic:
                    return new DynamicObjectCreator();

                case CreatorType.StronglyTyped:
                    return new ObjectCreator<T>();
            }
            
            throw new Exception("No implementation for " + creatorType);
        }
    }
}
