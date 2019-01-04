using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Tracing;

namespace SimpleSpecFlowListener
{
    public class SimpleListener : ITraceListener
    {
        public void WriteTestOutput(string message)
        {
            Console.WriteLine("********************write test output 1 "+ message);
        }

        public void WriteToolOutput(string message)
        {
            Console.WriteLine("******************write tool output 2 " + message);
        }
    }
}
