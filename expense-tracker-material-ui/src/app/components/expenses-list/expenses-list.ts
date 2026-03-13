import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ExpenseService } from '../../services/expense';
import { Expense } from '../../models/expense';
import { MATERIAL_MODULES } from '../../material';
import { ChangeDetectorRef } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-expense-list',
  standalone: true,
  imports: [CommonModule,...MATERIAL_MODULES],
  templateUrl: './expenses-list.html',
  styleUrls: ['./expenses-list.css']
})
export class ExpenseListComponent implements OnInit {

  expenses: Expense[] = [];
  displayedColumns: string[] = ['title','amt','category','date','actions'];

  loading = false;
  error = '';

  constructor(
    private service: ExpenseService,
    private router: Router,
    private snackBar:MatSnackBar,
    private cd:ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {

    this.loading = true;

    this.service.getAll().subscribe({
      next: (data: Expense[]) => {
        console.log("DATA FROM API:", data);
        this.expenses = data;
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err: any) => {
        console.error("API ERROR:", err);
        this.error = err.message;
        this.loading = false;
        this.cd.detectChanges();
      }
    });
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

      this.load(); // reload expenses

    },

    error: (err) => {
      this.error = err.message;

      this.snackBar.open(
        'Failed to delete expense',
        'Close',
        { duration: 3000 }
      );
    }

  });

}
}