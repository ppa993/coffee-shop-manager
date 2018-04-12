import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { Income } from '@app/models';
import { IncomeService } from '@app/services';
import { FormControl } from '@angular/forms';
import { create } from 'domain';

@Component({
  selector: 'anms-statistic',
  templateUrl: './statistic.component.html',
  styleUrls: ['./statistic.component.scss']
})
export class StatisticComponent implements OnInit {

  chart = [];
  result: Income[];
  selectedType: number;
  today = new Date();
  date = new FormControl(this.today);
  month = this.today.getMonth()+1;
  year = this.today.getFullYear();

  types = [
    { value: 1, viewValue: 'Day' },
    { value: 2, viewValue: 'Month' },
    { value: 3, viewValue: 'Year' }
  ];

  months = [
    { value: 1, viewValue: 'January' },
    { value: 2, viewValue: 'February' },
    { value: 3, viewValue: 'March' },
    { value: 4, viewValue: 'April' },
    { value: 5, viewValue: 'May' },
    { value: 6, viewValue: 'June' },
    { value: 7, viewValue: 'July' },
    { value: 8, viewValue: 'August' },
    { value: 9, viewValue: 'September' },
    { value: 10, viewValue: 'October' },
    { value: 11, viewValue: 'November' },
    { value: 12, viewValue: 'December' },
  ];

  constructor(
    private incomeService: IncomeService
  ) { }

  ngOnInit() {}

  getStatistic(){
    let time = new Date();
    if (this.selectedType == 1) time = this.date.value 
    if (this.selectedType == 2) time = new Date(this.today.getFullYear(), this.month) 
    if (this.selectedType == 3) time = new Date(this.year, 1);

    
    this.incomeService.getIncomeByType(this.selectedType, time)
    .subscribe(res => this.createChart(res));
  }


  createChart(res: any) {
    let total = res.map(res => res.total);
    let labels = res.map(res => res.label);

    this.chart = new Chart('canvas', {
      type: 'line',
      data: {
        labels: labels,
        datasets: [
          {
            data: total,
            backgroundColor: '#ff6384',
            borderColor: '#3cba9f',
            fill: false
          },
        ]
      },
      options: {
        legend: {
          display: false
        },
        scales: {
          xAxes: [{
            display: true
          }],
          yAxes: [{
            display: true
          }]
        }
      }
    })
  }

}
