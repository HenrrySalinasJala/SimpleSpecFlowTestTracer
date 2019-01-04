namespace SimpleTracer.SpecFlowPlugin.TraceClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TestScenarioBuilder : BaseTestBuilder, ITestBuilderPlan
    {
        public TestScenarioBuilder()
        {
            Steps = new List<Step>();
            Tags = new List<string>();
            Tags.Add("All");
        }

        public ITestBuilderPlan SetFullName(string fullName)
        {
            if (FullName!=null && !FullName.Equals(fullName, StringComparison.InvariantCultureIgnoreCase))
            {
                StartBuildingNewScenario();
            }
            if (string.IsNullOrEmpty(FullName))
            {
                Console.WriteLine("********** STart Test");
                FullName = fullName;
            }

            return this;
        }

        private void StartBuildingNewScenario()
        {
            Console.WriteLine("********** END TEST*****");
            var currentScenario = this.Build();
            Console.WriteLine("*************JSON PAYLOAD******************");
            Console.WriteLine(currentScenario.ToString());
            Console.WriteLine("*******************************");

            FullName = null;
            Title = null;
            Duration = null;
            Stack = null;
            Result = null;
            Screenshot = null;
            Video = null;
            ErrorMessage = null;
            Steps = new List<Step>();
            Tags = new List<string>();
            Tags.Add("All");
        }

        public ITestBuilderPlan SetTitle(string title)
        {
            if (string.IsNullOrEmpty(Title))
            {

                Title = title;
            }
            return this;
        }

        public ITestBuilderPlan SetResult(string result)
        {
            if (string.IsNullOrEmpty(Result))
            {

                Result = result;
            }
            return this;
        }

        public ITestBuilderPlan SetErrorMessage(string errorMessage)
        {
            if (string.IsNullOrEmpty(ErrorMessage))
            {

                ErrorMessage = errorMessage;
            }
            return this;
        }

        public ITestBuilderPlan SetDuration(string expectedDuration)
        {
            if (string.IsNullOrEmpty(Duration))
            {

                Duration = expectedDuration;
            }
            return this;
        }

        public ITestBuilderPlan SetStackTrace(string expectedStack)
        {
            if (string.IsNullOrEmpty(Stack))
            {

                Stack = expectedStack;
            }
            return this;
        }

        public ITestBuilderPlan SetScreenshot(string expectedScreenshot)
        {
            if (string.IsNullOrEmpty(Screenshot))
            {

                Screenshot = expectedScreenshot;
            }

            return this;
        }

        public ITestBuilderPlan SetVideo(string expectedVideo)
        {
            if (string.IsNullOrEmpty(Video))
            {

                Video = expectedVideo;
            }

            return this;
        }

        public ITestBuilderPlan SetStep(string formattedStep)
        {
            return SetStep(string.Empty, formattedStep);
        }

        public ITestBuilderPlan SetStep(string keyword, string formattedStep)
        {
            if (Steps != null)
            {
                var step = new Step();
                step.Keyword = keyword;
                step.Text = formattedStep;
                Steps.Add(step);
            }

            return this;
        }

        public ITestBuilderPlan SetStepState(string state)
        {
            if (Steps.Count > 0)
            {
                var step = Steps.Last();
                step.State = state;
            }

            return this;
        }

        public void SetStepDuration(string duration)
        {
            if (Steps.Count > 0)
            {
                var step = Steps.Last();
                step.Duration = duration;
            }
        }

        public ITestBuilderPlan AddTag(string tag)
        {
            if (Tags != null && !contains(tag))
            {

                Tags.Add(tag);
            }

            return this;
        }

        public bool contains(string tagToFind)
        {
            return Tags.Any(tag => tag.Equals(tagToFind, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
