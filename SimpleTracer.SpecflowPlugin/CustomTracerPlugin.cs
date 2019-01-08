using BoDi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.Bindings.Reflection;
using TechTalk.SpecFlow.Plugins;
using TechTalk.SpecFlow.Tracing;

[assembly: RuntimePlugin(typeof(SimpleTracer.SpecFlowPlugin.CustomTracerPlugin))]
namespace SimpleTracer.SpecFlowPlugin
{
    public class CustomTracerPlugin : IRuntimePlugin
    {
        public void Initialize(RuntimePluginEvents runtimePluginEvents, RuntimePluginParameters runtimePluginParameters)
        {
            runtimePluginEvents.CustomizeTestThreadDependencies += (sender, args) => 
            { args.ObjectContainer.RegisterTypeAs<SimpleTracer, ITestTracer>(); };
        }

        
    }
    
}
