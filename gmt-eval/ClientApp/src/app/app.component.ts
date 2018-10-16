import { Component } from '@angular/core';
import {MatDialog} from "@angular/material";
import {UploadComponent} from "./upload/upload.component";
import { Chart } from 'chart.js';
import {HttpClient} from "@angular/common/http";
import {InternetData} from "./models/internet-data";
import {TrendlineData} from "./models/trendline-data";
import {HealthData} from "./models/health-data";

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
    this.chart.data = null;
    this.http.get<InternetData>('/api/FetchData/FetchInternetData').subscribe(data => {
      this.chart.name = 'Average Grade by Internet Access';
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
    this.chart.data = null;
    this.http.get<number[]>('/api/FetchData/FetchFailureData').subscribe(data => {
      this.chart.name = 'Average Grade by Number of Failures';
      this.chart.data = new Chart('canvas', {
        type: 'line',
        data: {
          labels: ['0', '1', '2', '3'],
          datasets: [{
            label: this.chart.name,
            data: data,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
              'rgba(54, 162, 235, 0.2)'
            ],
            borderColor: [
              'rgba(255,99,132,1)',
              'rgba(54, 162, 235, 1)'
            ],
            borderWidth: 1,
            fill: false
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

  fetchStudyTimeData(): void {
    this.chart.data = null;
    this.http.get<number[]>('/api/FetchData/FetchStudyTimeData').subscribe(data => {
      this.chart.name = 'Average grade based on study time';
      this.chart.data = new Chart('canvas', {
        type: 'line',
        data: {
          labels: ['<2 hours', '2-5 hours', '5-10 hours', '10+ hours'],
          datasets: [{
            label: this.chart.name,
            data: data,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
              'rgba(54, 162, 235, 0.2)'
            ],
            borderColor: [
              'rgba(255,99,132,1)',
              'rgba(54, 162, 235, 1)'
            ],
            borderWidth: 1,
            fill: false
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

  fetchAbsenceData(): void {
    this.chart.data = null;
    this.http.get<TrendlineData>('/api/FetchData/FetchAbsenceData').subscribe(data => {
      this.chart.name = 'Average grade based on number of absences';
      this.chart.data = new Chart('canvas', {
        type: 'line',
        data: {
          datasets: [{
            label: this.chart.name,
            data: data.data,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
            ],
            borderColor: [
              'rgba(255,99,132,1)',
            ],
            borderWidth: 1,
            fill: false
          },
            {
              label: 'Trend Data',
              data: data.trendData,
              backgroundColor: [
                'rgba(54, 162, 235, 0.2)'
              ],
              borderColor: [
                'rgba(54, 162, 235, 1)'
              ],
              borderWidth: 1,
              fill: false
            }]
        },
        options: {
          scales: {
            yAxes: [{
              ticks: {
                beginAtZero: true
              }
            }],
            xAxes: [{
              type: 'linear',
              ticks: {
                beginAtZero: true
              }
            }]
          }
        }
      })
    })
  }

  fetchHealthData(): void {
    this.chart.data = null;
    this.http.get<HealthData>('/api/FetchData/FetchHealthData').subscribe(data => {
      this.chart.name = 'Average grade based on overall health';
      this.chart.data = new Chart('canvas', {
        type: 'line',
        data: {
          labels: ['Very bad', 'Bad', 'OK', 'Good', 'Very good'],
          datasets: [{
            label: this.chart.name + ' - G1 Data',
            data: data.g1Data,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
            ],
            borderColor: [
              'rgba(255,99,132,1)',
            ],
            borderWidth: 1,
            fill: false
          },
            {
              label: this.chart.name + ' - G2 Data',
              data: data.g2Data,
              backgroundColor: [
                'rgba(54, 162, 235, 0.2)'
              ],
              borderColor: [
                'rgba(54, 162, 235, 1)'
              ],
              borderWidth: 1,
              fill: false
            },
            {
              label: this.chart.name + ' - G3 Data',
              data: data.g3Data,
              backgroundColor: [
                'rgba(255, 206, 86, 0.2)',
              ],
              borderColor: [
                'rgba(255, 206, 86, 1)',
              ],
              borderWidth: 1,
              fill: false
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

  fetchTransitData(): void {
    this.chart.data = null;
    this.http.get<number[]>('/api/FetchData/FetchTransitData').subscribe(data => {
      this.chart.name = 'Average grade based on travel time';
      this.chart.data = new Chart('canvas', {
        type: 'line',
        data: {
          labels: ['<15 min', '15-30 min', '30-60 min', '>60 min'],
          datasets: [{
            label: this.chart.name,
            data: data,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
            ],
            borderColor: [
              'rgba(255,99,132,1)',
            ],
            borderWidth: 1,
            fill: false
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
