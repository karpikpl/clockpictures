using System;
using System.IO;
using System.Linq;
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

        [Test]
        public void LongSampleTest_WithStringData_Should_Pass()
        {
            // Arrange
            const string expectedAnswer = "possible\n";
            StringBuilder sb1 = new StringBuilder();
            for (int i = 0; i < 199999; i++)
            {
                sb1.Append((int)Math.Ceiling(i + 360000 / 199999f));
                sb1.Append(" ");
            }

            using (var input = new MemoryStream(Encoding.UTF8.GetBytes("200000\n359999 " + sb1 + "\n" + sb1 + "359999")))
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
        public void LongSampleTest_WithStringData_Should_Pass2()
        {
            // Arrange
            const string expectedAnswer = "impossible\n";
            StringBuilder sb1 = new StringBuilder();
            const int sb1Start = 1;
            StringBuilder sb2 = new StringBuilder();
            const int sb2Start = 359999;
            int sb1Prev = 0, sb2Prev = 0;
            for (int i = 0; i < 179999; i++)
            {
                sb1.Append(sb1Start + 2 * i);
                sb1.Append(" ");

                if (i == 179998)
                {
                    sb2.Append(0);
                }
                else
                {
                    sb2.Append(sb2Start - 2 * i);
                }
                sb2.Append(" ");

            }

            using (var input = new MemoryStream(Encoding.UTF8.GetBytes("179999\n" + sb1 + "\n" + sb2)))
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
        public void GetDiffs_Should_ReturnDiffsBetweenHands()
        {
            // Arrange
            int[] data = new[] { 0, 358000, 1000, 359000, 2000, 90000 };
            int[] expected = new[] { 1000, 1000, 88000, 268000, 1000, 1000 };

            // Act
            var result = Program.GetDiffs(data).ToArray();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void Compact_Should_CompactArray()
        {
            // Arrange
            int[] data = new[] { 100000, 100000, 100000, 359000, 2000, 90000 };
            string[] expected = new[] { "100000x3", "359000", "2000", "90000" };

            // Act
            var result = Program.Compact(data).ToArray();

            // Assert
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
