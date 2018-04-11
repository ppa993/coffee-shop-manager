import { Component, OnInit, ViewChild } from '@angular/core';
import { environment as env } from '@env/environment';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { MatPaginator, MatTableDataSource, MatDialog } from '@angular/material';
import { MoveTableDialogComponent, PaymentDialogComponent } from '@app/shared'
import { Product, ProductCollection, Table } from '@app/models';
import { TableService, InvoiceService } from '@app/services';

@Component({
  selector: 'anms-manage-table',
  templateUrl: './manage-table.component.html',
  styleUrls: ['./manage-table.component.scss']
})
export class ManageTableComponent implements OnInit {
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
    private invoiceService: InvoiceService,
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
    this.tableService.getTableById(this.id)
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

  updateTableProduct(productID: number, action: number, targetID: number){
    this.tableService.updateTableProduct(this.table.id, productID, action, targetID)
    .subscribe(result => {
      if(result){
        this.getTable();
      }
    });
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); 
    filterValue = filterValue.toLowerCase(); 
    this.productSource.filter = filterValue;
  }

  moveProduct(product: any): void {
    let dialogRef = this.dialog.open(MoveTableDialogComponent, {
      width: '400px',
      data: {from: this.id, product: product}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result!=null)
      {
        this.updateTableProduct(product.product.id, 3, result);
      }
    });
  }

  moveTable() {
    let dialogRef = this.dialog.open(MoveTableDialogComponent, {
      data: {from: this.id}
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result!=null)
      {
        this.tableService.moveTable(this.table.id, result)
        .subscribe(result => {
          if(result){
            this.location.back();
          }
        })
      }
    });
  }

  createNewInvoice(){    
    let dialogRef = this.dialog.open(PaymentDialogComponent, {
      data: {tableID: this.table.id},
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result)
      {
        this.invoiceService.createNewInvoice(this.table.id)
        .subscribe(invoiceResult => {
          if (invoiceResult){
              this.location.back();
          }
        })
      }
    });
  }
}
