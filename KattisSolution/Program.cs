using System;
using System.IO;
using System.Linq;
using KattisSolution.IO;

namespace KattisSolution
{
    class Program
    {
        static void Main(string[] args)
        {
            Solve(Console.OpenStandardInput(), Console.OpenStandardOutput());
        }

        public static void Solve(Stream stdin, Stream stdout)
        {
            IScanner scanner = new OptimizedPositiveIntReader(stdin);
            // uncomment when you need more advanced reader
            // scanner = new Scanner(stdin);
            // scanner = new LineReader(stdin);
            BufferedStdoutWriter writer = new BufferedStdoutWriter(stdout);

            var n = scanner.NextInt();
            int[] a = new int[n];
            int[] b = new int[n];

            for (int i = 0; i < n; i++)
            {
                a[i] = scanner.NextInt();
            }

            for (int i = 0; i < n; i++)
            {
                b[i] = scanner.NextInt();
            }

            // first put hands in order
            a = a.OrderBy(v => v).ToArray();
            b = b.OrderBy(v => v).ToArray();
            string result = null;

            for (int attempt = 0; attempt < n; attempt++)
            {
                if (CheckSolution(ref a, ref b, attempt))
                {
                    result = "possible";
                    break;
                }
            }

            writer.Write(result ?? "impossible");
            writer.Write("\n");
            writer.Flush();
        }

        private static bool CheckSolution(ref int[] a, ref int[] b, int bIndex)
        {
            int startDiff = Math.Abs(a[0] - b[bIndex]) % 360000;

            startDiff = startDiff <= 180000 ? startDiff : 360000 - startDiff;

            for (int i = 1; i < a.Length; i++)
            {
                var diff = Math.Abs(a[i] - b[(bIndex + i) % b.Length]) % 360000;
                diff = diff <= 180000 ? diff : 360000 - diff;

                if (diff != startDiff)
                    return false;
            }
            return true;
        }
    }
}
