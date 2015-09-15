using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace KattisSolution.Tests
{
    [TestFixture]
    public class KMPTests
    {
        [Test]
        public void KMP_Should_FindSubstring()
        {
            // Arrange
            const string text = "ABC ABCDAB ABCDABCDABDE";
            const string serch = "ABCDABD";
            const int expected = 15;

            // Act
            var result = Program.Kmp(text, serch);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
