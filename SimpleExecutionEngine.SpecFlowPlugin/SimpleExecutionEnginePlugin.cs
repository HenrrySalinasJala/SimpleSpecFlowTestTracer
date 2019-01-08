using TechTalk.SpecFlow.Infrastructure;
using TechTalk.SpecFlow.Plugins;

[assembly: RuntimePlugin(typeof(SimpleExecutionEngine.SpecFlowPlugin.SimpleExecutionEnginePlugin))]
namespace SimpleExecutionEngine.SpecFlowPlugin
{
    public class SimpleExecutionEnginePlugin : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters)
        {
            runtimePluginEvents.CustomizeTestThreadDependencies += (sender, args) =>
                args.ObjectContainer.RegisterTypeAs<SimpleExecutionEngine, ITestExecutionEngine>();
        }
    }
}
