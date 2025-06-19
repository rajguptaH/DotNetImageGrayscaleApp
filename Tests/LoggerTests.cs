using Image.API.Services;
using NUnit.Framework;
using System.IO;

namespace UnitTests
{
    public class ConversionWithLoggerTests
    {
        [Test]
        public void Log_WritesToFile()
        {
            string testMessage = "Test log message";
            Logger.Log(testMessage);

            string content = File.ReadAllText("conversion.log");
            Assert.IsTrue(content.Contains(testMessage));
        }
    }
}