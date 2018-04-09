import { Component, OnInit, ViewChild } from '@angular/core';
import { environment as env } from '@env/environment';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { MatPaginator, MatTableDataSource, MatDialog } from '@angular/material';
import { MoveTableDialogComponent } from '../../shared/dialogs/move-table-dialog/move-table-dialog.component'
import { TableService } from '@app/services/table.service';
import { Product } from '@app/modals/product';
import { Table } from '@app/modals/table';

@Component({
  selector: 'anms-manage-table',
  templateUrl: './manage-table.component.html',
  styleUrls: ['./manage-table.component.scss']
})
export class ManageTableComponent implements OnInit {
  versions = env.versions;
  id: string;
  table: Table;
  products: Product[];
  tableColumns = ['name', 'quantity', 'action'];
  productColumns = ['name', 'price', 'action'];
  tableSource = new MatTableDataSource();
  productSource = new MatTableDataSource();

  constructor(
    private route: ActivatedRoute,
    private location: Location,
    private tableService: TableService,
    public dialog: MatDialog
  ) {}

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.getTable();
    this.getProducts();
  }

  @ViewChild(MatPaginator) paginator: MatPaginator; 

  ngAfterViewInit() {
    this.tableSource.paginator = this.paginator;
  }

  getTable(){
    this.tableService.getTablebyId(this.id)
    .subscribe(table => {
      this.table = table;
      this.tableSource.data = table.products;
    });
  }

  getProducts(){
    this.tableService.getProducts()
    .subscribe(products => {
      this.products = products;
      this.productSource.data = products;
    });
  }

  add(element: any){
    element.quantity++;
  }

  remove(element: any){
    element.quantity--;
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); 
    filterValue = filterValue.toLowerCase(); 
    this.productSource.filter = filterValue;
  }

  moveTable(): void {
    let dialogRef = this.dialog.open(MoveTableDialogComponent, {
      width: '250px',
      data: {from: this.id}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result!=null)
        alert(result);
    });
  }

}
