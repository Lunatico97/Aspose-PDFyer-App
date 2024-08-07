import { Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { OptimizerComponent } from "./optimizer/optimizer.component";
import { EncrypterComponent } from "./optimizer/encrypter/encrypter.component";
import { CompressorComponent } from "./optimizer/compressor/compressor.component";
import { DocConverterComponent } from "./doc-converter/doc-converter.component";
import { ComparatorComponent } from "./multi-ops/comparator/comparator.component";
import { GeneratorComponent } from "./generator/generator.component";
import { MultiOpsComponent } from "./multi-ops/multi-ops.component";
import { MatchGeneratorComponent } from "./generator/match-generator/match-generator.component";
import { InvoiceGeneratorComponent } from "./generator/invoice-generator/invoice-generator.component";
import { ReplacerComponent } from "./multi-ops/replacer/replacer.component";
import { CustomGeneratorComponent } from "./custom-generator/custom-generator.component";
import { ServerDownComponent } from "./server-down/server-down.component";

export const routes: Routes = [
    {path: '', redirectTo: 'home', pathMatch: 'full'},
    {path: 'home', component: HomeComponent},
    {path: 'converter', component: DocConverterComponent},
    {path: 'optimizer', component: OptimizerComponent, children:[
        {path: 'compress', component: CompressorComponent},
        {path: 'encrypt', component: EncrypterComponent}
    ]},
    {path: 'multi-ops', component: MultiOpsComponent, children:[
        {path: 'comparator', component: ComparatorComponent},
        {path: 'generator', component: GeneratorComponent},
        {path: 'replacer', component: ReplacerComponent}
    ]},
    {path: 'generator', component: GeneratorComponent, children:[
        {path: 'generate-match', component: InvoiceGeneratorComponent},
        {path: 'generate-invoice', component: MatchGeneratorComponent}
    ]},
    {path: 'custom-generator', component: CustomGeneratorComponent},
    {path: 'server-down', component: ServerDownComponent}
];