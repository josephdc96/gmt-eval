import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {MatButtonModule, MatDialogModule, MatProgressBarModule, MatToolbarModule} from "@angular/material";
import { UploadComponent } from './upload/upload.component';

@NgModule({
  declarations: [
    AppComponent,
    UploadComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    MatToolbarModule,
    MatButtonModule,
    MatDialogModule,
    MatProgressBarModule
  ],
  entryComponents: [
    AppComponent, UploadComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
