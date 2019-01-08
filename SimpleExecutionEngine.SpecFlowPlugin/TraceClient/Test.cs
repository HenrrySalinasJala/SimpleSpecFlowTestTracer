using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SimpleExecutionEngine.SpecFlowPlugin.TraceClient.Client;
using System.Collections.Generic;

namespace SimpleExecutionEngine.SpecFlowPlugin.TraceClient
{
    public class Test
    {
        public Suite Suite { get; set; }
        public string TestId { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string TestDescription { get; set; }
        public string Duration { get; set; }
        public string Stack { get; set; }
        public string Result { get; set; }
        public string Screenshot { get; set; }
        public string Video { get; set; }
        public List<string> Tags { get; set; }
        public string ErrorMessage;
        public List<Step> Steps { get; set; }

        public Test(BaseTestBuilder builder)
        {
            Suite = new Suite();
            Suite.runId = builder.RunId;
            Suite.Name = builder.Suite;
            Suite.SuiteId = builder.SuiteId;
            Suite.Description = builder.SuiteDescription;
            TestId = builder.TestId;
            FullName = builder.FullName;
            Title = builder.Title;
            TestDescription = builder.TestDescription;
            Duration = builder.Duration;
            Stack = builder.Stack;
            Result = builder.Result;
            Screenshot = builder.Screenshot;
            Video = builder.Video;
            Steps = builder.Steps;
            ErrorMessage = builder.ErrorMessage;
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
