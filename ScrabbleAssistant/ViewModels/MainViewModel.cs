using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WordLookupCore;

namespace ScrabbleAssistant.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly WordLookup _lookup = WordLookup.Instance;
    public string AvailableLetters { get; set; } = string.Empty;
    public string CrossedLetters { get; set; } = string.Empty;
    private IEnumerable<string> _possibleWords = Enumerable.Empty<string>();
    public IEnumerable<string> PossibleWords
    {
        get => _possibleWords;
        set => SetProperty(ref _possibleWords, value);
    }
    public ICommand GetWordsCommand { get; }
    public MainViewModel()
    {
        GetWordsCommand = new RelayCommand(GetWords);
    }
    private void GetWords()
    {
        PossibleWords = _lookup.FindPossibleWords(AvailableLetters.ToLower(), CrossedLetters.ToLower()).ToList();
    }
}
