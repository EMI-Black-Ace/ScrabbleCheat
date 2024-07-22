using WordLookupCore.Array;

Console.WriteLine("Initializing dictionary...");
var lookup = new ArrayWordLookup();
Console.WriteLine("Dictionary initialized.");

Console.Write("What are your available letters?: ");
var availableLetters = Console.ReadLine()?.ToLower() ?? string.Empty;
Console.Write("What letters are you crossing?: ");
var crossedLetters = Console.ReadLine()?.ToLower() ?? string.Empty;
//foreach (var permutation in Permutations.GetPermutations(Enumerable.Range(0, availableLetters.Length).ToArray()))
//{
//    Console.WriteLine(string.Join(' ', permutation));
//}
foreach (var word in lookup.FindPossibleWords(availableLetters, crossedLetters))
{
    Console.WriteLine(word);
}