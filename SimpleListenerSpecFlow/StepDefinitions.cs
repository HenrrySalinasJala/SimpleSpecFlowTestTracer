using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using TechTalk.SpecFlow;

namespace SimpleListenerSpecFlow
{
    [Binding]
    public class StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public StepDefinitions(ScenarioContext scenarioContext)
        {
            this._scenarioContext = scenarioContext;
        }

        [StepArgumentTransformation]
        public Dictionary<string, string> transformtable(Table table)
        {
            return new Dictionary<string, string>();
        }

        [BeforeScenario]
        [AfterScenario]
        public void BeforeAfterHook()
        {
            Console.WriteLine("//////////////////// before/after");
        }

        [Given(@"Given step '(.*?)'")]
        public void Given(string p0)
        {
            Console.WriteLine($"given step parameter was {p0}");
            Thread.Sleep(500);
            Assert.True(true);
        }


        [When(@"When step '(.*?)'")]
        public void When(string p0)
        {
            Console.WriteLine($"step parameter was {p0}");
            Thread.Sleep(500);
            Assert.True(true);
        }

        [Then(@"Then step '(.*?)'")]
        public void Then(string p0)
        {
            Console.WriteLine($"step parameter was {p0}");
            Thread.Sleep(500);
            Assert.True(true);
        }

        [Then(@"Test failed")]
        public void Failed()
        {
            Thread.Sleep(1000);

            Assert.True(false);
        }

        [Then(@"Test error")]
        public void Error()
        {
            Thread.Sleep(1000);

            throw new Exception("automation error");
        }


        [Then(@"Test warning")]
        //[Obsolete("you should use another step instead")]
        public void Warning()
        {
            Thread.Sleep(1000);
            Assert.Warn("use another step instead");
        }

        [Then(@"Step with table")]
        public void ThenStepWithTable(Table table)
        {
            Assert.True(true);
        }

        [Then(@"Step with transformed")]
        public void ThenStepWithTransformed(Dictionary<string, string> table)
        {
            Assert.True(true);
        }

        [Then(@"Step with multiline text")]
        public void ThenStepWithMultilineText(string multilineText)
        {
            Assert.True(true);
        }

    }
}
