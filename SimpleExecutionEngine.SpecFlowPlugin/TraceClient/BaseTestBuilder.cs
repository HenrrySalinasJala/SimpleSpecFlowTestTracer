using log4net;
using System.Collections.Generic;

namespace SimpleExecutionEngine.SpecFlowPlugin.TraceClient
{
    public abstract class BaseTestBuilder
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string RunId;
        public string Suite;
        public string SuiteId;
        public string SuiteDescription;
        public string TestId;
        public string FullName;
        public string Title;
        public string TestDescription;
        public string Duration;
        public string Stack;
        public string Result;
        public string Screenshot;
        public string Video;
        public string ErrorMessage;
        public List<Step> Steps;
        public List<string> Tags;

        public Test Build()
        {
            return new Test(this);
        }
    }
}
