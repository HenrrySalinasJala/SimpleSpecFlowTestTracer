namespace SimpleTracer.SpecFlowPlugin.Test
{

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;

    public abstract class BaseTest
    {
        protected string BuilderResourceFolder { get; set; }

        protected JObject ReadJSONFile(string jsonFileName)
        {
            var json = new JObject();
            if (!string.IsNullOrEmpty(BuilderResourceFolder))
            {
                var jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, BuilderResourceFolder, jsonFileName);
                using (StreamReader reader = new StreamReader(jsonFilePath))
                {
                    string jsonString = reader.ReadToEnd();
                    json = JsonConvert.DeserializeObject<JObject>(jsonString);
                }

            }
            return json;
        }
    }
}
