import { Component, OnInit } from '@angular/core';
import { environment as env } from '@env/environment';
import { MatDialog } from '@angular/material/dialog';
import { TableService } from '@app/services/table.service';
import { Table } from '@app/models';
import { PaymentDialogComponent } from '@app/shared';
import { InvoiceService } from '@app/services';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'anms-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class TableComponent implements OnInit {
  versions = env.versions;
  tables: Table[];

  ngOnInit() {
    this.getTables();
  }

  constructor(
    private tableService: TableService,
    private invoiceService: InvoiceService,
    public dialog: MatDialog
  ) {}

  getTables(){
    this.tableService.getTables()
    .subscribe(tables => this.tables = tables);
  }

  createNewInvoice(tableID: number){    
    let dialogRef = this.dialog.open(PaymentDialogComponent, {
      data: {tableID: tableID},
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        this.invoiceService.createNewInvoice(tableID)
        .subscribe(invoiceResult => {
          if (invoiceResult){
              this.getTables();
          }
        })
      }
    });
  }
}
