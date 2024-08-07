import { Component, OnInit } from '@angular/core';
import { Configuration } from '../config/globalConfig';

@Component({
  selector: 'app-doc-converter',
  templateUrl: './doc-converter.component.html',
  styleUrls: ['./doc-converter.component.scss']
})
export class DocConverterComponent implements OnInit {

  convertPdfToWordEndpoint!: string;
  convertWordToPdfEndpoint!: string;
  convertXlsxToPdfEndpoint!: string;
  convertXlsxToWordEndpoint!: string;

  constructor() { }

  ngOnInit(): void {
    this.convertPdfToWordEndpoint = Configuration.convertPdfToWordEndpoint;
    this.convertWordToPdfEndpoint = Configuration.convertWordToPdfEndpoint;
    this.convertXlsxToPdfEndpoint = Configuration.convertXlsxToPdfEndpoint;
    this.convertXlsxToWordEndpoint = Configuration.convertXlsxToWordEndpoint;
  }

}
