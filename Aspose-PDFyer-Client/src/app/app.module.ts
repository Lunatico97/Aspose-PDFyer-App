import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ConverterComponent } from './converter/converter.component';
import { BrowserAnimationsModule, NoopAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { LoggerService } from './services/logger.service';
import { RouterService } from './services/router.service';
import { HTTPService } from './services/http.service';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { OptimizerComponent } from './optimizer/optimizer.component';
import { HomeComponent } from './home/home.component';
import { CompressorComponent } from './optimizer/compressor/compressor.component';
import { EncrypterComponent } from './optimizer/encrypter/encrypter.component';
import { DocConverterComponent } from './doc-converter/doc-converter.component';
import { ComparatorComponent } from './multi-ops/comparator/comparator.component';
import { GeneratorComponent } from './generator/generator.component';
import {MatSliderModule} from '@angular/material/slider';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import { MergerComponent } from './multi-ops/merger/merger.component';
import { MatchGeneratorComponent } from './generator/match-generator/match-generator.component';
import { MultiOpsComponent } from './multi-ops/multi-ops.component';
import { InvoiceGeneratorComponent } from './generator/invoice-generator/invoice-generator.component';
import { ReplacerComponent } from './multi-ops/replacer/replacer.component';
import { CustomGeneratorComponent } from './custom-generator/custom-generator.component';
import { RoundPipe } from './utilities/round.pipe';
import { RangePipe } from './utilities/range.pipe';
import { ServerDownComponent } from './server-down/server-down.component';
import { ErrorInterceptor } from './services/error.interceptor.service';

@NgModule({
  declarations: [
    AppComponent,
    ConverterComponent,
    OptimizerComponent,
    HomeComponent,
    CompressorComponent,
    EncrypterComponent,
    DocConverterComponent,
    ComparatorComponent,
    GeneratorComponent,
    MergerComponent,
    MatchGeneratorComponent,
    MultiOpsComponent,
    InvoiceGeneratorComponent,
    ReplacerComponent,
    CustomGeneratorComponent,
    RoundPipe,
    RangePipe,
    ServerDownComponent
  ],
  imports: [
    FormsModule,
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatSnackBarModule,
    MatSliderModule,
    MatProgressSpinnerModule,
    MatProgressBarModule,
  ],
  providers: [
    LoggerService, HTTPService, RouterService,
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
