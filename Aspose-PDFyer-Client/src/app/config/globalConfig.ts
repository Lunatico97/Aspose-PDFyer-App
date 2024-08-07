
export class Configuration
{
    static asposeApiURL : string = "http://localhost:5241/api";
    static fileUploadEndpoint: string = "/file/upload";
    static fileDownloadEndpoint: string = "/file/download";
    static compressEndpoint: string = "/convert/compress";
    static encryptEndpoint: string = "/convert/encrypt";
    static mergeEndpoint: string = "/convert/merge";
    static convertPdfToWordEndpoint: string = "/convert/pdf-to-word";
    static convertWordToPdfEndpoint: string = "/convert/word-to-pdf";
    static convertXlsxToPdfEndpoint: string = "/convert/excel-csv-to-pdf";
    static convertXlsxToWordEndpoint: string = "/convert/excel-csv-to-word";
    static findAndReplaceEndpoint: string = "/convert/find-and-replace";
    static comparePdfAspose: string = "/bill/compare/aspose";
    static comparePdfCustom: string = "/bill/compare/custom";
    static generateInvoice: string = "/bill/generateBill";
    static generateCard: string = "/wwe/generateCard";
    static generateCustom: string = "/custom/generateCustom";
    static getCustomHeadersEndpoint: string = "/custom/getCustomDataHeaders";
}