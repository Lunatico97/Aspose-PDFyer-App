import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Configuration } from 'src/app/config/globalConfig';
import { HTTPService } from 'src/app/services/http.service';
import { LoggerService } from 'src/app/services/logger.service';

@Component({
  selector: 'app-compressor',
  templateUrl: './compressor.component.html',
  styleUrls: ['./compressor.component.scss']
})
export class CompressorComponent implements OnInit {
  selectedFile: File | null = null;
  imageQuality: number = 50;
  tailName: string = "Compressed";
  obtainedBlob!: Blob;
  loading: boolean = false;
  download: boolean = false;

  constructor(private _logger: LoggerService, private _http: HTTPService) { }

  ngOnInit(): void {
  }

  compressPdf() {
    this.loading = true;
    if (this.selectedFile != null) {
      const formData = new FormData();
      formData.append('file', this.selectedFile);  
      const params = new HttpParams().set('imageQuality', this.imageQuality);
      this._http.sendPostRequestForBlobWithParams(formData, params, Configuration.compressEndpoint)
        .subscribe({
          next: (compressedBlob) => {
            this.obtainedBlob = compressedBlob;
            this.download = true;
            this._logger.pop('Compressed file obtained !');
            this.loading = false;
          },
          error: () => {
            this._logger.log('Compressor', 'Failed to compress file !');
          },
          complete: () => {
            this._logger.log('Compressor', 'Obtained compressed file !');
          }
        });
      this.tailName = "Compressed";
    }
  }

  onFileSelected(event: any) {
    if(event.target instanceof HTMLInputElement && event.target.files.length > 0)
    {
      this.selectedFile = event.target.files[0] as File;
    }
  }

  downloadFile() {
    const downloadLink = document.createElement('a');
    const url = window.URL.createObjectURL(this.obtainedBlob); 
    downloadLink.href = url; 
    downloadLink.download = `${this.selectedFile!.name.split('.')[0]}_${this.tailName}.pdf`;
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
    this.download = false;
  }
}
