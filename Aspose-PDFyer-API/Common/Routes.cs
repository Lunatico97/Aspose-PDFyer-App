namespace AsposeTriage.Common
{
    public static class Routes
    {
        // Bill routes
        public const string GenerateBill = "generateBill";
        public const string GetSalesData = "getSalesData";
        public const string ComparePDFAspose = "compare/aspose";
        public const string ComparePDFCustom = "compare/custom";
        // Convert routes
        public const string Word2PDF = "word-to-pdf";
        public const string PDF2Word = "pdf-to-word";
        public const string Excel2PDF = "excel-csv-to-pdf";
        public const string Excel2Word = "excel-csv-to-word";
        public const string FindAndReplace = "find-and-replace";
        public const string Compress = "compress";
        public const string Encrypt = "encrypt";
        public const string Merge = "merge";
        // Custom routes
        public const string GetCustomDataHeaders = "getCustomDataHeaders";
        public const string GeneratePDFCustom = "generateCustom";
        // File routes
        public const string FileUpload = "upload";
        public const string FileDownload = "download";
        // S3 routes
        public const string S3FileUpload = "upload-s3";
        public const string S3FileDownload = "download-s3";
        // WWE routes
        public const string GenerateRoster = "generateRoster";
        public const string GenerateMatchCard = "generateCard";
        public const string GetWrestlersInfo = "getWrestlersInfo";
        // PDF routes
        public const string GeneratePDF = "generatePDF";
        public const string ParseTableFromPDF = "parseTableFromPDF";
        public const string GeneratePDFUsingXML  = "generatePDFusingXML";
    }
}
