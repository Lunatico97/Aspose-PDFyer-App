import { Component, OnInit } from '@angular/core';
import { LoggerService } from '../../services/logger.service';
import { HTTPService } from '../../services/http.service';
import { Configuration } from '../../config/globalConfig';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-comparator',
  templateUrl: './comparator.component.html',
  styleUrls: ['./comparator.component.scss']
})
export class ComparatorComponent implements OnInit {
  selectedFile1: File | null = null;
  selectedFile2: File | null = null;
  obtainedBlob!: Blob;
  asposeWay: boolean = false;
  downloadFilename!: string;
  download: boolean = false;
  loading: boolean = false;

  constructor(private _logger: LoggerService, private _http: HTTPService) { }

  ngOnInit(): void {
  }

  comparePdfsInCustomWay() {
    if (this.selectedFile1 != null && this.selectedFile2 != null) {
      this.loading = true;
      const formData = new FormData();
      formData.append('pdf1', this.selectedFile1);  
      formData.append('pdf2', this.selectedFile2);   
      this._http.sendPostRequest(formData, Configuration.comparePdfCustom)
        .subscribe({
          next: (response: any) => {
            this._logger.pop(response.message);
            if(response.success){
              this.downloadFilename = response.downloadFilename;
              this.onDownload();
            }
            this.loading = false;
          },
          error: () => {
            this._logger.log('Comparator', 'Failed to compare PDFs !');
          },
          complete: () => {
            this._logger.log('Comparator', 'PDF Comparator request complete !');
          }
        });
    }
  }

  comparePdfsInAsposeWay() {
    if (this.selectedFile1 != null && this.selectedFile2 != null) {
      this.loading = true;
      const formData = new FormData();
      formData.append('pdf1', this.selectedFile1);  
      formData.append('pdf2', this.selectedFile2);   
      this._http.sendPostRequest(formData, Configuration.comparePdfAspose)
        .subscribe({
          next: (response: any) => {
            this._logger.pop(response.message);
            if(response.success){
              this.downloadFilename = response.downloadFilename;
              this.onDownload();
            }
            this.loading = false;
          },
          error: () => {
            this._logger.log('Comparator', 'Failed to compare PDFs !');
          },
          complete: () => {
            this._logger.log('Comparator', 'PDF Comparator request complete !');
          }
        });
    }
  }

  comparePdfs(){
    if(!this.asposeWay){
      this.comparePdfsInCustomWay();
    }
    else this.comparePdfsInAsposeWay();
  }

  onFile1Selected(event: any) {
    if(event.target instanceof HTMLInputElement && event.target.files.length > 0)
    {
      this.selectedFile1 = event.target.files[0] as File;
    }
    this.download = false;
  }

  onFile2Selected(event: any) {
    if(event.target instanceof HTMLInputElement && event.target.files.length > 0)
    {
      this.selectedFile2 = event.target.files[0] as File;
    }
  }

  onDownload(){
    var params = new HttpParams().set("filename", this.downloadFilename)
                                 .append("type", "pdf");
    this._http.sendGetRequestForBlob(params, Configuration.fileDownloadEndpoint)
    .subscribe({
      next: (comparisonBlob) => {
        this.obtainedBlob = comparisonBlob;
        this.download = true;
      },
      error: () => {
        this._logger.log('Comparator', 'Failed to download comparison table !');
      },
      complete: () => {
        this._logger.log('Comparator', 'Comparison completed !');
      }
    });
  }

  downloadFile() {
    this._logger.pop('Comparison downloaded !');
    const downloadLink = document.createElement('a');
    const url = window.URL.createObjectURL(this.obtainedBlob);
    downloadLink.href = url; 
    downloadLink.download = `PDF_Checks.pdf`;
    downloadLink.click();
    window.URL.revokeObjectURL(url);
  }

  previewFile(){
    const url = window.URL.createObjectURL(this.obtainedBlob);
    window.open(url, '_blank');
  }

  reset()
  {
    this.selectedFile1 = null;
    this.selectedFile2 = null;
    this.download = false;
  }

}
