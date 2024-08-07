import { Component, OnInit } from '@angular/core';
import { Configuration } from '../../config/globalConfig';
import { LoggerService } from '../../services/logger.service';
import { HTTPService } from '../../services/http.service';
import { HttpErrorResponse, HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-merger',
  templateUrl: './merger.component.html',
  styleUrls: ['./merger.component.scss']
})
export class MergerComponent implements OnInit {
  selectedFiles: File[] = [];
  obtainedBlob!: Blob;
  asposeWay: boolean = false;
  downloadFilename!: string;
  download: boolean = false;
  loading: boolean = false;

  constructor(private _logger: LoggerService, private _http: HTTPService) { }

  ngOnInit(): void {
    this.selectedFiles = [];
  }

  mergePdfs() {
    if (this.selectedFiles != null && this.selectedFiles.length > 1) {
      if(this.selectedFiles.length > 4)
      {
        this._logger.pop('Sorry, the free version allows at most 4 pages !');
        return;
      }
      this.loading = true;
      const formData = new FormData();  
      for(let selectedFile of this.selectedFiles)
      {
        formData.append('files', selectedFile);
      } 
      this._http.sendPostRequestForBlob(formData, Configuration.mergeEndpoint)
        .subscribe({
          next: (mergedBlob) => {
            this.obtainedBlob = mergedBlob;
            this._logger.pop("Merge complete !");
            this.download = true;
            this.loading = false;
          },
          error: (error: HttpErrorResponse) => {
            this._logger.log(`Merger [${error.statusText}]`, error.error);
          },
          complete: () => {
            this._logger.log('Merger', 'PDF Merge request complete !');
          }
        });
    }
  }

  onFileSelected(event: any) {
    if(event.target instanceof HTMLInputElement && event.target.files.length > 0)
    {
      const files: FileList = event.target.files;
      if (files && files.length > 0) {
        for (let i = 0; i < files.length; i++) {
          this.selectedFiles.push(files.item(i)! as File); 
        }
      }
    }
  }

  downloadFile() {
    this._logger.pop('Merged PDF downloaded !');
    const downloadLink = document.createElement('a');
    const url = window.URL.createObjectURL(this.obtainedBlob);
    downloadLink.href = url; 
    downloadLink.download = `${this.selectedFiles[0].name.split('.')[0]}_Merged.pdf`;
    downloadLink.click();
    window.URL.revokeObjectURL(url);
  }

  previewFile(){
    const url = window.URL.createObjectURL(this.obtainedBlob);
    window.open(url, '_blank');
  }

  reset()
  {
    this.selectedFiles = [];
    this.download = false;
  }
}
