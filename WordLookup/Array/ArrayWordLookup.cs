using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WordLookupCore.Array
{
    public class ArrayWordLookup : IWordLookup
    {
        private string[] words;
        public ArrayWordLookup()
        {
            words = Properties.Resources.dictionary.Split(Environment.NewLine);
        }

        public IEnumerable<string> FindPossibleWords(string availableLetters, string pattern = "")
        {
            var wildcards = availableLetters.Count(l => l == '*');
            var usableLetters = (availableLetters.Replace("*", "") + pattern.Replace("_", "")).ToUpper();
            var patternRegex = new Regex(pattern.ToUpper().Replace("_", @"\S"));
            return words.Where(word =>
            {
                for(int i = 0; i < 26; i++)
                {
                    if(usableLetters.Count(l => l == 'A' + i) + wildcards < word.Count(l => l == 'A' + i))
                    {
                        return false;
                    }
                }
                if(word.Length > usableLetters.Length + wildcards)
                {
                    return false;
                }
                if(pattern != "")
                {
                    return patternRegex.IsMatch(word);
                }
                return true;
            });
        }

        public IEnumerable<string> GetAllWords()
        {
            return words;
        }

        public bool IsWord(string word)
        {
            return System.Array.BinarySearch(words, word.ToUpper()) > 0;
        }
    }
}
