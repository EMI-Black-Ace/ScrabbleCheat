using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLookupCore.DB
{
    internal class WordDbEntry
    {
        [Key]
        public string Word { get; private set; } = string.Empty;
        public int A { get; private set; }
        public int B { get; private set; }
        public int C { get; private set; }
        public int D { get; private set; }
        public int E { get; private set; }
        public int F { get; private set; }
        public int G { get; private set; }
        public int H { get; private set; }
        public int I { get; private set; }
        public int J { get; private set; }
        public int K { get; private set; }
        public int L { get; private set; }
        public int M { get; private set; }
        public int N { get; private set; }
        public int O { get; private set; }
        public int P { get; private set; }
        public int Q { get; private set; }
        public int R { get; private set; }
        public int S { get; private set; }
        public int T { get; private set; }
        public int U { get; private set; }
        public int V { get; private set; }
        public int W { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }
        public int Total { get; set; }
        public static WordDbEntry GetEntry(string word)
        {
            var normalizedWord = word.ToLower();
            var entry = new WordDbEntry() { Word = normalizedWord };
            entry.A = normalizedWord.Count(l => l == 'a');
            entry.B = normalizedWord.Count(l => l == 'b');
            entry.C = normalizedWord.Count(l => l == 'c');
            entry.D = normalizedWord.Count(l => l == 'd');
            entry.E = normalizedWord.Count(l => l == 'e');
            entry.F = normalizedWord.Count(l => l == 'f');
            entry.G = normalizedWord.Count(l => l == 'g');
            entry.H = normalizedWord.Count(l => l == 'h');
            entry.I = normalizedWord.Count(l => l == 'i');
            entry.J = normalizedWord.Count(l => l == 'j');
            entry.K = normalizedWord.Count(l => l == 'k');
            entry.L = normalizedWord.Count(l => l == 'l');
            entry.M = normalizedWord.Count(l => l == 'm');
            entry.N = normalizedWord.Count(l => l == 'n');
            entry.O = normalizedWord.Count(l => l == 'o');
            entry.P = normalizedWord.Count(l => l == 'p');
            entry.Q = normalizedWord.Count(l => l == 'q');
            entry.R = normalizedWord.Count(l => l == 'r');
            entry.S = normalizedWord.Count(l => l == 's');
            entry.T = normalizedWord.Count(l => l == 't');
            entry.U = normalizedWord.Count(l => l == 'u');
            entry.V = normalizedWord.Count(l => l == 'v');
            entry.W = normalizedWord.Count(l => l == 'w');
            entry.X = normalizedWord.Count(l => l == 'x');
            entry.Y = normalizedWord.Count(l => l == 'y');
            entry.Z = normalizedWord.Count(l => l == 'Z');
            entry.Total = normalizedWord.Length;
            return entry;
        }
    }
}
