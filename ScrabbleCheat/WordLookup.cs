using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrabbleCheat
{
    internal class WordLookup
    {
        private TryWordNode RootNode = new() { Word = "" };
        public WordLookup(string dictionaryPath) 
        { 
            var validWords = File.ReadAllLines(dictionaryPath);
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
            foreach(var letter in word)
            {
                if(!node.Children.TryGetValue(letter, out node))
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

        public IEnumerable<string> FindPossibleWords(string availableLetters) => FindPossibleWordsRaw(availableLetters).Distinct();

        private IEnumerable<string> FindPossibleWordsRaw(string availableLetters)
        {
            foreach (var permutation in Permutations.GetPermutations(Enumerable.Range(0, availableLetters.Length).ToArray()))
            {
                var node = RootNode;
                foreach(var seq in permutation) 
                { 
                    if(node.Children.TryGetValue(availableLetters[seq], out var nextNode))
                    {
                        node = nextNode;
                        if(node.IsWord)
                        {
                            yield return node.Word;
                        }
                    }
                }
            }
        }
    }
}
