using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace SimpleTracer.SpecFlowPlugin.TraceClient
{
    public class TestScenario
    {
        public string Feature { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Stack { get; set; }
        public string Result { get; set; }
        public string Screenshot { get; set; }
        public string Video { get; set; }
        public List<Step> Steps { get; set; }
        public List<string> Tags { get; set; }

        public TestScenario(BaseTestBuilder builder)
        {
            Feature = builder.Feature;
            FullName = builder.FullName;
            Title = builder.Title;
            Duration = builder.Duration;
            Stack = builder.Stack;
            Result = builder.Result;
            Screenshot = builder.Screenshot;
            Video = builder.Video;
            Steps = builder.Steps;
            Tags = builder.Tags;
        }

        public override string ToString()
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            var json = JsonConvert.SerializeObject(this, jsonSerializerSettings);
            return json;
        }
    }
}
