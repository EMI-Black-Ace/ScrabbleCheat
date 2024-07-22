using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLookupCore.Trie
{
    public class TrieWordLookup : IWordLookup
    {
        private TryWordNode RootNode = new() { Word = "" };
        private static TrieWordLookup? instance;
        public static TrieWordLookup Instance
        {
            get
            {
                if (instance == null)
                    instance = new TrieWordLookup();
                return instance;
            }
        }
        private TrieWordLookup()
        {
            var validWords = Properties.Resources.dictionary.Split(Environment.NewLine);
            foreach (var word in validWords)
            {
                RootNode.AddWord(word.ToLower());
            }
        }
        private class TryWordNodeCollection : IEnumerable<TryWordNode>
        {
            private TryWordNode[] array = new TryWordNode[26];
            public IEnumerator<TryWordNode> GetEnumerator() => array.Where(x => x != null).GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public void Add(char letter, TryWordNode parent)
            {
                if (array[letter - 'a'] == null)
                {
                    array[letter - 'a'] = new TryWordNode() { Word = parent.Word + letter, Parent = parent };
                }
            }
            public TryWordNode this[char letter] => array[letter - 'a'];
            public bool BranchesToLetter(char letter) => array[letter - 'a'] != null;
        }
        [DebuggerDisplay("{Word}:{IsWord}")]
        private class TryWordNode : IEnumerable<TryWordNode>
        {
            public TryWordNode? Parent { get; set; }
            public required string Word { get; set; }
            public bool IsWord { get; set; }
            public TryWordNodeCollection Children { get; } = new();
            public void AddWord(string word, int depth = 0)
            {
                if (word.Length == depth)
                {
                    IsWord = true;
                    return;
                }
                if (!Children.BranchesToLetter(word[depth]))
                {
                    Children.Add(word[depth], this);
                }
                Children[word[depth]].AddWord(word, depth + 1);
            }

            public IEnumerator<TryWordNode> GetEnumerator()
            {
                foreach (var node in Children.SelectMany(n => n))
                {
                    yield return node;
                }
                yield return this;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public bool IsWord(string word)
        {
            word = word.ToLower();
            var node = RootNode;
            foreach (var letter in word)
            {
                if (node.Children.BranchesToLetter(letter))
                {
                    return false;
                }
                else
                {
                    node = node.Children[letter];
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
                char letter = availableLetters[sequence[i]];
                if (letter == '*')
                {
                    foreach (var word in node.Children.SelectMany(c => FindPossibleWordsForSequence(availableLetters, c, sequence, i + 1)))
                    {
                        yield return word;
                    }
                }
                else if (node.Children.BranchesToLetter(letter))
                {
                    node = node.Children[letter];
                    if (node.IsWord)
                    {
                        yield return node.Word;
                    }
                }
                else break;
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
