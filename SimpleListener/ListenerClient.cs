namespace NUnit.Engine.Listeners
{
    using log4net;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Globalization;
    using System.IO;
    using System.Xml;

    internal class ListenerClient
    {
        /// <summary>
        /// The logger instance.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string TcParseServiceMessagesInside = "tc:parseServiceMessagesInside";
        private static readonly IEnumerable<ServiceMessage> EmptyServiceMessages = new ServiceMessage[0];

        public void SuiteStarted(EventId eventId)
        {
            log.Info("/**********************************************");
            log.Info("Suite started");
            log.Info("/**********************************************");
            var assemblyName = Path.GetFileName(eventId.FullName);

            JObject suiteStartedBody = JObject.FromObject(new
            {
                name = assemblyName,
                flow= eventId.FlowId,
                runState = "started"
            });
            log.Debug(suiteStartedBody.ToString());

        }

        public void SuiteFinished(EventId eventId)
        {
            log.Info("/**********************************************");
            log.Info("Suite Finished");
            log.Info("/**********************************************");
            var assemblyName = Path.GetFileName(eventId.FullName);

            JObject suiteFinishedBody = JObject.FromObject(new
            {
                name = assemblyName,
                runState = "finished"
            });
            log.Debug(suiteFinishedBody.ToString());
        }

        public IEnumerable<ServiceMessage> FlowStarted(string flowId, string parentFlowId)
        {
            log.Info("/**********************************************");
            log.Info("Flow started");
            log.Info("/**********************************************");
            yield return new ServiceMessage(ServiceMessage.Names.FlowStarted,
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Parent, parentFlowId));
        }

        public IEnumerable<ServiceMessage> FlowFinished(string flowId)
        {
            log.Info("/**********************************************");
            log.Info("Flow finished ");
            log.Info("/**********************************************");
            yield return new ServiceMessage(ServiceMessage.Names.FlowFinished,
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, flowId));
        }

        public void TestStarted(EventId eventId)
        {
            log.Info("/**********************************************");
            log.Info("Test Started");
            log.Info("/**********************************************");
            JObject testStartedBody = JObject.FromObject(new
            {
                fullName = eventId.FullName,
                runState = "started",
                flowId = eventId.FlowId
            });
            log.Debug(testStartedBody.ToString());
        }

        public void TestFinished(EventId eventId, XmlNode testEvent, XmlNode infoEvent)
        {
            log.Info("/**********************************************");
            log.Info("Test Finished getting result");
            log.Info("/**********************************************");

            var result = testEvent.GetAttribute("result");
            if (string.IsNullOrEmpty(result))
            {
                return;
            }
            dynamic testFinishedBody = new ExpandoObject();

            testFinishedBody.fullName = eventId.FullName;
            testFinishedBody.runState = "finished";
            testFinishedBody.flowId = eventId.FlowId;
            testFinishedBody.result = result;
            testFinishedBody.testResultProperties = new { };


            log.Info("/**********************************************");
            log.Info("RESULT: " + result);
            log.Info("/**********************************************");
            object testResultProperties;
            switch (result.ToLowerInvariant())
            {
                case "passed":
                    testResultProperties = TestFinished(eventId, testEvent);
                    break;

                case "inconclusive":
                    testResultProperties = TestInconclusive(eventId, testEvent);
                    break;

                case "skipped":
                    testResultProperties = TestSkipped(eventId, testEvent);
                    break;

                case "failed":
                    testResultProperties = TestFailed(eventId, testEvent, infoEvent);
                    break;

                default:
                    testResultProperties = EmptyServiceMessages;
                    break;
            }

            testFinishedBody.testResultProperties = testResultProperties;
            JObject testFinishedJSON = JObject.FromObject(testFinishedBody);
            log.Debug(testFinishedJSON.ToString());
        }

        private static void Write(TextWriter writer, ServiceMessageAttr attribute)
        {
            writer.Write(attribute.Name);
            writer.Write("='");
            writer.Write(EscapeString(attribute.Value));
            writer.Write('\'');
        }

        private static string EscapeString(string value)
        {
            return value != null
                ? value.Replace("|", "||")
                       .Replace("'", "|'")
                       .Replace("’", "|’")
                       .Replace("\n", "|n")
                       .Replace("\r", "|r")
                       .Replace(char.ConvertFromUtf32(int.Parse("0086", NumberStyles.HexNumber)), "|x")
                       .Replace(char.ConvertFromUtf32(int.Parse("2028", NumberStyles.HexNumber)), "|l")
                       .Replace(char.ConvertFromUtf32(int.Parse("2029", NumberStyles.HexNumber)), "|p")
                       .Replace("[", "|[")
                       .Replace("]", "|]")
                : null;
        }

        public IEnumerable<ServiceMessage> TestOutput(EventId eventId, XmlNode testEvent)
        {
            if (string.IsNullOrEmpty(eventId.FlowId))
            {
                yield break;
            }

            var stream = testEvent.GetAttribute("stream");
            IEnumerable<ServiceMessage> messages;
            if (!string.IsNullOrEmpty(stream) && stream.ToLower() == "error")
            {
                messages = Output(eventId, ServiceMessage.Names.TestStdErr, testEvent.InnerText);
            }
            else
            {
                messages = Output(eventId, ServiceMessage.Names.TestStdOut, testEvent.InnerText);
            }

            foreach (var message in messages)
            {
                yield return message;
            }
        }

        public IEnumerable<ServiceMessage> TestOutputAsMessage(EventId eventId, XmlNode testEvent)
        {
            if (testEvent == null) throw new ArgumentNullException("testEvent");

            var output = testEvent.SelectSingleNode("output");
            if (output == null)
            {
                yield break;
            }

            foreach (var message in OutputAsMessage(eventId, output.InnerText))
            {
                yield return message;
            }
        }

        private ExpandoObject TestFinished(EventId eventId, XmlNode testFinishedEvent)
        {
            log.Info("/**********************************************");
            log.Info("Test finished/passed");
            log.Info("/**********************************************");
            if (testFinishedEvent == null)
            {
                throw new ArgumentNullException("testFinishedEvent");
            }

            var durationStr = testFinishedEvent.GetAttribute(ServiceMessageAttr.Names.Duration);
            double durationDecimal;
            var durationMilliseconds = 0;
            if (durationStr != null && double.TryParse(durationStr, NumberStyles.Any, CultureInfo.InvariantCulture, out durationDecimal))
            {
                durationMilliseconds = (int)(durationDecimal * 1000d);
            }

            if (testFinishedEvent == null) throw new ArgumentNullException("sendOutputEvent");

            var output = testFinishedEvent.SelectSingleNode("output");

            var reason = string.Empty;
            if (testFinishedEvent == null) throw new ArgumentNullException("ev");

            var reasonMessageElement = testFinishedEvent.SelectSingleNode("reason/message");
            if (reasonMessageElement != null)
            {
                var reasonMessage = reasonMessageElement.InnerText;
                if (!string.IsNullOrEmpty(reasonMessage))
                {
                    reason = reasonMessage;
                }
            }

            dynamic testFinishedBody = new ExpandoObject();
            testFinishedBody.duration = durationMilliseconds;
            testFinishedBody.reason = reason;
            testFinishedBody.output = output != null ? output.InnerText : string.Empty;
            return testFinishedBody;
        }

        private ExpandoObject TestFailed(EventId eventId, XmlNode testFailedEvent, XmlNode infoSource)
        {
            log.Info("/**********************************************");
            log.Info("Test Failed");
            log.Info("/**********************************************");
            if (testFailedEvent == null)
            {
                throw new ArgumentNullException("testFailedEvent");
            }

            if (infoSource == null)
            {
                infoSource = testFailedEvent;
            }

            var errorMessage = infoSource.SelectSingleNode("failure/message");
            var stackTrace = infoSource.SelectSingleNode("failure/stack-trace");

            dynamic testFinishedBody = TestFinished(eventId, testFailedEvent);
            testFinishedBody.errorMessage = errorMessage == null ? string.Empty : errorMessage.InnerText;
            testFinishedBody.stackTrace = stackTrace == null ? string.Empty : stackTrace.InnerText;
            return testFinishedBody;
        }

        private static dynamic Merge(object item1, object item2)
        {
            IDictionary<string, object> result = new ExpandoObject();

            foreach (var property in item1.GetType().GetProperties())
            {
                if (property.CanRead)
                    result[property.Name] = property.GetValue(item1);
            }

            foreach (var property in item2.GetType().GetProperties())
            {
                if (property.CanRead)
                    result[property.Name] = property.GetValue(item2);
            }

            return result;
        }

        private IEnumerable<ServiceMessage> TestSkipped(EventId eventId, XmlNode testSkippedEvent)
        {
            log.Info("/**********************************************");
            log.Info("Test Skiped");
            log.Info("/**********************************************");
            if (testSkippedEvent == null)
            {
                throw new ArgumentNullException("testSkippedEvent");
            }

            foreach (var message in Output(eventId, testSkippedEvent))
            {
                yield return message;
            }

            var reason = testSkippedEvent.SelectSingleNode("reason/message");

            yield return new ServiceMessage(ServiceMessage.Names.TestIgnored,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, eventId.FullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Message, reason == null ? string.Empty : reason.InnerText),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId));
        }

        private IEnumerable<ServiceMessage> TestInconclusive(EventId eventId, XmlNode testInconclusiveEvent)
        {
            log.Info("/**********************************************");
            log.Info("Test Inconclusive");
            log.Info("/**********************************************");
            if (testInconclusiveEvent == null)
            {
                throw new ArgumentNullException("testInconclusiveEvent");
            }

            foreach (var message in Output(eventId, testInconclusiveEvent))
            {
                yield return message;
            }

            yield return new ServiceMessage(ServiceMessage.Names.TestIgnored,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, eventId.FullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Message, "Inconclusive"),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId));
        }

        private IEnumerable<ServiceMessage> Output(EventId eventId, string messageName, string outputStr)
        {

            if (string.IsNullOrEmpty(outputStr))
            {
                yield break;
            }

            yield return new ServiceMessage(messageName,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Name, eventId.FullName),
                new ServiceMessageAttr(ServiceMessageAttr.Names.Out, outputStr),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId),
                new ServiceMessageAttr(ServiceMessageAttr.Names.TcTags, TcParseServiceMessagesInside));
        }

        private IEnumerable<ServiceMessage> OutputAsMessage(EventId eventId, string outputStr)
        {
            if (string.IsNullOrEmpty(outputStr))
            {
                yield break;
            }

            yield return new ServiceMessage(ServiceMessage.Names.Message,
                new ServiceMessageAttr(ServiceMessageAttr.Names.Text, outputStr),
                new ServiceMessageAttr(ServiceMessageAttr.Names.FlowId, eventId.FlowId),
                new ServiceMessageAttr(ServiceMessageAttr.Names.TcTags, TcParseServiceMessagesInside));
        }

        private IEnumerable<ServiceMessage> Output(EventId eventId, XmlNode sendOutputEvent)
        {
            if (sendOutputEvent == null) throw new ArgumentNullException("sendOutputEvent");

            var output = sendOutputEvent.SelectSingleNode("output");
            if (output == null)
            {
                yield break;
            }

            foreach (var message in Output(eventId, ServiceMessage.Names.TestStdOut, output.InnerText))
            {
                yield return message;
            }
        }

        private IEnumerable<ServiceMessage> ReasonMessage(EventId eventId, XmlNode ev)
        {
            if (ev == null) throw new ArgumentNullException("ev");

            var reasonMessageElement = ev.SelectSingleNode("reason/message");
            if (reasonMessageElement == null)
            {
                yield break;
            }

            var reasonMessage = reasonMessageElement.InnerText;
            if (string.IsNullOrEmpty(reasonMessage))
            {
                yield break;
            }

            foreach (var message in Output(eventId, ServiceMessage.Names.TestStdOut, "Assert.Pass message: " + reasonMessage))
            {
                yield return message;
            }
        }
    }
}
