using ScrabbleCheat;

Console.WriteLine("Initializing dictionary...");
var lookup = new WordLookup("../../../dictionary.txt");
Console.WriteLine("Dictionary initialized.");

Console.Write("What are your available letters?: ");
var availableLetters = Console.ReadLine()?.ToLower() ?? string.Empty;
//foreach (var permutation in Permutations.GetPermutations(Enumerable.Range(0, availableLetters.Length).ToArray()))
//{
//    Console.WriteLine(string.Join(' ', permutation));
//}
foreach (var word in lookup.FindPossibleWords(availableLetters))
{
    Console.WriteLine(word);
}