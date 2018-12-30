using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Analyser.Tests
{
    [TestClass()]
    public class AnalyserTests
    {
        [TestMethod()]
        public void ReplaceSpecialCharacterTest()
        {
            string text = "&#123;test&#123;test234&#556;";
            string result = " test test234 ";
            string str = WebClient.util.Analyser.ReplaceSpecialCharacters(text);
            Assert.AreEqual(str, result);
        }

        [TestMethod()]
        public void GetWordCount_OneStopWordAndWordsDictionary_Count()
        {
            string text = "sathia test";
            Dictionary<string, int> StopWords = new Dictionary<string, int>();
            Dictionary<string, int> Words = new Dictionary<string, int>();
            StopWords.Add("sathia", 0);
            WebClient.util.Analyser.SplitTextAndCount(text, Words, StopWords, null, true);
            int length = Words.Count;

            Assert.AreEqual(length, 1);
        }

        [TestMethod()]
        public void GetWordCount_DifferentWordsWithMixedCases_CorrectFrequency_Occurences()
        {
            string text = "sathia TEst SATHIA test";
            Dictionary<string, int> Words = new Dictionary<string, int>();
            Words.Add("sathia", 1);
            WebClient.util.Analyser.SplitTextAndCount(text, Words, null, null, false);
            int occurences = Words["sathia"];

            Assert.AreEqual(occurences, 3);
        }

        [TestMethod()]
        public void GetWordCount_DuplicateWordsWithMixedCases_CorrectFrequency_Occurences()
        {
            string text = "sathia SATHIA";
            Dictionary<string, int> Words = new Dictionary<string, int>();
            WebClient.util.Analyser.SplitTextAndCount(text, Words, null, null, true);
            int occurences = Words["sathia"];

            Assert.AreEqual(occurences, 2);
        }

        [TestMethod()]
        public void GetWordCount_DuplicateWordsWithMixedCases_CorrectFrequency_Count()
        {
            string text = "sathia SATHIA";
            Dictionary<string, int> Words = new Dictionary<string, int>();
            WebClient.util.Analyser.SplitTextAndCount(text, Words, null, null, true);
            int length = Words.Count;

            Assert.AreEqual(length, 1);
        }

        [TestMethod()]
        public void GetWordCount_EmptyText_EmptyResult()
        {
            string text = "              ";
            Dictionary<string, int> Words = new Dictionary<string, int>();
            WebClient.util.Analyser.SplitTextAndCount(text, Words, null, null, true);
            int length = Words.Count;

            Assert.AreEqual(length, 0);
        }
    }
}