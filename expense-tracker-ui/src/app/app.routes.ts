import { Routes } from '@angular/router';
import { ExpensesListComponent } from './components/expenses-list/expenses-list';
import { ExpenseFormComponent } from './components/expense-form/expense-form';

export const appRoutes: Routes = [
  { path: '', redirectTo: 'expenses', pathMatch: 'full' },
  { path: 'expenses', component: ExpensesListComponent },
  { path: 'expenses/new', component: ExpenseFormComponent },
  { path: 'expenses/:id/edit', component: ExpenseFormComponent },
  { path: '**', redirectTo: 'expenses' }
];
