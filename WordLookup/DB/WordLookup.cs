using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordLookupCore.DB
{
    public class WordLookup
    {
        private WordDb _db;
        private WordLookup(WordDb _db)
        {
            this._db = _db;
        }

        private static async Task<WordLookup> GetLookup()
        {
            var db = await WordDb.GetDatabase();
            var lookup = new WordLookup(db);
            return lookup;
        }

        public bool IsWord(string word)
        {
            var normalizedWord = word.ToLower();
            return _db.entries.Where(e => e.Word == normalizedWord).Any();
        }

        public IEnumerable<string> GetAllWords()
        {
            return _db.entries.Select(e => e.Word).ToList();
        }

        public IEnumerable<string> FindPossibleWords(string availableLetters, string crossedLetters)
        {
            var letters = LetterCount(availableLetters.Remove('*').ToLower() + crossedLetters.Remove('*').ToLower());
            var wildcards = availableLetters.Count(l => l == '*');
            var pattern = crossedLetters.ToLower();
            var query = _db.entries.AsQueryable();
            query = query.Where(w => w.A <= letters[0] + wildcards);
            query = query.Where(w => w.B <= letters[1] + wildcards);
            query = query.Where(w => w.C <= letters[2] + wildcards);
            query = query.Where(w => w.D <= letters[3] + wildcards);
            query = query.Where(w => w.E <= letters[4] + wildcards);
            query = query.Where(w => w.F <= letters[5] + wildcards);
            query = query.Where(w => w.G <= letters[6] + wildcards);
            query = query.Where(w => w.H <= letters[7] + wildcards);
            query = query.Where(w => w.I <= letters[8] + wildcards);
            query = query.Where(w => w.J <= letters[9] + wildcards);
            query = query.Where(w => w.K <= letters[10] + wildcards);
            query = query.Where(w => w.L <= letters[11] + wildcards);
            query = query.Where(w => w.M <= letters[12] + wildcards);
            query = query.Where(w => w.N <= letters[13] + wildcards);
            query = query.Where(w => w.O <= letters[14] + wildcards);
            query = query.Where(w => w.P <= letters[15] + wildcards);
            query = query.Where(w => w.Q <= letters[16] + wildcards);
            query = query.Where(w => w.R <= letters[17] + wildcards);
            query = query.Where(w => w.S <= letters[18] + wildcards);
            query = query.Where(w => w.T <= letters[19] + wildcards);
            query = query.Where(w => w.U <= letters[20] + wildcards);
            query = query.Where(w => w.V <= letters[21] + wildcards);
            query = query.Where(w => w.W <= letters[22] + wildcards);
            query = query.Where(w => w.X <= letters[23] + wildcards);
            query = query.Where(w => w.Y <= letters[24] + wildcards);
            query = query.Where(w => w.Z <= letters[25] + wildcards);
            query = query.Where(w => w.Total <= letters[26] + wildcards);
            query = query.Where(w => EF.Functions.Like(w.Word, $"%{pattern}%"));
            return query.Select(w => w.Word).ToList();
        }

        private int[] LetterCount(string word)
        {
            int[] letters = new int[27];
            for(int i = 0; i < word.Length - 1; i++)
            {
                letters[word[i] - 'a']++;
            }
            letters[26] = word.Length;
            return letters;
        }
    }
}
