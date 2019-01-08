namespace SimpleExecutionEngine.SpecFlowPlugin.TraceClient
{
    using global::SimpleExecutionEngine.SpecFlowPlugin.TraceClient.Client;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using TechTalk.SpecFlow;

    public class NUnitTestBuilder : BaseTestBuilder, ITestBuilderPlan
    {
        private readonly Guid _testRunId;

        public NUnitTestBuilder()
        {
            _testRunId = Guid.NewGuid();
            Steps = new List<Step>();
            Tags = new List<string>();
            Tags.Add("All");
        }

        public ITestBuilderPlan SetRunId()
        {
            if (string.IsNullOrEmpty(RunId))
            {
                RunId = _testRunId.ToString();
                Console.WriteLine($"setting run id {RunId}");
            }

            return this;
        }

        public ITestBuilderPlan SetSuite(string feature)
        {
            if (string.IsNullOrEmpty(Suite))
            {
                Suite = feature;
                SuiteId = Guid.NewGuid().ToString();
            }

            return this;
        }

        public ITestBuilderPlan SetSuiteDescription(string description)
        {
            if (string.IsNullOrEmpty(SuiteDescription))
            {
                SuiteDescription = description;
            }

            return this;
        }

        public ITestBuilderPlan SetFullName()
        {
            var fullName = TestContext.CurrentContext.Test.FullName;
            return SetFullName(fullName);
        }

        public ITestBuilderPlan SetFullName(string fullName)
        {
            if (string.IsNullOrEmpty(FullName))
            {
                TestId = Guid.NewGuid().ToString();
                FullName = fullName;
            }

            return this;
        }

        public void CleanUpTestScenarioBuilder()
        {
            TestId = null;
            FullName = null;
            Title = null;
            TestDescription = null;
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

        public void CleanUpSuiteInTestBuilder()
        {
            Suite = null;
            SuiteDescription = null;
        }

        public ITestBuilderPlan SetTitle(string title)
        {
            if (string.IsNullOrEmpty(Title))
            {

                Title = title;
            }
            return this;
        }

        public ITestBuilderPlan SetTestDescription(string description)
        {
            if (string.IsNullOrEmpty(TestDescription))
            {
                TestDescription = description;
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

        public ITestBuilderPlan SetResult(Stopwatch ScenarioWatch)
        {
            var status = GetTestStatus();
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;
            var message = TestContext.CurrentContext.Result.Message;
            var duration = TimeSpan.FromMilliseconds(ScenarioWatch.ElapsedMilliseconds);
            SetResult(status.ToString());
            SetErrorMessage(message);
            SetStackTrace(stackTrace);
            SetDuration(Convert.ToString(duration.TotalSeconds));
            UpdateTest();
            return this;
        }

        public ITestBuilderPlan InitTest(string scenarioTitle, IEnumerable<string> allTags)
        {
            SetFullName();
            SetTitle(scenarioTitle);
            allTags.ToList()
                .ForEach(tag => AddTag(tag));
            CreateTest();
            return this;
        }

        /// <summary>
        /// Gets the current test status.
        /// </summary>
        /// <returns>The test Status.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static TestStatus GetTestStatus()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            TestStatus logstatus;
            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = TestStatus.Failed;
                    break;
                case TestStatus.Inconclusive:
                    logstatus = TestStatus.Inconclusive;
                    break;
                case TestStatus.Warning:
                    logstatus = TestStatus.Warning;
                    break;
                case TestStatus.Skipped:
                    logstatus = TestStatus.Skipped;
                    break;
                default:
                    logstatus = TestStatus.Passed;
                    break;
            }

            return logstatus;
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

        public ITestBuilderPlan SetSkippedStep(bool isStepSkipped, Stopwatch StepWatch)
        {
            if (isStepSkipped)
            {
                StepWatch.Stop();
                SetStepState(TestStatus.Skipped.ToString());
                var duration = TimeSpan.FromMilliseconds(StepWatch.ElapsedMilliseconds);
                SetStepDuration(Convert.ToString(duration.TotalSeconds));
            }
            return this;
        }

        public ITestBuilderPlan SetStep(string keyword, string text)
        {
            if (Steps != null)
            {
                var step = new Step();
                step.Keyword = keyword;
                step.Text = text;
                Steps.Add(step);
            }

            return this;
        }


        public ITestBuilderPlan SetStep(string keyword, string text, string multilineTextArg, Table tableArg)
        {
            if (Steps != null)
            {
                var step = new Step();
                step.Keyword = keyword;
                step.Text = text;
                step.MultilineText = multilineTextArg;
                step.Table = tableArg != null ? ToArray(tableArg) : null;
                Steps.Add(step);
                UpdateTest();
            }

            return this;
        }

        /// <summary>
        /// Transforms the data table to a bi dimensional array.
        /// </summary>
        /// <param name="table">The data table.</param>
        /// <returns>Bi Dimensional array of strings.</returns>
        private static string[][] ToArray(Table table)
        {
            try
            {
                string[][] arrayResult = new string[table.Rows.Count + 1][];

                for (int i = 0; i <= table.RowCount; i++)
                {
                    if (i == 0)
                    {
                        arrayResult[i] = table.Header.ToArray();
                    }
                    else
                    {
                        arrayResult[i] = table.Rows[i - 1].Values.ToArray();
                    }
                }

                return arrayResult;
            }
            catch (Exception error)
            {
                Console.WriteLine($"Unable to Create Table strings {error.Message}");
                return new string[0][];
            }
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

        public ITestBuilderPlan SetStepState(bool isStepFailed, Stopwatch StepWatch)
        {
            var currentState = isStepFailed ? TestStatus.Failed.ToString() : TestStatus.Passed.ToString();
            var duration = TimeSpan.FromMilliseconds(StepWatch.ElapsedMilliseconds);
            SetStepState(currentState);
            SetStepDuration(Convert.ToString(duration.TotalSeconds));

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

        private void UpdateTest()
        {
            try
            {
                RequestUtil.Put($"/api/Tests/{TestId}", Build().ToString());
            }
            catch (Exception error)
            {
                log.Fatal($"Unable to update test from client {error.Message}");
            }
        }

        private void CreateTest()
        {
            try
            {
                RequestUtil.Post("/api/Tests", Build().ToString());
            }
            catch (Exception error)
            {
                log.Fatal($"Unable to create test from client {error.Message}");
            }
        }
    }
}
