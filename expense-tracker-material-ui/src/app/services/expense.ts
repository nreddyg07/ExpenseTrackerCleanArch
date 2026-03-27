import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Expense } from '../models/expense';
import { ApiResponse } from '../models/api-response';
import { environment } from '../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Expense[]> {
  return this.http
    .get<ApiResponse<Expense[]>>(this.baseUrl)
    .pipe(map(res => res.data!));
}

getById(id: number): Observable<Expense> {
  return this.http
    .get<ApiResponse<Expense>>(`${this.baseUrl}/${id}`)
    .pipe(map(res => res.data!));
}

create(expense: Expense): Observable<Expense> {
  return this.http
    .post<ApiResponse<Expense>>(this.baseUrl, expense)
    .pipe(map(res => res.data!));
}

update(expense: Expense): Observable<Expense> {
  return this.http
    .put<ApiResponse<Expense>>(`${this.baseUrl}/${expense.id}`, expense)
    .pipe(map(res => res.data!));
}

delete(id: number): Observable<void> {
  return this.http
    .delete<ApiResponse<void>>(`${this.baseUrl}/${id}`)
    .pipe(map(res => res.data!));
}

upsertMultiple(expenses: Expense[]): Observable<any> {
  return this.http
    .post<ApiResponse<any>>(`${this.baseUrl}/upsertMultiple`, {
      expenses: expenses.map(e => ({
        ...e,
        id: e.id ?? 0
      }))
    })
    .pipe(map(res => res.data!));
}

deleteMultiple(ids: number[]): Observable<void> {
  return this.http
    .delete<ApiResponse<void>>(`${this.baseUrl}/multipleDelete`, {
      body: ids
    })
    .pipe(map(res => res.data!));
}

}