using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLookupCore
{
    public interface IWordLookup
    {
        bool IsWord(string word);
        IEnumerable<string> GetAllWords();
        IEnumerable<string> FindPossibleWords(string availableLetters, string pattern = "");
    }
}
