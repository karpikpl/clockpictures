using System.IO;
using System.Text;
using NUnit.Framework;

namespace KattisSolution.Tests
{
    [TestFixture]
    public class ExtraTests
    {
        [Test]
        public void SampleTest_WithStringData_Should_Pass_When_SmallNumbers()
        {
            // Arrange
            const string expectedAnswer = "possible\n";
            using (var input = new MemoryStream(Encoding.UTF8.GetBytes("2\n0 180000\n1 180001")))
            using (var output = new MemoryStream())
            {
                // Act
                Program.Solve(input, output);
                var result = Encoding.UTF8.GetString(output.ToArray());

                // Assert
                Assert.That(result, Is.EqualTo(expectedAnswer));
            }
        }

        [Test]
        public void SampleTest_WithStringData_Should_Pass_When_LongString()
        {
            // Arrange
            StringBuilder sb1 = new StringBuilder().Append("0 ");
            StringBuilder sb2 = new StringBuilder(8).Append("8 ");
            for (int i = 0; i < 199999; i++)
            {
                sb1.Append((359999 - 2 * i) % 360000).Append(" ");
                sb2.Append(150000 + i).Append(" ");
            }

            const string expectedAnswer = "impossible\n";
            using (var input = new MemoryStream(Encoding.UTF8.GetBytes("200000\n" + sb1 + "\n" + sb2)))
            using (var output = new MemoryStream())
            {
                // Act
                Program.Solve(input, output);
                var result = Encoding.UTF8.GetString(output.ToArray());

                // Assert
                Assert.That(result, Is.EqualTo(expectedAnswer));
            }
        }

        [Test]
        public void SampleTest_WithStringData_Should_Pass_When_1and2Diffs()
        {
            // Arrange
            StringBuilder sb1 = new StringBuilder().Append("0 ");
            StringBuilder sb2 = new StringBuilder(8).Append("8 ");
            int a = 0;
            for (int i = 0; i < 199999; i++)
            {
                a += (i % 2 == 0 ? 1 : 2);
                sb1.Append(a).Append(" ");
                sb2.Append(i + (i % 3)).Append(" ");
            }

            const string expectedAnswer = "impossible\n";
            using (var input = new MemoryStream(Encoding.UTF8.GetBytes("200000\n" + sb1 + "\n" + sb2)))
            using (var output = new MemoryStream())
            {
                // Act
                Program.Solve(input, output);
                var result = Encoding.UTF8.GetString(output.ToArray());

                // Assert
                Assert.That(result, Is.EqualTo(expectedAnswer));
            }
        }

        [Test]
        public void AreTwoArraysSame_Should_Compare_TwoRings()
        {
            // Arrange
            var a = new[] {"a", "b", "c"};
            var b = new[] {"c", "a", "b"};

            // Act
            var result =Program.AreTwoRingsSame(a, b);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AreTwoArraysSame_Should_Compare_TwoRings2()
        {
            // Arrange
            var a = new[] { "a", "b", "c" };
            var b = new[] { "b", "c", "a" };

            // Act
            var result = Program.AreTwoRingsSame(a, b);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AreTwoArraysSame_Should_Compare_TwoRings3()
        {
            // Arrange
            var a = new[] { "a", "b", "c" };
            var b = new[] { "a", "b", "c" };

            // Act
            var result = Program.AreTwoRingsSame(a, b);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
