using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace CountWords
{
    class Program
    {
        static Regex regexRemoveSymbols = new Regex("[;,.]");

        static IDictionary<string, int> GetWordsCountDict(string line)
        {
            var clearLine = regexRemoveSymbols.Replace(line, "");
            var splitResult = new List<string>(clearLine.Split(' '));
            return splitResult.GroupBy(m => m).ToDictionary(keySelector: g => g.Key, elementSelector: g => g.Count());
        }

        static void Main(string[] args)
        {
            const string relativePath = @".\input.txt";
            try
            {
                IEnumerable<string> lines = File.ReadAllLines(relativePath);
                Console.WriteLine($"Read successfull on path \"{relativePath}\"");

                var dictsList = new List<IDictionary<string, int>>();
                foreach (string line in lines)
                {
                    dictsList.Add(GetWordsCountDict(line));
                }

                var result = dictsList.SelectMany(dict => dict)
                    .ToLookup(pair => pair.Key, pair => pair.Value)
                    .ToDictionary(group => group.Key, group => group.Sum());

                var resultLines = result.Select(kvp => $"{kvp.Key}: {kvp.Value}");

                Console.WriteLine("Words stat:");
                Console.WriteLine(string.Join(Environment.NewLine, resultLines));
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File on path \"{relativePath}\" not found");
            }
        }
    }
}
