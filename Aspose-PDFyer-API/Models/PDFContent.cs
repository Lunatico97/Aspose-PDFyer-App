using AsposeTriage.Structures;

namespace AsposeTriage.Models
{
    public class PDFContent
    {  
        public string Filename { get; set;} = string.Empty;
        public Header InputHeader{ get; set; } = new Header();
        public Content InputContent { get; set; } = new Content();
    }
}

/*
     {
       "filename": "WWE",
       "inputHeader": {
                         "title": "Survivor Series",
                         "font": "Times New Roman",
                         "imagePath": "Input/wwe.jpg"
                      },
       "inputContent": {
                         "text": "It's time to see who remains the lone survivor !",
                         "font": "Calibri",
                         "table": {
                             "headerRows": ["Raw", "SmackDown!"],
                             "dataRows": [
                                 ["John Cena", "Edge"],
                                 ["Randy Orton", "Rey Mysterio"],
                                 ["Triple H", "Batista"],
                                 ["Kane", "JBL"],
                                 ["Shawn Michaels", "Undertaker"]
                             ]
                         }
                      }
     }
 */
