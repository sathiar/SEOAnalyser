using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

[assembly: InternalsVisibleTo("WordCounter.UnitTests")]

namespace WebClient.util
{
    public class Analyser
    {
        private readonly HtmlNode _rootNode;
        private readonly char[] _delimiterList = { ' ', ',', '.', '\r', '\n', '\t', '/' };

        private Dictionary<string, int> _words;
        private Dictionary<string, int> _keywords;
        private readonly Dictionary<string, int> _wordToIgnore;
        private int _extLinksNumber = -1;

        public Analyser(string pageUrlOrText, bool isUrl, string wordsToIgnore = null, char[] _delimiters = null)
        {
            if (_delimiters != null)
                _delimiterList = _delimiters;

            if (!String.IsNullOrEmpty(wordsToIgnore))
            {
                _wordToIgnore = new Dictionary<string, int>();
                SplitTextAndCount(wordsToIgnore, _wordToIgnore, null, _delimiterList, true);
                if (_wordToIgnore.Count == 0)
                    _wordToIgnore = null;
            }

            HtmlDocument htmlDocument;

            try
            {
                if (isUrl)
                    htmlDocument = new HtmlWeb().Load(pageUrlOrText); //load from url
                else
                {
                    htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(pageUrlOrText); // load from string
                }
            }
            catch (HtmlWebException ex) 
            {
                throw new WebException("HtmlAgilityPack: " + ex.Message);
            }

            _rootNode = htmlDocument.DocumentNode;
        }

        /// <summary>
        /// Count word occurencies in document and insert word/occurencies in dictionary
        /// </summary>
        /// <returns>Dictionary of words</returns>
        public Dictionary<string, int> AddWordsCountToDictionary()
        {
            if (_words != null)
                return _words; 

            _words = new Dictionary<string, int>();
            CompleteTextNodesSearch(_rootNode, _words, _wordToIgnore, _delimiterList, true);

            if (_words.Count == 0)
                _words = null;

            return _words;
        }

        /// <summary>
        /// Get keywords from "meta keywords" and count this keywords in text
        /// If words are already counted (GetWordsDictionary() have been invoked), word occurences will get from dictionary of words
        /// Otherwise will be count by searching in text
        /// </summary>
        /// <returns>Keywords dictionary</returns>
        public Dictionary<string, int> GetKeywordsCount()
        {
            if (_keywords != null)
                return _keywords; //return already counted dic

            _keywords = GetMetaTagsKeywordsCount(_rootNode, _delimiterList);
            if (_keywords != null) 
                if (_words != null)
                {
                    //get word occurencies from dicionary of words
                    var tempKeywords = new Dictionary<string, int>();
                    foreach (var keyword in _keywords.Keys)
                    {
                        tempKeywords[keyword] = _words.ContainsKey(keyword) ? _words[keyword] : 0;
                    }
                    _keywords = tempKeywords;
                }
                else
                {
                    //Full search in text
                    CompleteTextNodesSearch(_rootNode, _keywords, _wordToIgnore, _delimiterList, false);
                }
            return _keywords;
        }

        public int GetExternalLinksCount()
        {
            if (_extLinksNumber >= 0)
                return _extLinksNumber; 

            _extLinksNumber = 0;
            var nodes = _rootNode.SelectNodes(@"//a[@href]");
            if (nodes != null)
                foreach (var link in nodes)
                {
                    var att = link.Attributes["href"];
                    if (att == null) continue;
                    var href = att.Value;
                    if (href.StartsWith("javascript", StringComparison.InvariantCultureIgnoreCase) ||
                        href.StartsWith("#", StringComparison.InvariantCultureIgnoreCase)) continue;

                    var uri = new Uri(href, UriKind.RelativeOrAbsolute);

                    if (uri.IsAbsoluteUri)
                    {
                        _extLinksNumber++;
                    }
                }
            return _extLinksNumber;
        }

        internal static void CompleteTextNodesSearch(HtmlNode rootNode, Dictionary<string, int> dic, Dictionary<string, int> stopWordsDictionary,
            char[] wordsSeparator, bool isCanAddNewKeys)
        {
            foreach (var node in rootNode.Descendants("#text"))
            {
                if (String.Compare(node.ParentNode.Name, "script", true, CultureInfo.InvariantCulture) != 0)
                {
                    string s = node.InnerText;
                    s = ReplaceNotLetters(ReplaceSpecialCharacters(s)).Trim();
                    if (!String.IsNullOrEmpty(s))
                    {
                        SplitTextAndCount(s, dic, stopWordsDictionary, wordsSeparator, isCanAddNewKeys);
                    }
                }
            }
        }

        internal static Dictionary<string, int> GetMetaTagsKeywordsCount(HtmlNode rootNode, char[] wordsSeparator)
        {
            HtmlNode keywordsNode = rootNode.SelectSingleNode("//meta[@name='Keywords']");
            if (keywordsNode != null)
            {
                string keyWords = keywordsNode.GetAttributeValue("content", "");
                if (!String.IsNullOrEmpty(keyWords))
                {
                    string[] splittedKeywords = keyWords.Split(wordsSeparator, StringSplitOptions.RemoveEmptyEntries);
                    if (splittedKeywords.Length > 0)
                    {
                        var keywordDictionary = new Dictionary<string, int>();
                        foreach (var keyWord in splittedKeywords)
                        {
                            keywordDictionary[keyWord.ToLower()] = 0;
                        }
                        return keywordDictionary;
                    }
                }
            }
            return null;
        }

        public static void SplitTextAndCount(string s, Dictionary<string, int> dic, Dictionary<string, int> wordsToIgnore,
            char[] delimiters, bool canAddNewKeys)
        {
            if (String.IsNullOrEmpty(s))
                return;

            string[] words = s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].ToLower();
                if (dic.ContainsKey(words[i]))
                    dic[words[i]]++;
                else
                    if (canAddNewKeys)
                    if (wordsToIgnore == null || !wordsToIgnore.ContainsKey(words[i]))
                        dic[words[i]] = 1;
            }

        }

        public static string ReplaceSpecialCharacters(string s)
        {
            return Regex.Replace(s, @"&[^\s;]+;", " ");
        }

        public static string ReplaceNotLetters(string s)
        {
            return Regex.Replace(s, @"[^a-zA-Z]+", " ");
        }
    }
}
