import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { TableService } from '@app/services';
import { Table, Product } from '@app/models';

@Component({
  selector: 'anms-payment-dialog',
  templateUrl: './payment-dialog.component.html',
  styleUrls: ['./payment-dialog.component.scss']
})
export class PaymentDialogComponent implements OnInit {

  table: Table;

  constructor(    
    private tableService: TableService,
    public dialogRef: MatDialogRef<PaymentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.getTable();
  }

  getTable(){
    this.tableService.getTableById(this.data.tableID)
    .subscribe(result => {
      this.table = result;
    })
  }
}
