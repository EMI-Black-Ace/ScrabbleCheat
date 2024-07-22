﻿using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordLookupCore.DB;

namespace Benchmark
{
    public class WordLookupBenchmark
    {
        DbWordLookup _lookup = DbWordLookup.GetLookup().GetAwaiter().GetResult();
        private string _word = "optimiz";
        private string _cross = "ed";
        private string _wildcard1 = "opti*iz";
        private string _wildcard2 = "o*tim*iz";

        [Benchmark]
        public List<string> LookupWords() => _lookup.FindPossibleWords(_word, _cross).ToList();
        [Benchmark]
        public List<string> LookupWordsOneWildcard() => _lookup.FindPossibleWords(_wildcard1, _cross).ToList();
        [Benchmark]
        public List<string> LookupWordsTwoWildcards() => _lookup.FindPossibleWords(_wildcard2, _cross).ToList();
    }
}
