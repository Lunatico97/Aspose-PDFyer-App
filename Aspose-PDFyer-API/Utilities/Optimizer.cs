using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Aspose.Words.Pdf2Word.FixedFormats;
using Aspose.Words.Saving;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;

namespace AsposeTriage.Utilities
{
    public class Optimizer
    {
        public static byte[] CompressPDF(IFormFile formFile, int imgQuality = 50)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                Document document = new Document(ms);
                var optimizeOptions = new Aspose.Pdf.Optimization.OptimizationOptions();
                optimizeOptions.ImageCompressionOptions.CompressImages = true;
                optimizeOptions.ImageCompressionOptions.ImageQuality = (imgQuality % 100);
                document.OptimizeResources(optimizeOptions);
                using (MemoryStream outStream = new MemoryStream())
                {
                    document.Save(outStream, Aspose.Pdf.SaveFormat.Pdf);
                    return outStream.ToArray();
                }
            }
        }

        public static byte[] EncryptPDF(IFormFile formFile, string ownerPassword, string userPassword)
        {
            using (var ms = new MemoryStream())
            {
                formFile.CopyTo(ms);
                Document document = new Document(ms);
                var permissions = Aspose.Pdf.Permissions.PrintDocument | Aspose.Pdf.Permissions.ModifyContent | Aspose.Pdf.Permissions.ExtractContent;

                // Apply encryption to the document
                document.Encrypt(ownerPassword, userPassword, permissions, CryptoAlgorithm.RC4x128);

                using (MemoryStream outStream = new MemoryStream())
                {
                    document.Save(outStream, Aspose.Pdf.SaveFormat.Pdf);
                    return outStream.ToArray();
                }
            }
           
        }
        
    }
}
