import { Component, OnInit } from '@angular/core';
import { HttpParams } from '@angular/common/http';
import { Configuration } from 'src/app/config/globalConfig';
import { HTTPService } from 'src/app/services/http.service';
import { LoggerService } from 'src/app/services/logger.service';
import { CustomDAO } from './customDao';

@Component({
  selector: 'app-custom-generator',
  templateUrl: './custom-generator.component.html',
  styleUrls: ['./custom-generator.component.scss']
})
export class CustomGeneratorComponent implements OnInit {

  selectedFile: File | null = null;
  title: string = "Diwas Enterprises";
  footer!: string;
  tableTopMargin: number = 50;
  tableLeftMargin: number = 10;
  tableFontSize: number = 8;
  obtainedBlob!: Blob;
  uploaded: boolean = false;
  download: boolean = false;
  downloadFileName!: string;
  loading: boolean = false;
  customHeaders : string[] = [];
  selectedHeaders: {[key: string]: boolean} = {};
  forceHeaders: string[] = [];

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
              this.getCustomHeaders();
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

  getCustomHeaders(){
    if(this.selectedFile != null)
    {
      const params = new HttpParams().set('filename', this.selectedFile.name);
      this._http.sendMultiGetRequest<string>({"params": params}, Configuration.getCustomHeadersEndpoint)
        .subscribe({
          next: (response: string[]) => {
            this.customHeaders = response;
            this.selectedHeaders = {};
            for (const header of this.customHeaders) {
              this.selectedHeaders[header] = true;
            }
          },
          error: () => {
            this._logger.log('Generator', 'Failed to get headers');
          },
          complete: () => {
            this._logger.log('Generator', 'Data headers request completed !');
          }
        });
    }
  }

  submitSelectedHeaders() {
    const selected = Object.keys(this.selectedHeaders).filter(key => this.selectedHeaders[key]);
    this.forceHeaders = selected;
    console.log('Selected headers:', selected);
  }

  generateCustomPdf() {
    this.submitSelectedHeaders();
    this.loading = true;
    if (this.selectedFile != null) { 
      var dao : CustomDAO = {
        Title: this.title,
        Headers: this.forceHeaders ,
        Footer: this.footer,
        Filename: this.selectedFile.name,
        TableFontSize: this.tableFontSize, RelativeTableX: this.tableLeftMargin, RelativeTableY: this.tableTopMargin
      }
      this._http.sendPostRequest<any>(dao, Configuration.generateCustom)
        .subscribe({
          next: (response) => {
            if(response.success){
              this.download = true;
              this.downloadFileName = `${this.title}_${response.date}.pdf`
              this.onDownload();
            }
            this.loading = false;
            this._logger.log('Generator', response.message);
            this._logger.pop(response.message, 5000);
          },
          error: () => {
            this._logger.log('Generator', 'Failed to generate custom PDF !');
          },
          complete: () => {
            this._logger.log('Generator', 'Generated custom PDF !');
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
        this._logger.log('Generator', 'Failed to download PDF !');
      },
      complete: () => {
        this._logger.log('Generator', 'PDF download completed !');
      }
    });
  }

  downloadFile() {
    this._logger.pop('Custom PDF downloaded !');
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
  }
}
