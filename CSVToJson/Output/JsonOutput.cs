namespace CSVToJson.Output
{

    /// <summary>
    /// Outputs to JSON format
    /// </summary>
    public class JsonOutput : IOutput
    {
        public string Output(object inputObject)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(inputObject);
        }
    }
}
