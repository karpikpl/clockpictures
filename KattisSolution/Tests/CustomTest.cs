using System.IO;
using System.Text;
using NUnit.Framework;

namespace KattisSolution.Tests
{
    [TestFixture]
    [Category("sample")]
    public class CustomTest
    {
        [Test]
        public void SampleTest_WithStringData_Should_Pass()
        {
            // Arrange
            const string expectedAnswer = "possible\n";
            using (var input = new MemoryStream(Encoding.UTF8.GetBytes("6\n0 358000 1000 359000 2000 90000\n92000 90000 88000 89000 180000 91000")))
            using (var output = new MemoryStream())
            {
                // Act
                Program.Solve(input, output);
                var result = Encoding.UTF8.GetString(output.ToArray());

                // Assert
                Assert.That(result, Is.EqualTo(expectedAnswer));
            }
        }
    }
}
