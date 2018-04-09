import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { catchError, map, tap } from 'rxjs/operators';

import { Table } from '@app/modals/table';
import { Product } from '@app/modals/product';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class TableService {

  private tableUrl = 'http://localhost:56720/api/tables';
  private productUrl = 'http://localhost:56720/api/products';

  constructor(private http: HttpClient) { }

  getTables(): Observable<Table[]> {
    return this.http.get<Table[]>(this.tableUrl);
  }

  getTablebyId(id: string): Observable<Table> {
    return this.http.get<Table>(this.tableUrl + '/' + id);
  }

  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.productUrl);
  }

}
