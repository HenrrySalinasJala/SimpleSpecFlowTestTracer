namespace SimpleTracer.SpecFlowPlugin
{
    using BoDi;
    using NUnit.Framework.Internal;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Bindings;
    using TechTalk.SpecFlow.Bindings.Reflection;
    using TechTalk.SpecFlow.BindingSkeletons;
    using TechTalk.SpecFlow.Infrastructure;
    using TechTalk.SpecFlow.Plugins;
    using TechTalk.SpecFlow.Tracing;

    public class SimpleTracer : ITestTracer
    {
        private readonly ITraceListener traceListener;
        private readonly IStepFormatter stepFormatter;
        private readonly IStepDefinitionSkeletonProvider stepDefinitionSkeletonProvider;
        private readonly TechTalk.SpecFlow.Configuration.SpecFlowConfiguration specFlowConfiguration;

        public SimpleTracer(ITraceListener traceListener, IStepFormatter stepFormatter,
            IStepDefinitionSkeletonProvider stepDefinitionSkeletonProvider,
            TechTalk.SpecFlow.Configuration.SpecFlowConfiguration specFlowConfiguration)
        {
            this.traceListener = traceListener;
            this.stepFormatter = stepFormatter;
            this.stepDefinitionSkeletonProvider = stepDefinitionSkeletonProvider;
            this.specFlowConfiguration = specFlowConfiguration;
           
        }
        //public SimpleTracer(ScenarioContext scenarioContext)
        //{
        //    this.ScenarioContext = scenarioContext;
        //}

        public void TraceBindingError(BindingException ex)
        {
            Console.WriteLine("TraceBindingError");
            Console.WriteLine(ex.Message);
            traceListener.WriteToolOutput("binding error: {0}", ex.Message);

        }

        public void TraceDuration(TimeSpan elapsed, IBindingMethod method, object[] arguments)
        {
            Console.WriteLine("TraceDuration");
            Console.WriteLine(elapsed.TotalSeconds);
            traceListener.WriteToolOutput("duration: {0}: {1:F1}s",
                            stepFormatter.GetMatchText(method, arguments), elapsed.TotalSeconds);
        }

        public void TraceDuration(TimeSpan elapsed, string text)
        {
            Console.WriteLine("TraceDuration 2");
            Console.WriteLine(elapsed.TotalSeconds);
            traceListener.WriteToolOutput("duration: {0}: {1:F1}s", text, elapsed.TotalSeconds);
        }

        public void TraceStep(StepInstance stepInstance, bool showAdditionalArguments)
        {
            Console.WriteLine("TraceStep");
            var testFullName = TestExecutionContext.CurrentContext.CurrentTest.FullName;
           

            //testScenarioBuilder.SetFullName(testFullName)
            //                   .SetTitle(stepInstance.StepContext.ScenarioTitle);
            var currentTags = stepInstance.StepContext.Tags;
            //currentTags.ToList().ForEach(t => testScenarioBuilder.AddTag(t));
            string stepText = stepFormatter.GetStepText(stepInstance);
            //testScenarioBuilder.SetStep(stepInstance.Keyword, stepText);

            traceListener.WriteTestOutput(stepText.TrimEnd());
        }

        public void TraceStepDone(BindingMatch match, object[] arguments, TimeSpan duration)
        {
            Console.WriteLine("TraceStepDone");
            var testFullName = TestExecutionContext.CurrentContext.CurrentTest.FullName;
            //testScenarioBuilder.SetStepState("Passed");
            //testScenarioBuilder.SetStepDuration(Convert.ToString(duration.TotalSeconds));

            traceListener.WriteToolOutput("done: {0} ({1:F1}s)",
                            stepFormatter.GetMatchText(match, arguments), duration.TotalSeconds);
        }

        public void TraceStepPending(BindingMatch match, object[] arguments)
        {
            Console.WriteLine("TraceStepPending");

            var formattedSted = stepFormatter.GetMatchText(match, arguments);
            traceListener.WriteToolOutput("pending: {0}",
                            stepFormatter.GetMatchText(match, arguments));
            //testScenarioBuilder.SetStepState("Skipped");
        }

        public void TraceStepSkipped()
        {
            Console.WriteLine("---------------TraceStepSkipped------------");
            traceListener.WriteToolOutput("skipped because of previous errors");
            //testScenarioBuilder.SetStepState("Skipped");
            //Console.WriteLine("*******************************");
            //Console.WriteLine(testScenarioBuilder.Build().ToString());
            //Console.WriteLine("*******************************");
        }

        public void TraceWarning(string text)
        {
            Console.WriteLine("TraceWarning");
            traceListener.WriteToolOutput("warning: {0}", text);
            Console.WriteLine(text);
            //testScenarioBuilder.SetResult("Warning");
        }

        public void TraceError(Exception ex)
        {
            Console.WriteLine("----------------TraceError----------------");
            //Console.WriteLine("*******************************");
            //Console.WriteLine(testScenarioBuilder.Build().ToString());
            //Console.WriteLine("*******************************");
            WriteErrorMessage(ex.Message);
            WriteLoaderExceptionsIfAny(ex);
        }

        private void WriteLoaderExceptionsIfAny(Exception ex)
        {
            switch (ex)
            {
                case TypeInitializationException typeInitializationException:
                    WriteLoaderExceptionsIfAny(typeInitializationException.InnerException);
                    break;
                case ReflectionTypeLoadException reflectionTypeLoadException:
                    WriteErrorMessage("Type Loader exceptions:");
                    FormatLoaderExceptions(reflectionTypeLoadException);
                    break;
            }
        }


        private void FormatLoaderExceptions(ReflectionTypeLoadException reflectionTypeLoadException)
        {
            var exceptions = reflectionTypeLoadException.LoaderExceptions
                .Select(x => x.ToString())
                .Distinct()
                .Select(x => $"LoaderException: {x}");
            foreach (var ex in exceptions)
            {
                WriteErrorMessage(ex);
            }
        }

        private void WriteErrorMessage(string ex)
        {
            traceListener.WriteToolOutput("error: {0}", ex);
        }

        public void TraceNoMatchingStepDefinition(StepInstance stepInstance, ProgrammingLanguage targetLanguage, CultureInfo bindingCulture, List<BindingMatch> matchesWithoutScopeCheck)
        {
            Console.WriteLine("TraceNoMatchingStepDefinition");
            Console.WriteLine(stepInstance.Keyword);
            Console.WriteLine(stepInstance.Text);

            StringBuilder message = new StringBuilder();
            if (matchesWithoutScopeCheck == null || matchesWithoutScopeCheck.Count == 0)
                message.AppendLine("No matching step definition found for the step. Use the following code to create one:");
            else
            {
                string preMessage = string.Format("No matching step definition found for the step. There are matching step definitions, but none of them have matching scope for this step: {0}.",
                    string.Join(", ", matchesWithoutScopeCheck.Select(m => stepFormatter.GetMatchText(m, null)).ToArray()));
                traceListener.WriteToolOutput(preMessage);
                message.AppendLine("Change the scope or use the following code to create a new step definition:");
            }
            message.Append(
               stepDefinitionSkeletonProvider.GetStepDefinitionSkeleton(targetLanguage, stepInstance, specFlowConfiguration.StepDefinitionSkeletonStyle, bindingCulture)
                    .Indent(StepDefinitionSkeletonProvider.METHOD_INDENT));

            traceListener.WriteToolOutput(message.ToString());
        }

    }
}
