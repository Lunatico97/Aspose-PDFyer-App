
import { Component } from '@angular/core';
import { LoggerService } from '../services/logger.service';

@Component({
  selector: 'app-optimizer',
  templateUrl: './optimizer.component.html',
  styleUrls: ['./optimizer.component.scss']
})
export class OptimizerComponent {
  selectedFile: File | null = null;
  imageQuality: number = 50;
  tailName: string = "";
  userPwd!: string;
  obtainedBlob!: Blob;
  download: boolean = false;

  constructor(private _logger: LoggerService) { }
}

