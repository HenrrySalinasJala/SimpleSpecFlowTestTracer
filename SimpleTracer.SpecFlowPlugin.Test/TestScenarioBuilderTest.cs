
namespace SimpleTracer.SpecFlowPlugin.Test
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using TechTalk.SpecFlow.Bindings;
    using TraceClient;

    [TestFixture]
    public class TestScenarioBuilderTest : BaseTest
    {
        [SetUp]
        public void Initialize()
        {
            BuilderResourceFolder = "Resources";
        }

        [Test]
        public void Test_TestScenarioBuilder_ShouldBePossibleToBuildAnScenarioWithFullNameOnly()
        {
            TestScenarioBuilder scenarioBuilder = new TestScenarioBuilder();
            const string expectedFullName = "test.assembly1.feature.test1";
            scenarioBuilder.SetFullName(expectedFullName);
            scenarioBuilder.SetFullName("fullName2");
            var scenario = scenarioBuilder.Build();

            Assert.AreEqual(expectedFullName, scenario.FullName);
        }

        [Test]
        public void Test_TestScenarioBuilder_ShouldBePossibleToBuildAnScenarioWithFullValues()
        {
            TestScenarioBuilder scenarioBuilder = new TestScenarioBuilder();
            const string expectedFullName = "test.assembly1.feature.test1";
            const string expectedTitle = "test title";
            const string expectedDuration = "500";
            const string expectedStack = "Exception";
            const string expectedResult = "passed";
            const string expectedScreenshot = "base64;djdunu9askmkm01askckjnk";
            const string expectedVideo = "C:\\test\\test.mp4";

            scenarioBuilder.SetFullName(expectedFullName);
            scenarioBuilder.SetTitle(expectedTitle);
            scenarioBuilder.SetDuration(expectedDuration);
            scenarioBuilder.SetStackTrace(expectedStack);
            scenarioBuilder.SetResult(expectedResult);
            scenarioBuilder.SetScreenshot(expectedScreenshot);
            scenarioBuilder.SetVideo(expectedVideo);

            var scenario = scenarioBuilder.Build();

            Assert.AreEqual(expectedFullName, scenario.FullName);
            Assert.AreEqual(expectedTitle, scenario.Title);
            Assert.AreEqual(expectedDuration, scenario.Duration);
            Assert.AreEqual(expectedStack, scenario.Stack);
            Assert.AreEqual(expectedResult, scenario.Result);
            Assert.AreEqual(expectedScreenshot, scenario.Screenshot);
            Assert.AreEqual(expectedVideo, scenario.Video);
        }

        [Test]
        public void Test_TestScenarioBuilder_ShouldBePossibleToSetScenarioResult()
        {
            TestScenarioBuilder scenarioBuilder = new TestScenarioBuilder();
            const string expectedResult = "Passed";
            scenarioBuilder.SetResult(expectedResult);
            const string expectedStepText = "step one 'parameter'";
            scenarioBuilder.SetStep(expectedStepText);
            scenarioBuilder.SetStepState("done");
            var scenario = scenarioBuilder.Build();

            Assert.AreEqual(expectedResult, scenario.Result);
        }

        [Test]
        public void Test_TestScenarioBuilder_ShouldBePossibleToBuildAJSONPayload()
        {
            TestScenarioBuilder scenarioBuilder = new TestScenarioBuilder();
            const string expectedFullName = "test.assembly1.feature.test1";
            const string expectedTitle = "test title";
            const string expectedDuration = "500";
            const string expectedStack = "Exception";
            const string expectedResult = "passed";
            const string expectedScreenshot = "base64;djdunu9askmkm01askckjnk";
            const string expectedVideo = "C:\\test\\test.mp4";
            var expectedTags = new List<string>() { "tag1", "tag2" };
            scenarioBuilder.SetFullName(expectedFullName);
            scenarioBuilder.SetTitle(expectedTitle);
            scenarioBuilder.SetDuration(expectedDuration);
            scenarioBuilder.SetStackTrace(expectedStack);
            scenarioBuilder.SetResult(expectedResult);
            scenarioBuilder.SetScreenshot(expectedScreenshot);
            scenarioBuilder.SetVideo(expectedVideo);
            expectedTags.ForEach(tag => scenarioBuilder.AddTag(tag));
            var scenario = scenarioBuilder.Build();
            var actualJson = scenario.ToString();
            var expectedJson = ReadJSONFile("expectedTestScenarioWithFullFields.json");

            Assert.AreEqual(expectedJson.ToString(), actualJson,
               "Payload content is different");
        }

        [Test]
        public void Test_TestScenarioBuilder_ShouldBePossibleToBuildAJSONPayloadWithSteps()
        {
            TestScenarioBuilder scenarioBuilder = new TestScenarioBuilder();
            const string expectedFullName = "test.assembly1.feature.test1";
            const string expectedTitle = "test title";
            const string expectedDuration = "500";
            const string expectedStack = "Exception";
            const string expectedResult = "passed";
            const string expectedScreenshot = "base64;djdunu9askmkm01askckjnk";
            const string expectedVideo = "C:\\test\\test.mp4";

            var expectedTags = new List<string>() { "tag1", "tag2" };

            scenarioBuilder.SetFullName(expectedFullName);
            scenarioBuilder.SetTitle(expectedTitle);
            scenarioBuilder.SetDuration(expectedDuration);
            scenarioBuilder.SetStackTrace(expectedStack);
            scenarioBuilder.SetResult(expectedResult);
            scenarioBuilder.SetScreenshot(expectedScreenshot);
            scenarioBuilder.SetVideo(expectedVideo);
            expectedTags.ForEach(tag => scenarioBuilder.AddTag(tag));
            scenarioBuilder.SetStep("Given","I send a request test");
            scenarioBuilder.SetStepState("Passed");
            var duration = TimeSpan.FromMilliseconds(500);
            scenarioBuilder.SetStepDuration(Convert.ToString(duration.TotalSeconds));
            var scenario = scenarioBuilder.Build();
            var actualJson = scenario.ToString();
            var expectedJson = ReadJSONFile("expectedTestScenarioWithSteps.json");

            Assert.AreEqual(expectedJson.ToString(), actualJson,
               "Payload content is different");
        }
    }
}
