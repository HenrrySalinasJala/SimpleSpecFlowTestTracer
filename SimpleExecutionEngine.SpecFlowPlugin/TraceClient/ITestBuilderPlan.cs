using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleExecutionEngine.SpecFlowPlugin.TraceClient
{
    public interface ITestBuilderPlan
    {
        ITestBuilderPlan SetFullName();
        ITestBuilderPlan SetTitle(string title);
        
    }
}
