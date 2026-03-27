import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ExpenseService } from '../../services/expense';
import { Expense } from '../../models/expense';
import { MATERIAL_MODULES } from '../../material';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription } from 'rxjs';
import { SelectionModel } from '@angular/cdk/collections';
import { ChangeDetectorRef } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-expense-list',
  standalone: true,
  imports: [CommonModule, ...MATERIAL_MODULES],
  templateUrl: './expenses-list.html',
  styleUrls: ['./expenses-list.css']
})
export class ExpenseListComponent implements OnInit, OnDestroy {

  dataSource = new MatTableDataSource<Expense>();

  expenses: Expense[] = [];

  displayedColumns: string[] = ['select','title','amt','category','date','actions'];

  loading = false;
  error = '';

  private expenseSub?: Subscription;

  selection = new SelectionModel<Expense>(true, []);

  searchText = '';

  constructor(
    private service: ExpenseService,
    private router: Router,
    private snackBar: MatSnackBar,
    private cd: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    // ✅ Improved filter (search across all fields)
    this.dataSource.filterPredicate = (data: Expense, filter: string) => {

      const combined = (
        data.title +
        data.category +
        data.amt +
        data.date
      ).toLowerCase();

      return combined.includes(filter);
    };

    this.load();
  }

  load(): void {

    this.loading = true;

    this.expenseSub = this.service.getAll().subscribe({

      next: (data: Expense[]) => {

        this.expenses = data;

        // ✅ IMPORTANT: bind to datasource
        this.dataSource.data = data;

        this.loading = false;
        this.cd.detectChanges();
      },

      error: (err: any) => {
        this.error = err.message;
        this.loading = false;
        this.cd.detectChanges();
      }

    });

  }

  // ✅ FILTER LOGIC
  applyFilter() {
    this.selection.clear(); // optional but good UX
    this.dataSource.filter = this.searchText;
  }

  search(event: Event) {
    this.searchText = (event.target as HTMLInputElement).value
      .trim()
      .toLowerCase();

    this.applyFilter();
  }

  create(): void {
    this.router.navigate(['/new']);
  }

  edit(expense: Expense): void {
    this.router.navigate(['/edit', expense.id]);
  }

  remove(expense: Expense): void {

    if (!confirm('Delete this expense?')) return;

    this.service.delete(expense.id!).subscribe({

      next: () => {

        this.snackBar.open(
          'Expense deleted successfully',
          'Close',
          { duration: 3000 }
        );

        this.load();
      },

      error: () => {

        this.snackBar.open(
          'Failed to delete expense',
          'Close',
          { duration: 3000 }
        );

      }

    });

  }

  /* ===================== SELECT OPERATIONS ===================== */

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length; // ✅ FIXED
    return numSelected === numRows;
  }

  toggleAllRows() {

    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data); // ✅ FIXED
  }

  deleteSelected() {

    const selected = this.selection.selected;

    if (selected.length === 0) {
      this.snackBar.open('No expenses selected', 'Close', { duration: 2000 });
      return;
    }

    if (!confirm(`Delete ${selected.length} expenses?`)) return;

    selected.forEach(exp => {
      this.service.delete(exp.id!).subscribe();
    });

    this.snackBar.open(
      `${selected.length} expenses deleted`,
      'Close',
      { duration: 3000 }
    );

    this.selection.clear();
    this.load();
  }

  ngOnDestroy(): void {

    if (this.expenseSub) {
      this.expenseSub.unsubscribe();
    }

  }

}