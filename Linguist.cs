using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cunning_Linguist
{
    public class Linguist
    {
        private readonly Dictionary<string, int> _words;
        private readonly string _vowels = "aeiouy";
        private bool _firstGuess = true;

        private readonly List<string> _wordleOpeners =
        [
            "least", "trace", "slate", "crane", "salet",
            "crate", "stare", "soare", "canoe", "raise",
            "arose", "arise", "roast", "least", "dealt",
            "cones", "trial", "audio", "adieu", "ouija",
            "clasp", "scald", "clint", "trope", "slant",
            "carte", "place", "grain", "paint", "storm",
            "about", "ocean", "oaken", "irate", "alien",
            "alone", "atone", "canoe", "equal", "outre",
            "blend", "crush", "fresh", "shout", "cloud",
            "spice", "round", "grain", "peach", "until"
        ];

        public Linguist()
        {
            _words = ReadWordsFromFile("wordlist.txt");
            Score();
        }

        public void Process(string[] FixedLetters, string[] FloatingLetters, string BadLetters)
        {
            foreach (var word in _words)
            {
                _words[word.Key] = 1;
            }

            var fixedLettersString = string.Concat(FixedLetters);
            var floatingTokens = string.Concat(FloatingLetters);
            _firstGuess = false;

            if (string.IsNullOrWhiteSpace(floatingTokens) && string.IsNullOrWhiteSpace(BadLetters) && string.IsNullOrWhiteSpace(fixedLettersString))
            {
                Score();
                _firstGuess = true;
                return;
            }

            var knownTokens = floatingTokens.ToLower().Order().Distinct().ToList();
            var badTokens = BadLetters.ToLower().Order().Distinct().Where(w => !knownTokens.Contains(w)).ToList();

            foreach (var token in knownTokens)
            {
                foreach (var word in _words.Where(w => w.Value > 0))
                {
                    if (!word.Key.Contains(token))
                    {
                        _words[word.Key] = 0;
                    }

                    for (var i = 0; i < word.Key.Length; i++)
                    {
                        if (FloatingLetters[i].Contains(word.Key[i]))
                        {
                            _words[word.Key] = 0;
                        }
                    }
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (string.IsNullOrEmpty(FixedLetters[i]))
                {
                    continue;
                }

                foreach (var word in _words.Where(w => w.Value > 0))
                {
                    var wordToken = word.Key[i].ToString();
                    if (wordToken != FixedLetters[i])
                    {
                        _words[word.Key] = 0;
                    }
                    else
                    {
                        _words[word.Key] = 1;
                    }
                }
            }

            foreach (var token in badTokens)
            {
                foreach (var word in _words.Where(w => w.Value > 0))
                {
                    if (word.Key.Contains(token))
                    {
                        _words[word.Key] = 0;
                    }
                }
            }

            Score();
        }

        private void Score()
        {
            var remainingWords = _words.Where(w => w.Value > 0);
            var remianingWordsCount = remainingWords.Count();

            foreach (var word in remainingWords)
            {
                var baseScore = WordsElminatedByWord(word.Key);
                var vowelCount = VowelScore(word.Key);

                _words[word.Key] = baseScore + (int)Math.Round(baseScore * (vowelCount * 1.5));
            }
        }

        public string GetSuggestedWord()
        {
            if (_firstGuess)
            {
                return _wordleOpeners[0];
            }

            var remainingWords = _words.Where(w => w.Value > 0).OrderByDescending(w => w.Value).ToList();

            if (remainingWords.Count == 0)
            {
                return "None!";
            }
            else
            {
                return remainingWords[0].Key;
            }
        }

        public int VowelScore(string word)
        {
            return word.Where(c => _vowels.Contains(c)).Distinct().Count();
        }

        public string GetAllSuggestedWords()
        {
            if (_firstGuess)
            {
                return string.Join(", ", _wordleOpeners.Skip(1));
            }

            return string.Join(", ", _words.Where(w => w.Value > 0).OrderByDescending(w => w.Value).Skip(1).Select(w => w.Key));
        }

        public int WordsElminatedByWord(string candidate)
        {
            HashSet<string> eliminatedWords = [];
            for (int i = 0; i < 5; i++)
            {
                var letter = candidate[i];
                foreach (var word in _words.Where(w => w.Value > 0))
                {
                    if (word.Key[i] == letter)

                    {
                        eliminatedWords.Add(word.Key);
                    }
                }
            }

            return eliminatedWords.Count;
        }

        private static Dictionary<string, int> ReadWordsFromFile(string fileName)
        {
            var words = File.ReadAllLines(fileName).Distinct();
            var dictionary = new Dictionary<string, int>();

            foreach (var word in words)
            {
                dictionary[word] = 1;
            }

            return dictionary;
        }
    }
}