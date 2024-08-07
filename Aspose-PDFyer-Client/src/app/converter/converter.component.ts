
import { Component, Input } from '@angular/core';
import { HTTPService } from '../services/http.service';
import { LoggerService } from '../services/logger.service';

@Component({
  selector: 'app-converter',
  templateUrl: './converter.component.html',
  styleUrls: ['./converter.component.scss']
})
export class ConverterComponent {
  selectedFile: File | null = null;
  obtainedBlob!: Blob;
  download: boolean = false;
  loading: boolean = false;
  @Input() convertEndpoint!: string;
  @Input() successMessage!: string;
  @Input() title!: string;
  @Input() convertFrom!: string;
  @Input() convertTo!: string;

  constructor(private _logger: LoggerService, private _http: HTTPService) { }

  onFileSelected(event: any) {
    if(event.target instanceof HTMLInputElement && event.target.files.length > 0)
    {
      this.selectedFile = event.target.files[0] as File;
    }
  }

  onConvertCallback() {
    if (this.selectedFile != null) {
      this.loading = true;
      const formData = new FormData();
      formData.append('file', this.selectedFile);  
      this._http.sendPostRequestForBlob(formData, this.convertEndpoint)
        .subscribe({
          next: (convertedBlob) => {
            this.obtainedBlob = convertedBlob;
            this.download = true;
            this._logger.pop(this.successMessage);
            this.loading = false;
          },
          error: () => {
            this._logger.log('Converter', 'Failed to convert PDF to word file !');
          }
        });
    }
  }

  downloadFile() {
    const downloadLink = document.createElement('a');
    const url = window.URL.createObjectURL(this.obtainedBlob);
    downloadLink.href = url;
    downloadLink.download = `${this.selectedFile!.name.split('.')[0]}${this.convertTo}`;
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
