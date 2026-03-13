import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ExpenseService } from '../../services/expense';
import { Expense } from '../../models/expense';

@Component({
  selector: 'app-expenses-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './expenses-list.html',
  styleUrls: ['./expenses-list.css']
})
export class ExpensesListComponent implements OnInit {
  expenses: Expense[] = [];
  loading = false;
  error = '';

  constructor(private service: ExpenseService, private router: Router) { }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = '';
    this.service.getAll().subscribe({
      next: (data) => {
        this.expenses = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.message ?? 'Failed to load expenses';
        this.loading = false;
      }
    });
  }

  create(): void {
    this.router.navigate(['/expenses/new']);
  }

  edit(exp: Expense): void {
    if (exp.id == null) return;
    this.router.navigate([`/expenses/${exp.id}/edit`]);
  }

  remove(exp: Expense): void {
    if (exp.id == null) return;
    if (!confirm('Delete this expense?')) return;
    this.service.delete(exp.id).subscribe({
      next: () => this.load(),
      error: (err) => (this.error = err?.message ?? 'Delete failed')
    });
  }
}
