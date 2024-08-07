import { Component, OnInit } from '@angular/core';
import { LoggerService } from '../../services/logger.service';
import { HTTPService } from '../../services/http.service';
import { Configuration } from '../../config/globalConfig';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-match-generator',
  templateUrl: './match-generator.component.html',
  styleUrls: ['./match-generator.component.scss']
})
export class MatchGeneratorComponent implements OnInit {
  wrestler1!: string;
  wrestler2!: string;
  obtainedBlob!: Blob;
  download: boolean = false;
  downloadFileName!: string;
  loading: boolean = false;

  constructor(private _logger: LoggerService, private _http: HTTPService) { }

  ngOnInit(): void {
  }

  generateMatchCard(){
    this.loading = true;
    var params = new HttpParams().set("wrestler1", this.wrestler1)
                                   .append("wrestler2", this.wrestler2);
    this._http.sendPostRequestWithQuery<any>({"params": params}, [], Configuration.generateCard)
        .subscribe({
          next: (response) => {
            if(response.success){
              this.download = true;
              this.downloadFileName = response.downloadFilename;
              this.onDownload();
            }
            this.loading = false;
            this._logger.log('Generator', response.message);
            this._logger.pop(response.message, 5000);
          },
          error: () => {
            this._logger.log('Generator', 'Failed to generate match card !');
          },
          complete: () => {
            this._logger.log('Generator', 'Generated match card PDF !');
          }
        });
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
        this._logger.log('Generator', 'Failed to download match card !');
      },
      complete: () => {
        this._logger.log('Generator', 'Match card download completed !');
      }
    });
  }

  downloadFile() {
    this._logger.pop('Match card downloaded !');
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
    this.wrestler1 = "";
    this.wrestler2 = "";
    this.download = false;
  }

}
