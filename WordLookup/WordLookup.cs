using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLookupCore
{
    public class WordLookup
    {
        private TryWordNode RootNode = new() { Word = "" };
        public WordLookup()
        {
            var validWords = Properties.Resources.dictionary.Split(Environment.NewLine);
            foreach (var word in validWords)
            {
                RootNode.AddWord(word.ToLower());
            }
        }
        [DebuggerDisplay("{Word}:{IsWord}")]
        private class TryWordNode : IEnumerable<TryWordNode>
        {
            public TryWordNode? Parent { get; set; }
            public required string Word { get; set; }
            public bool IsWord { get; set; }
            public Dictionary<char, TryWordNode> Children { get; } = new();
            public void AddWord(string word, int depth = 0)
            {

                if (word.Length == depth)
                {
                    IsWord = true;
                    return;
                }
                TryWordNode node;
                if (Children.ContainsKey(word[depth]))
                {
                    node = Children[word[depth]];
                }
                else
                {
                    node = new TryWordNode { Word = word.Substring(0, depth + 1), Parent = this };
                    Children.Add(word[depth], node);
                }
                node.AddWord(word, depth + 1);
            }

            public IEnumerator<TryWordNode> GetEnumerator()
            {
                foreach (var node in Children.Values.SelectMany(n => n))
                {
                    yield return node;
                }
                yield return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public bool IsWord(string word)
        {
            word = word.ToLower();
            var node = RootNode;
            foreach (var letter in word)
            {
                if (!node.Children.TryGetValue(letter, out node))
                {
                    return false;
                }
            }
            return node.IsWord;
        }

        public IEnumerable<string> GetAllWords()
        {
            return RootNode.Where(n => n.IsWord).Select(n => n.Word);
        }

        public IEnumerable<string> FindPossibleWords(string availableLetters, string crossedLetters) => FindPossibleWordsRaw(availableLetters + crossedLetters).Where(w => w.Contains(crossedLetters)).Distinct();

        private IEnumerable<string> FindPossibleWordsRaw(string availableLetters, TryWordNode? startNode = null)
        {
            foreach (var permutation in Permutations.GetPermutations(Enumerable.Range(0, availableLetters.Length).ToArray()))
            {
                foreach (var word in FindPossibleWordsForSequence(availableLetters, RootNode, permutation, 0))
                {
                    yield return word;
                }
            }
        }

        private IEnumerable<string> FindPossibleWordsForSequence(string availableLetters, TryWordNode startNode, int[] sequence, int startPoint)
        {
            var node = startNode;
            for (int i = startPoint; i < sequence.Length; i++)
            {
                if (availableLetters[sequence[i]] == '*')
                {
                    foreach (var substituteLetter in node.Children.Keys)
                    {
                        var newstr = new StringBuilder(availableLetters).Replace('*', substituteLetter, sequence[i], 1).ToString();
                        foreach (var word in FindPossibleWordsForSequence(newstr, node, sequence, i))
                        {
                            yield return word;
                        }
                    }
                }
                if (node.Children.TryGetValue(availableLetters[sequence[i]], out var nextNode))
                {
                    node = nextNode;
                    if (node.IsWord)
                    {
                        yield return node.Word;
                    }
                }
            }
        }

        private IEnumerable<string> PossibleLetterSetsIncludingWildcards(string availableLetters)
        {
            if (availableLetters.Contains('*'))
            {
                foreach (var letter in "abcdefghijklmnopqrstuvwxyz")
                {
                    StringBuilder s = new(availableLetters);
                    s.Replace('*', letter, availableLetters.IndexOf('*'), 1);
                    foreach (var word in PossibleLetterSetsIncludingWildcards(s.ToString()))
                    {
                        yield return word;
                    }
                }
            }
            else
            {
                yield return availableLetters;
            }
        }
    }
}
