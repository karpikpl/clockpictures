using System;
using System.IO;
using System.Linq;
using System.Text;

namespace KattisSolution
{
    public class NewSolution
    {
        private static StreamReader _reader;

        public static string ReadAndSolve(Stream stdin, Stream stdout)
        {
            _reader = new StreamReader(stdin);

            var n = ReadInt();

            var a = new int[n];
            var b = new int[n];

            for (int i = 0; i < n; i++)
            {
                a[i] = ReadInt();
            }
            for (int i = 0; i < n; i++)
            {
                b[i] = ReadInt();
            }

            return Solve(a, b);
        }

        public static string Solve(int[] a, int[] b)
        {
            int n = a.Length;

            a = a.OrderBy(v => v).ToArray();
            b = b.OrderBy(v => v).ToArray();

            var sbA = new StringBuilder();
            var sbB = new StringBuilder();

            for (int i = 0; i < n - 1; i++)
            {
                sbA.Append(a[i + 1] - a[i]);
                sbB.Append(b[i + 1] - b[i]);
            }

            sbA.Append(360000 - a[n - 1] + a[0]);
            sbB.Append(360000 - b[n - 1] + b[0]);
            sbA.Append(sbA);

            a = null;
            b = null;

            var searcher = new StringSearcher(sbB.ToString().ToCharArray());
            return (searcher.Search(sbA.ToString().ToCharArray()) == -1 ? "impossible" : "possible");
        }

        static int ReadInt()
        {
            var newChar = _reader.Read();
            int result = 0;

            while (newChar < '0' || newChar > '9')
            {
                newChar = _reader.Read();
            }

            while (newChar >= '0' && newChar <= '9')
            {
                result = result * 10 + newChar - '0';
                newChar = _reader.Read();
            }

            return result;
        }
    }

    public class StringSearcher  // Knuth-Morris-Pratt string search
    {
        private char[] W;  // the (small) pattern to search for
        private int[] T;   // lets the search function to skip ahead

        public StringSearcher(char[] W)
        {
            this.W = new char[W.Length];
            Array.Copy(W, this.W, W.Length);
            this.T = BuildTable(W);           // use a helper to build T
        }

        private int[] BuildTable(char[] W)
        {
            int[] result = new int[W.Length];
            int pos = 2;
            int cnd = 0;
            result[0] = -1;
            result[1] = 0;
            while (pos < W.Length)
            {
                if (W[pos - 1] == W[cnd])
                {
                    ++cnd; result[pos] = cnd; ++pos;
                }
                else if (cnd > 0)
                    cnd = result[cnd];
                else
                {
                    result[pos] = 0; ++pos;
                }
            }
            return result;
        }

        public int Search(char[] S)
        {
            // look for this.W inside of S
            int m = 0;
            int i = 0;
            while (m + i < S.Length)
            {
                if (this.W[i] == S[m + i])
                {
                    if (i == this.W.Length - 1)
                        return m;
                    ++i;
                }
                else
                {
                    m = m + i - this.T[i];
                    if (this.T[i] > -1)
                        i = this.T[i];
                    else
                        i = 0;
                }
            }
            return -1;  // not found
        }
    }
}
