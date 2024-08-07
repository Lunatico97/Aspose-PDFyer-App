import { Component, OnInit } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Configuration } from 'src/app/config/globalConfig';
import { HTTPService } from 'src/app/services/http.service';
import { LoggerService } from 'src/app/services/logger.service';

@Component({
  selector: 'app-invoice-generator',
  templateUrl: './invoice-generator.component.html',
  styleUrls: ['./invoice-generator.component.scss']
})
export class InvoiceGeneratorComponent implements OnInit {
  selectedFile: File | null = null;
  inputCity!: string;
  wrestler1!: string;
  wrestler2!: string;
  obtainedBlob!: Blob;
  uploaded: boolean = false;
  download: boolean = false;
  downloadFileName!: string;
  loading: boolean = false;

  constructor(private _logger: LoggerService, private _http: HTTPService) { }

  ngOnInit(): void {
  }

  uploadDataFile()
  {
    if (this.selectedFile != null) {
      const formData = new FormData();
      formData.append('dataFile', this.selectedFile);  
      this._http.sendPostRequest<any>(formData, Configuration.fileUploadEndpoint)
        .subscribe({
          next: (response: any) => {
            if(response.success)
            {
              this.uploaded = true;
              this._logger.pop(response.message);
            }
            else this._logger.log("Generator", response.message);
          },
          error: () => {
            this._logger.log('Generator', 'Failed to upload data file !');
          },
          complete: () => {
            this._logger.log('Generator', 'Uploaded data file successfully !');
          }
        });
    }
  }

  generatePdf() {
    this.loading = true;
    if (this.selectedFile != null) { 
      var params = new HttpParams().set("dataFilename", this.selectedFile.name)
                                   .append("location", this.inputCity);
      this._http.sendPostRequestWithQuery<any>({"params": params}, [], Configuration.generateInvoice)
        .subscribe({
          next: (response) => {
            if(response.success){
              this.download = true;
              this.downloadFileName = `${this.inputCity}_${response.date}.pdf`
              this.onDownload();
            }
            this.loading = false;
            this._logger.log('Generator', response.message);
            this._logger.pop(response.message, 5000);
          },
          error: () => {
            this._logger.log('Generator', 'Failed to generate invoice !');
          },
          complete: () => {
            this._logger.log('Generator', 'Generated invoice PDF !');
          }
        });
    }
  }

  onFileSelected(event: any) {
    if(event.target instanceof HTMLInputElement && event.target.files.length > 0)
    {
      this.selectedFile = event.target.files[0] as File;
    }
  }

  onDownload(){
    var params = new HttpParams().set("filename", this.downloadFileName)
                                 .append("type", "pdf");
    this._http.sendGetRequestForBlob(params, Configuration.fileDownloadEndpoint)
    .subscribe({
      next: (compressedBlob) => {
        this.obtainedBlob = compressedBlob;
        this.download = true;
      },
      error: () => {
        this._logger.log('Generator', 'Failed to download invoice !');
      },
      complete: () => {
        this._logger.log('Generator', 'Invoice download completed !');
      }
    });
  }

  downloadFile() {
    this._logger.pop('Invoice downloaded !');
    const downloadLink = document.createElement('a');
    const url = window.URL.createObjectURL(this.obtainedBlob);
    downloadLink.href = url; 
    downloadLink.download = this.downloadFileName;
    downloadLink.click();
    window.URL.revokeObjectURL(url);  
  }

  previewFile(){
    const url = window.URL.createObjectURL(this.obtainedBlob);
    window.open(url, '_blank');
  }

  reset()
  {
    this.selectedFile = null;
    this.uploaded = false;
    this.download = false;
    this.inputCity = "";
  }
}
