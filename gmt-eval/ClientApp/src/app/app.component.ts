import { Component } from '@angular/core';
import {MatDialog} from "@angular/material";
import {UploadComponent} from "./upload/upload.component";
import { Chart } from 'chart.js'
import {HttpClient} from "@angular/common/http";
import {InternetData} from "./models/internet-data";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  chart = {
    name: '',
    data: null
  };

  constructor(public dialog: MatDialog, private http: HttpClient) { }

  openDialog(): void {
    const dialogRef = this.dialog.open(UploadComponent, {
      width: '250px',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
    });
  }

  fetchInternetData(): void {
    this.http.get<InternetData>('/api/FetchData/FetchInternetData').subscribe(data => {
      this.chart.name = 'Average Grade by Internet Access';
      this.chart.data = null;
      this.chart.data = new Chart('canvas', {
        type: 'bar',
        data: {
          labels: ['With Internet', 'Without Internet'],
          datasets: [{
            label: this.chart.name,
            data: [data.withNet, data.withoutNet],
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
              'rgba(54, 162, 235, 0.2)'
            ],
            borderColor: [
              'rgba(255,99,132,1)',
              'rgba(54, 162, 235, 1)'
            ],
            borderWidth: 1
          }]
        },
        options: {
          scales: {
            yAxes: [{
              ticks: {
                beginAtZero:true
              }
            }]
          }
        }
      })
    });
  }

  fetchFailureData(): void {
    this.http.get<number[]>('/api/FetchData/FetchFailureData').subscribe(data => {
      this.chart.name = 'Average Grade by Number of Failures';
      this.chart.data = null;
      this.chart.data = new Chart('canvas', {
        type: 'line',
        data: {
          labels: ['0', '1', '2', '3'],
          datasets: [{
            label: this.chart.name,
            data: data,
            borderColor: [
              'rgba(255,99,132,1)',
            ],
            borderWidth: 1
          }]
        },
        options: {
          scales: {
            yAxes: [{
              ticks: {
                beginAtZero:true
              }
            }]
          }
        }
      })
    })
  }
}
