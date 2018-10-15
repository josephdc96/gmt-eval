import {Component, OnInit} from '@angular/core';
import {MatDialogRef} from "@angular/material";
import {HttpClient, HttpEventType, HttpRequest} from "@angular/common/http";

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<UploadComponent>, private http: HttpClient) { }
  progress: number = 0;
  color: string = 'primary';

  ngOnInit() {
  }

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onOkClick(files): void {
    if (files.length === 0) {
      this.dialogRef.close();
    }

    const formData = new FormData();

    for (let file of files) {
      formData.append(file.name, file);
    }

    const uploadReq = new HttpRequest('POST', `api/AddData`, formData, {
      reportProgress: true,
    });

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round(100 * event.loaded / event.total);
        this.color = 'primary';
      }
      else if (event.type === HttpEventType.Response) {
        if (event.body === 'success') {
          this.dialogRef.close();
        }
        else {
          this.color = 'warn';
        }
      }
    })
  }

}
