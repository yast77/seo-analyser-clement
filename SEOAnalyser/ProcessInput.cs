using HtmlAgilityPack;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

namespace SEOAnalyser
{
    public class ProcessInput
    {
        private static readonly string[] stopWords = new string[] { "a", "able", "about", "across", "after", "all", "almost", "also", "am", "among", "an", "and", "any", "are", " as", "at", "be", "because", "been", "but", "by", "can", "cannot", "could", "dear", "did", "do", "does", "either", "else", "ever", "every", "for", "from", "get", "got", "had", "has", "have", "he", "her", "hers", "him", "his", "how", "however", "i", "if", "in", "into", " is ", "it", "its", "just", "least", "let", "like", "likely", "may", "me", "might", "most", "must", "my", "neither", "no", "nor", "not", "of", "off", "often", "on", "only", "or", "other", "our", "own", "rather", "said", "say", "says", "she", "should", "since", "so", "some", "than", "that", "the", "their", "them", "then", "there", "these", "they", "this", "tis", "to", "too", "twas", "us", "wants", "was", "we", "were", "what", "when", "where", "which", "while", "who", "whom", "why", "will", "with", "would", "yet", "you", "your" };
        private static List<string> FilterStopWords(List<string> inputStringList)
        {
            inputStringList.RemoveAll(x => stopWords.Contains(x));

            return inputStringList;
        }

        private static List<string> ConvertTextToStringList(string inputText)
        {
            var removedHyperlink = Regex.Replace(inputText.ToLower(), @"\b(?:https?://|www\.)\S+\b", "");
            var removedNewlineExtraspaces = Regex.Replace(removedHyperlink, @"\t|\n|\r", " ");
            var removedPunctuation = new string(removedNewlineExtraspaces.Where(c => !char.IsPunctuation(c)).ToArray());
            return removedPunctuation.Split(' ').ToList();
        }

        public static string ConvertHtmlToString(string inputHtml)
        {
            //Get Html body content
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(inputHtml);

            //Remove Script and Style Tags
            htmlDoc.DocumentNode.Descendants()
                            .Where(n => n.Name == "script" || n.Name == "style")
                            .ToList()
                            .ForEach(n => n.Remove());

            var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//body");

            return htmlBody.InnerText;

        }

        private static List<OccurrenceModel> GetNumberOfOccurrenceList(List<string> inputStringList)
        {
            var lOccurrence = FilterStopWords(inputStringList).GroupBy(x => x).Select(n => new OccurrenceModel
            {
                OccurrenceWordOrLink = n.Key,
                OccurrenceCount = n.Count()
            }).ToList();

            //Remove empty spaces words
            return lOccurrence.Where(x => !string.IsNullOrWhiteSpace(x.OccurrenceWordOrLink)).ToList();
        }

        public static List<OccurrenceModel> GetNumberOfOccurrencePage(string inputString)
        {
            var inputStringList = ConvertTextToStringList(inputString);


            return GetNumberOfOccurrenceList(inputStringList);
        }

        public static List<OccurrenceModel> GetExternalLinkList(string inputString)
        {
            var linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var lExternalList = linkParser.Matches(inputString).Cast<Match>().Select(m => m.Value).ToList();

            return GetNumberOfOccurrenceList(lExternalList);
        }

        public static List<OccurrenceModel> GetNumberOfMetatagOccurrence(string downloadString)
        {
            var inputString = ConvertHtmlToString(downloadString);
            var inputStringList = ConvertTextToStringList(inputString.ToLower());

            var lMetaTag = GetMetatagList(downloadString);

            var lMetaTagPageWordMatch = lMetaTag.Where(x => inputStringList.Contains(x)).ToList();
            var getNumberOfMetaTagOccurrence = GetNumberOfOccurrenceList(lMetaTagPageWordMatch);

            //Add MetaTag Keyword that not exist in Occurence list
            var oTempOccurrence = new OccurrenceModel();
            foreach (var item in FilterStopWords(lMetaTag))
            {
                if (getNumberOfMetaTagOccurrence.Where(x => x.OccurrenceWordOrLink.Contains(item)).Count() <= 0)
                {
                    oTempOccurrence = new OccurrenceModel() { OccurrenceWordOrLink = item, OccurrenceCount = 0 };
                    getNumberOfMetaTagOccurrence.Add(oTempOccurrence);
                }
            }

            return getNumberOfMetaTagOccurrence;
        }

        private static List<string> GetMetatagList(string inputHtml)
        {
            var finalMetatagList = new List<string>();
            //Get Metatag
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(inputHtml);

            //Get Meta Description content
            var metaDescriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//meta[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='description']");

            if (metaDescriptionNode != null)
            {
                HtmlAttribute desc;

                desc = metaDescriptionNode.Attributes["content"];
                string fullDescription = desc.Value.ToLower();

                finalMetatagList.AddRange(ConvertTextToStringList(fullDescription));
            }

            //Get Meta Keyword content
            var metaKeywordsNode = htmlDoc.DocumentNode.SelectSingleNode("//meta[translate(@name,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')='keywords']");

            if (metaKeywordsNode != null)
            {
                HtmlAttribute keyword;

                keyword = metaKeywordsNode.Attributes["content"];
                string fullKeywords = keyword.Value.ToLower();

                finalMetatagList.AddRange(fullKeywords.Split(',').Select(x => x.Trim()).ToList());
            }

            return finalMetatagList;
        }

    }
}