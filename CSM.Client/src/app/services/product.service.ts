import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';

import { Product } from '@app/models';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class ProductService {

  private productUrl = 'http://localhost:56720/api/products';

  constructor(private http: HttpClient) { }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.productUrl);
  }

  updateProduct(updateProduct: Product): Observable<boolean> {
    return this.http.put<boolean>(this.productUrl + '/' + updateProduct.id, updateProduct);
  }

  addNewProduct(newProduct: Product): Observable<boolean> {
    return this.http.post<boolean>(this.productUrl, newProduct);
  }

  removeProduct(id: number): Observable<boolean> {
    return this.http.delete<boolean>(this.productUrl + '/' + id);
  }

}
