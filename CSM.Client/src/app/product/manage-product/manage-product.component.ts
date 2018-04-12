import { Component, OnInit } from '@angular/core';
import { MatTableDataSource, MatSnackBar } from '@angular/material';
import { Chart } from 'chart.js';
import { Product } from '@app/models';
import { ProductService } from '@app/services';
import { create } from 'domain';

@Component({
  selector: 'anms-manage-product',
  templateUrl: './manage-product.component.html',
  styleUrls: ['./manage-product.component.scss']
})
export class ManageProductComponent implements OnInit {

  products: Product[];
  editingProduct: Product;
  editing = false;
  addNew = true;
  adding = false;
  productColumns = ['name', 'price', 'action'];
  productSource = new MatTableDataSource();

  constructor(
    private productService: ProductService,
    public snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.getProducts();
  }


  getProducts(){
    this.productService.getProducts()
    .subscribe(products => {
      this.products = products;
      this.productSource.data = products;
    });
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); 
    filterValue = filterValue.toLowerCase(); 
    this.productSource.filter = filterValue;
  }

  openUpdate(product: Product){
    this.editingProduct = product;
    this.addNew = false;
    this.editing = true;
    this.adding = false;
  }

  updateProduct(){
    this.productService.updateProduct(this.editingProduct)
    .subscribe(res => {
      this.cancel();
      this.getProducts();
      this.snackBar.open("Edited successfully!!", null, {
        duration: 2000,
      });
    })
  }

  addNewProduct(){
    this.productService.addNewProduct(this.editingProduct)
    .subscribe(res => {
      this.cancel();
      this.getProducts();
      this.snackBar.open("Added successfully!!", null, {
        duration: 2000,
      });
    })
  }

  removeProduct(id: number){
    this.productService.removeProduct(id)
    .subscribe(res => {
      this.getProducts();
      this.snackBar.open("Removed successfully!!", null, {
        duration: 2000,
      });
    })
  }

  cancel(){
    this.editingProduct = null;
    this.addNew = true;
    this.adding = false;
    this.editing = false;
  }

  openAdd(){
    this.editingProduct = new Product();
    this.adding = true;
    this.addNew = false;
    this.editing = false;
  }

}
