import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Table, Product } from '@app/models';
import { environment as env } from '@env/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class TableService {

  private tableUrl = env.apiUrl + 'tables';

  constructor(private http: HttpClient) { }

  getTables(): Observable<Table[]> {
    return this.http.get<Table[]>(this.tableUrl);
  }

  getTableById(id: string): Observable<Table> {
    return this.http.get<Table>(this.tableUrl + '/' + id);
  }

  moveTable(tableID: number, targetID: number): Observable<boolean> {
    return this.http.post<boolean>(this.tableUrl + '/'  + tableID, targetID, httpOptions);
  }

  updateTableProduct(tableID: number, productID: number, action: number, targetID: number): Observable<boolean> {
    let body = {productID: productID, action: action, targetID: targetID};
    return this.http.put<boolean>(this.tableUrl + '/' + tableID, body, httpOptions);
  }

}
