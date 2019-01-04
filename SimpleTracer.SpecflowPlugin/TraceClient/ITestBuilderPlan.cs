using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTracer.SpecFlowPlugin.TraceClient
{
    public interface ITestBuilderPlan
    {
        ITestBuilderPlan SetFullName(string fullName);
        ITestBuilderPlan SetTitle(string title);
        
    }
}
