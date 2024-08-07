namespace AsposeTriage.Common
{
    public static class Messages
    {
        // File messages
        public const string FileRequired = "File is required !";
        public const string FileNotFound = "File not found !";
        public const string FileNameNotProvided = "No file name is provided to proceed further !";
        public const string FileUploadSuccess = "File successfully uploaded !";
        public const string FileUploadFailure = "Unable to upload file to the server !";
        public const string FileWithInvalidHeaderFormat = "The header format doesn't match ! Choose a valid data file !";
        // PDF messages
        public const string PDFParseFailure = "Unable to parse given PDF file !";
        public const string PDFGeneratedSuccess = "PDF file generated successfully !";
        public const string PDFGeneratedFailure = "Failed to generate PDF file !";
        // Bill messages
        public const string LocationNotProvided = "No location is provided to generate bill !";
        public const string LocationNotAssociatedToSupplier = "This location is not a region associated to the supplier !";
        public const string BillGeneratedSuccess = "Bill generated successfully !";
        public const string BillGeneratedFailure = "Failed to generate required bill PDF !";
        // Merge messages
        public const string MoreThanOneFileToMerge = "More than one file is required for merging action";
        public const string GetLicenseToMergeMorePages = "Better take the license to merge more than 4 pages";
        // WWE controller messages
        public const string WrestlerNotInRoster = "Wrestler not available in the roster !";
        public const string RosterGenerated = "Roster generated !";
        public const string WrestlerNotProvided = "Wrestler's name is not provided for the match card !";
        public const string MatchCardGenerated = "Match card generated successfully !";
        // Comparison messages
        public const string FilesRequiredForComparison = "Both files must be provided for comparison !";
        public const string AsposeComparisonSuccess = "Aspose comparisons performed successfully !";
        public const string CustomComparisonSuccess = "Difference matching performed successfully !";
        
    }
}
