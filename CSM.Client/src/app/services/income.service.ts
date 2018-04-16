import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Income } from '@app/models';
import { environment as env } from '@env/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable()
export class IncomeService {

  private incomeUrl = env.apiUrl + 'income';

  constructor(
    private http: HttpClient
  ) { }


  getIncomeByType(type: number, time: Date): Observable<Income[]> {
    return this.http.post<Income[]>(this.incomeUrl + '/' + type, time);
  }

}
