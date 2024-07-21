using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WordLookupCore.DB
{
    internal class WordDb : DbContext
    {
        private string connectPath;
        private WordDb()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            connectPath = Path.Join(path, "dictionary.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={connectPath}");



        public DbSet<WordDbEntry> entries { get; set; }
        public static async Task<WordDb> GetDatabase()
        {
            var db = new WordDb();
            if(await db.Database.EnsureCreatedAsync())
            {
                var words = Properties.Resources.dictionary.Split(Environment.NewLine);
                foreach(var word in words)
                {
                    db.entries.Add(WordDbEntry.GetEntry(word));
                }
                await db.SaveChangesAsync();
            }

            return db;
        }
    }
}
