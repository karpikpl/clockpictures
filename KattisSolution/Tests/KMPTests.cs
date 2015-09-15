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

        [Test]
        public void KMP_Should_FindSubstring2()
        {
            // Arrange
            const string text = "ABC ABCDAB ABCDABCDABDE";
            const string serch = "DABD";
            const int expected = 18;

            // Act
            var result = Program.Kmp(text, serch);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void KMP_Should_FindSubstring3()
        {
            // Arrange
            const string text = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxyxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxyxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxyxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxy";
            const string serch = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            const int expected = -1;

            // Act
            var result = Program.Kmp(text, serch);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void KMP_Should_FindSubstring_When_TextLength1()
        {
            // Arrange
            const string text = "x";
            const string serch = "y";
            const int expected = -1;

            // Act
            var result = Program.Kmp(text, serch);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void KMP_Should_FindSubstring5()
        {
            // Arrange
            const string text = "ab";
            const string serch = "c";
            const int expected = -1;

            // Act
            var result = Program.Kmp(text, serch);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void KMP_Should_FindSubstring6()
        {
            // Arrange
            const string text = "abc";
            const string serch = "b";
            const int expected = 1;

            // Act
            var result = Program.Kmp(text, serch);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void KMP_Should_FindSubstring_When_NoMatch()
        {
            // Arrange
            const string text = "ABC ABCDAB ABCDABCDABDE";
            const string serch = "DABB";
            const int expected = -1;

            // Act
            var result = Program.Kmp(text, serch);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
