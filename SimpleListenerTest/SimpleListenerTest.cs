using log4net;
using NUnit.Framework;

namespace SimpleListener
{
    [TestFixture]
    public class SimpleListenerTest
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Test]
        public void Example()
        {
            log.Info("example");
            Assert.True(true);
        }
    }
}
