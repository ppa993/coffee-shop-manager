import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';
import { Invoice } from '@app/models';
import { InvoiceService } from '@app/services';

@Component({
  selector: 'anms-statistic',
  templateUrl: './statistic.component.html',
  styleUrls: ['./statistic.component.scss']
})
export class StatisticComponent implements OnInit {

  chart = [];
  result: Invoice[];

  constructor(
    private invoiceService: InvoiceService
  ) { }

  ngOnInit() {
    this.invoiceService.getInvoices()
      .subscribe(res => {

        let total = res.map(res => res.total)
        let alldates = res.map(res => res.createdDate)

        let weatherDates = []
        alldates.forEach((res) => {
          let jsdate = new Date(res)
          weatherDates.push(jsdate.toLocaleTimeString(['en'], { year: 'numeric', month: 'short', day: 'numeric'}))
        })

        this.chart = new Chart('canvas', {
          type: 'bar',
          data: {
            labels: weatherDates,
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
      })
  }

}
