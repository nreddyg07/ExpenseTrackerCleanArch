import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Expense } from '../models/expense';
import { environment } from '../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class ExpenseService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Expense[]> {
    return this.http.get<Expense[]>(this.baseUrl);
  }

  getById(id: number): Observable<Expense> {
    return this.http.get<Expense>(`${this.baseUrl}/${id}`);
  }

  // Backend now returns bool for write operations
  create(expense: Expense): Observable<boolean> {
    return this.http.post<boolean>(this.baseUrl, expense);
  }

  update(expense: Expense): Observable<boolean> {
    return this.http.put<boolean>(`${this.baseUrl}/${expense.id}`, expense);
  }

  delete(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.baseUrl}/${id}`);
  }

  upsertMultiple(expenses: Expense[]): Observable<boolean> {
    // Corrected endpoint name to match your [HttpPost("upsert-multiple")]
    return this.http.post<boolean>(`${this.baseUrl}/upsert-multiple`, {
      expenses: expenses.map(e => ({
        ...e,
        id: e.id ?? 0
      }))
    });
  }

  deleteMultiple(ids: number[]): Observable<boolean> {
    // Corrected endpoint name to match your [HttpDelete("multipleDelete")]
    return this.http.delete<boolean>(`${this.baseUrl}/multipleDelete`, {
      body: ids
    });
  }
}