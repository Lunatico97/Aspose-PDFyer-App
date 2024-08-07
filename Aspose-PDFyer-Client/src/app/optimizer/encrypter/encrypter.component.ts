import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Configuration } from 'src/app/config/globalConfig';
import { HTTPService } from 'src/app/services/http.service';
import { LoggerService } from 'src/app/services/logger.service';

@Component({
  selector: 'app-encrypter',
  templateUrl: './encrypter.component.html',
  styleUrls: ['./encrypter.component.scss']
})
export class EncrypterComponent implements OnInit {
  selectedFile: File | null = null;
  tailName: string = "Encrypted";
  userPwd!: string;
  obtainedBlob!: Blob;
  loading: boolean = false;
  download: boolean = false;

  constructor(private _logger: LoggerService, private _http: HTTPService) { }

  ngOnInit(): void {
  }

  encryptPdf() {
    this.loading = true;
    if (this.selectedFile != null) {
      const formData = new FormData();
      formData.append('file', this.selectedFile); 
      var params = new HttpParams().set('userPwd', this.userPwd)
                                 .append('ownerPwd', this.userPwd.split('').reverse().join(''));
      this._http.sendPostRequestForBlobWithParams(formData, params, Configuration.encryptEndpoint)
        .subscribe({
          next: (encryptedBlob) => {
            this.obtainedBlob = encryptedBlob;
            this.download = true;
            this._logger.pop('Encrypted file obtained !');
            this.loading = false;
          },
          error: () => {
            this._logger.log('Encryptor', 'Failed to encrypt file !');
          }
        });
        this.tailName = "Encrypted";
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
    this.userPwd = "";
  }

}
