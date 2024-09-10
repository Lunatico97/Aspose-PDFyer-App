using Aspose.Words;
using DiffMatchPatch;

namespace AsposeTriage.Utilities
{
    public static class DocumentComparator
    {
        public static List<string[]> CompareCustom(IFormFile file1, IFormFile file2)
        {
            List<string[]> checks = new List<string[]>();
            Document doc1 = new Document(file1.OpenReadStream());
            Document doc2 = new Document(file2.OpenReadStream());
            string text1, text2;
            using (MemoryStream stream1 = new MemoryStream())
            using (MemoryStream stream2 = new MemoryStream())
            {
                doc1.Save(stream1, SaveFormat.Text);
                doc2.Save(stream2, SaveFormat.Text);
                text1 = System.Text.Encoding.UTF8.GetString(stream1.ToArray()).Trim();
                text2 = System.Text.Encoding.UTF8.GetString(stream2.ToArray()).Trim();
            }
            var dmp = new diff_match_patch();
            List<Diff> diffs = dmp.diff_main(text1, text2);
            dmp.diff_cleanupSemantic(diffs);
            foreach (Diff diff in diffs)
            {
                checks.Add(new[] { diff.operation.ToString(), diff.text.ToString() });
            }
            return checks;
        }

        public static Stream CompareAspose(IFormFile file1, IFormFile file2, bool saveLocal=true)
        {
            Document docA = new Document(file1.OpenReadStream());
            Document docB = new Document(file2.OpenReadStream());

            // There should be no revisions before comparison.
            docA.AcceptAllRevisions();
            docB.AcceptAllRevisions();

            docA.Compare(docB, "Diwas Adhikari", DateTime.Now);
            Stream compareStream = new MemoryStream();
            docA.Save(compareStream, SaveFormat.Pdf);
            return compareStream;
        }

        public static byte[] MergeDocuments(IFormFile[] files)
        {
            try
            {
                Aspose.Pdf.Document[] documents = files.Select(file => new Aspose.Pdf.Document(file.OpenReadStream())).ToArray();
                Aspose.Pdf.Document mergedDocument = new Aspose.Pdf.Document();
                foreach (var document in documents)
                {
                    mergedDocument.Pages.Add(document.Pages);
                }
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    mergedDocument.Save(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return memoryStream.ToArray();
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
