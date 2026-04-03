import { Injectable } from '@angular/core';
import { Expense } from '../models/expense';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StagingService {

  created: Expense[] = [];
  updated: Expense[] = [];
  deletedIds: number[] = [];

  openDrawer$ = new Subject<void>();

  clear() {
    this.created = [];
    this.updated = [];
    this.deletedIds = [];
  }
}