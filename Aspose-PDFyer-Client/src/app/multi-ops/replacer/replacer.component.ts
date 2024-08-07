import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Configuration } from 'src/app/config/globalConfig';
import { HTTPService } from 'src/app/services/http.service';
import { LoggerService } from 'src/app/services/logger.service';

@Component({
  selector: 'app-replacer',
  templateUrl: './replacer.component.html',
  styleUrls: ['./replacer.component.scss']
})
export class ReplacerComponent implements OnInit {
  selectedFile: File | null  = null;
  findText!: string;
  replaceText!: string;
  obtainedBlob!: Blob;
  download: boolean = false;
  downloadFileName!: string;
  loading: boolean = false;
  exactReplacement: boolean = true;

  constructor(private _logger: LoggerService, private _http: HTTPService) { }

  ngOnInit(): void {
  }

  findAndReplace(){
    if(this.selectedFile != null)
    {
      this.loading = true;
      const formdata = new FormData();
      formdata.append('file', this.selectedFile);
      var params = new HttpParams().set("findText", this.findText)
                                    .append("replaceText", this.replaceText)
                                    .append("exactReplacementFlag", this.exactReplacement);
      this._http.sendPostRequestForBlobWithParams(formdata, params, Configuration.findAndReplaceEndpoint)
        .subscribe({
          next: (replacedBlob) => {
            this.obtainedBlob = replacedBlob;
            this.download = true;
            this._logger.pop('Find & replace completed !');
            this.loading = false;
          },
          error: () => {
            this._logger.log('Converter', 'Failed to perform replacement !');
          }
        });
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
        this._logger.log('Replacer', 'Failed to download replaced PDF !');
      },
      complete: () => {
        this._logger.log('Replacer', 'Replaced PDF download completed !');
      }
    });
  }

  onFileSelected(event: any) {
    if(event.target instanceof HTMLInputElement && event.target.files.length > 0)
    {
      this.selectedFile = event.target.files[0] as File;
    }
  }

  downloadFile() {
    this._logger.pop('New PDF downloaded !');
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
    this.findText = "";
    this.replaceText = "";
    this.download = false;
  }

}
