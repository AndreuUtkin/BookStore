using Xceed.Words.NET;
using Xceed.Document.NET;

namespace СОСapi
{
    public class Trans
    {
        public static string curDateTime()
        {
            DateTime myDateTime = DateTime.Now;
            return myDateTime.ToString("yyyy-MM-dd-HH-mm-ss-fff");
        }
        public static void mkTrans(int userId, int BookId)
        {
            string fileName = @"D:\Trans\trans-" + curDateTime() + ".docx";

            var doc = DocX.Create(fileName); //create docx word document

            doc.AddHeaders(); //add header (optional)
            doc.AddFooters(); //add footer in this document (optional code)

            // Force the first page to have a different Header and Footer.
            doc.DifferentFirstPage = true;

            doc.Headers.First.InsertParagraph("Transaction of ").Append(curDateTime());// Insert a Paragraph into the first Header.
            doc.Footers.First.InsertParagraph("Page ").AppendPageNumber(PageNumberFormat.normal).Append(" of ").AppendPageCount(PageNumberFormat.normal); // add footer with page number

            doc.InsertParagraph("Uid: " + userId.ToString() + " Bid: " + BookId.ToString()); // inserts a new paragraph with text

            doc.Save(); // save changes to file
        }
    }
}
