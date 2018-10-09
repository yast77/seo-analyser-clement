using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SEOAnalyser.UnitTests
{
    [TestClass]
    public class ProcessInputTests
    {
        [TestMethod]
        public void Process_GetNumberOfOccurrencePage()
        {
            var fakeInputString = "This is what we call fake input string, yes, is fake input string...";

            var fakeListObject = new List<OccurrenceModel>();
            var fakeObject = new OccurrenceModel();

            fakeObject.OccurrenceWordOrLink = "is";
            fakeObject.OccurrenceCount = 2;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "call";
            fakeObject.OccurrenceCount = 1;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "fake";
            fakeObject.OccurrenceCount = 2;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "input";
            fakeObject.OccurrenceCount = 2;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "string";
            fakeObject.OccurrenceCount = 2;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "yes";
            fakeObject.OccurrenceCount = 1;
            fakeListObject.Add(fakeObject);

            var actualList = ProcessInput.GetNumberOfOccurrencePage(fakeInputString);

            Assert.IsTrue(fakeListObject.SequenceEqual(actualList, new OccurrenceModelComparer()));
        }

        [TestMethod]
        public void Process_GetExternalLinkList()
        {
            var fakeInputString = "There are 3 links here, link1: http://www.google.com, link2: http://www.yahoo.com, link3 http://www.google.com";

            var fakeListObject = new List<OccurrenceModel>();
            var fakeObject = new OccurrenceModel();

            fakeObject.OccurrenceWordOrLink = "http://www.google.com";
            fakeObject.OccurrenceCount = 2;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "http://www.yahoo.com";
            fakeObject.OccurrenceCount = 1;
            fakeListObject.Add(fakeObject);

            var actualList = ProcessInput.GetExternalLinkList(fakeInputString);

            Assert.IsTrue(fakeListObject.SequenceEqual(actualList, new OccurrenceModelComparer()));
        }

        [TestMethod]
        public void Process_GetNumberOfMetatagOccurrence()
        {
            var fakeInputString = @"<!DOCTYPE html>
<html>
<head>
  <meta charset=""UTF-8"">
  <meta name=""description"" content=""This is meta description"">
  <meta name=""keywords"" content=""Meta,SEO,Analyser,Sitecore,Anything"">
</head>
<body>

<p>This is content to test number of occurrences on the page of each word listed in meta tags. It is to test Sitecore SEO Analyser so that SEO works.</p>

</body>
</html>
          ";

            var fakeListObject = new List<OccurrenceModel>();
            var fakeObject = new OccurrenceModel();

            fakeObject.OccurrenceWordOrLink = "is";
            fakeObject.OccurrenceCount = 1;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "meta";
            fakeObject.OccurrenceCount = 2;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "seo";
            fakeObject.OccurrenceCount = 1;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "analyser";
            fakeObject.OccurrenceCount = 1;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "sitecore";
            fakeObject.OccurrenceCount = 1;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "description";
            fakeObject.OccurrenceCount = 0;
            fakeListObject.Add(fakeObject);

            fakeObject = new OccurrenceModel();
            fakeObject.OccurrenceWordOrLink = "anything";
            fakeObject.OccurrenceCount = 0;
            fakeListObject.Add(fakeObject);

            var actualList = ProcessInput.GetNumberOfMetatagOccurrence(fakeInputString);

            Assert.IsTrue(fakeListObject.SequenceEqual(actualList, new OccurrenceModelComparer()));
        }
    }
}
