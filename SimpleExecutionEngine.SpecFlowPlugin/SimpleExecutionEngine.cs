using BoDi;
using SimpleExecutionEngine.SpecFlowPlugin.TraceClient;
using System;
namespace SimpleExecutionEngine.SpecFlowPlugin
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Bindings;
    using TechTalk.SpecFlow.BindingSkeletons;
    using TechTalk.SpecFlow.ErrorHandling;
    using TechTalk.SpecFlow.Infrastructure;
    using TechTalk.SpecFlow.Tracing;
    using TechTalk.SpecFlow.UnitTestProvider;

    public class SimpleExecutionEngine : TestExecutionEngine, ITestExecutionEngine
    {
        public Stopwatch StepWatch;
        public Stopwatch ScenarioWatch;
        private readonly NUnitTestBuilder testScenarioBuilder;
        private readonly IContextManager contextManager;

        public SimpleExecutionEngine(IStepFormatter stepFormatter, ITestTracer testTracer, IErrorProvider errorProvider, IStepArgumentTypeConverter stepArgumentTypeConverter,
            TechTalk.SpecFlow.Configuration.SpecFlowConfiguration specFlowConfiguration, IBindingRegistry bindingRegistry, IUnitTestRuntimeProvider unitTestRuntimeProvider,
            IStepDefinitionSkeletonProvider stepDefinitionSkeletonProvider, IContextManager contextManager, IStepDefinitionMatchService stepDefinitionMatchService,
            IDictionary<string, IStepErrorHandler> stepErrorHandlers, IBindingInvoker bindingInvoker, ITestObjectResolver testObjectResolver = null, IObjectContainer testThreadContainer = null) //TODO: find a better way to access the container
            : base(stepFormatter, testTracer, errorProvider, stepArgumentTypeConverter, specFlowConfiguration, bindingRegistry,
                 unitTestRuntimeProvider, stepDefinitionSkeletonProvider, contextManager, stepDefinitionMatchService, stepErrorHandlers,
                 bindingInvoker, testObjectResolver, testThreadContainer)
        {
            this.contextManager = contextManager;
            StepWatch = new Stopwatch();
            ScenarioWatch = new Stopwatch();
            testScenarioBuilder = new NUnitTestBuilder();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnTestRunStart()
        {
            base.OnTestRunStart();
            testScenarioBuilder.SetRunId();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnTestRunEnd()
        {
            base.OnTestRunEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="featureInfo"></param>
        public new void OnFeatureStart(FeatureInfo featureInfo)
        {
            base.OnFeatureStart(featureInfo);
            testScenarioBuilder.SetSuite(featureInfo.Title);
            testScenarioBuilder.SetSuiteDescription(featureInfo.Description);
        }
        /// <summary>
        /// 
        /// </summary>
        public new void OnFeatureEnd()
        {
            base.OnFeatureEnd();
            testScenarioBuilder.CleanUpSuiteInTestBuilder();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scenarioInfo"></param>
        public new void OnScenarioStart(ScenarioInfo scenarioInfo)
        {
            testScenarioBuilder.CleanUpTestScenarioBuilder();
            base.OnScenarioStart(scenarioInfo);
            var allTags = Enumerable.Union(scenarioInfo.Tags, this.contextManager
                                                                  .FeatureContext
                                                                  .FeatureInfo
                                                                  .Tags).ToList();
            //testScenarioBuilder.SetFullName()
            //                   .SetTitle(scenarioTitle);
            //allTags.ToList()
            //    .ForEach(tag => testScenarioBuilder.AddTag(tag));

            testScenarioBuilder.InitTest(scenarioInfo.Title, allTags);
            ScenarioWatch.Restart();
        }

        public new void OnAfterLastStep()
        {
            base.OnAfterLastStep();
        }

        /// <summary>
        /// 
        /// </summary>
        public new void OnScenarioEnd()
        {
            ScenarioWatch.Stop();
            base.OnScenarioEnd();

            testScenarioBuilder.SetResult(ScenarioWatch);
            var scenario = testScenarioBuilder.Build();

            Console.WriteLine("******JSON****");
            Console.WriteLine(scenario.ToString());
        }

        protected override void OnStepStart()
        {
            StepWatch.Restart();
            base.OnStepStart();
        }

        protected override void OnStepEnd()
        {
            StepWatch.Stop();
            base.OnStepEnd();
            var isStepFailed = this.contextManager.ScenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError;
            testScenarioBuilder.SetStepState(isStepFailed, StepWatch);
        }

        #region Given-When-Then

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stepDefinitionKeyword"></param>
        /// <param name="keyword"></param>
        /// <param name="text"></param>
        /// <param name="multilineTextArg"></param>
        /// <param name="tableArg"></param>
        public new void Step(StepDefinitionKeyword stepDefinitionKeyword, string keyword, string text, string multilineTextArg, Table tableArg)
        {
            testScenarioBuilder.SetStep(keyword, text, multilineTextArg, tableArg);
            bool isStepSkipped = this.contextManager.ScenarioContext.ScenarioExecutionStatus != ScenarioExecutionStatus.OK;
            testScenarioBuilder.SetSkippedStep(isStepSkipped, StepWatch);
            base.Step(stepDefinitionKeyword, keyword, text, multilineTextArg, tableArg);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public new void Pending()
        {
            base.Pending();
        }
    }
}