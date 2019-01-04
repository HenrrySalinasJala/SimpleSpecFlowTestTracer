using NUnit.Framework;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace SimpleListenerSpecFlow
{
    [Binding]
    public class StepDefinitions
    {
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
            //try
            //{
            Thread.Sleep(1000);
            Assert.Warn("use another step instead");
            //}
            //catch (Exception error)
            //{
            //    Console.WriteLine($"test case with warning message {error.Message}");
            //}
        }

    }
}
