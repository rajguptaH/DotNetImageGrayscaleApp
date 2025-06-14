using BackendLib;
using NUnit.Framework;
using System.IO;

namespace UnitTests
{
    public class LoggerTests
    {
        [Test]
        public void Log_WritesToFile()
        {
            string testMessage = "Test log message";
            Logger.Log(testMessage);

            string content = File.ReadAllText("log.txt");
            Assert.IsTrue(content.Contains(testMessage));
        }
    }
}