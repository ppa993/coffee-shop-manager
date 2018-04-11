import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { TableService } from '@app/services/table.service';
import { Table } from '@app/models';

@Component({
  selector: 'anms-move-table-dialog',
  templateUrl: './move-table-dialog.component.html',
  styleUrls: ['./move-table-dialog.component.scss']
})
export class MoveTableDialogComponent implements OnInit {

  tables: Table[];

  constructor(    
    private tableService: TableService,
    public dialogRef: MatDialogRef<MoveTableDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.tableService.getTables().subscribe(result =>{
      this.tables = result.filter(item => item.id != this.data.from);
    })
  }
}
