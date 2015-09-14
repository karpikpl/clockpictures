using System;
using System.IO;
using KattisSolution.IO;

namespace KattisSolution
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Solve(Console.OpenStandardInput(), Console.OpenStandardOutput());
        }

        public static void Solve(Stream stdin, Stream stdout)
        {
            IScanner scanner = new OptimizedPositiveIntReader(stdin);
            // uncomment when you need more advanced reader
            // scanner = new Scanner(stdin);
            // scanner = new LineReader(stdin);
            var writer = new BufferedStdoutWriter(stdout);

            var input = scanner.NextInt();

            writer.Write(input * 5);
            writer.Write("\n");
            writer.Flush();
        }

        public static int Kmp(string text, string searchString)
        {
            int m = 0;
            int i = 0;
            int[] T = new int[text.Length];

            KmpTable(text, T);

            while (m + i < text.Length)
            {
                if (searchString[i] == text[m + i])
                {
                    if (i == searchString.Length - 1)
                        return m;
                    i++;
                }
                else
                {
                    if (T[i] > -1)
                    {
                        m = m + i - T[i];
                        i = T[i];
                    }
                    else
                    {
                        i = 0;
                        m++;
                    }
                }
            }
            return -1;
        }

        private static void KmpTable(string w, int[] T)
        {
            int pos = 2;
            int cnd = 0;

            T[0] = -1;
            T[1] = 0;

            while (pos < w.Length)
            {
                if (w[pos - 1] == w[cnd])
                {
                    cnd++;
                    T[pos] = cnd;
                    pos++;
                }
                else if (cnd > 0)
                {
                    cnd = T[cnd];
                }
                else
                {
                    T[pos] = 0;
                    pos++;
                }
            }
        }
    }
}