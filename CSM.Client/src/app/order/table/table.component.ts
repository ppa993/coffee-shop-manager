import { Component, OnInit } from '@angular/core';
import { environment as env } from '@env/environment';
import { MatDialog } from '@angular/material/dialog';
import { TableService } from '@app/services/table.service';
import { Table } from '@app/modals/table';

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
    public dialog: MatDialog
  ) {}

  getTables(){
    this.tableService.getTables()
    .subscribe(tables => this.tables = tables);
  }
}
