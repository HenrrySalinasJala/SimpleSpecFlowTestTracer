using System;
using System.Collections.Generic;

namespace SimpleTracer.SpecFlowPlugin.TraceClient
{
    public abstract class BaseTestBuilder
    {
       // protected TestScenario testScenario;

        public string Feature;
        public string FullName;
        public string Title;
        public string Duration;
        public string Stack;
        public string Result;
        public string Screenshot;
        public string Video;
        public string ErrorMessage;
        public List<Step> Steps;
        public List<string> Tags;

        protected TestScenario Build()
        {
            return new TestScenario(this);
        }
    }
}
