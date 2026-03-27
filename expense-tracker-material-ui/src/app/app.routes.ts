import { Routes } from '@angular/router';
import { ExpenseListComponent } from './components/expenses-list/expenses-list';
import { ExpenseFormComponent } from './components/expense-form/expense-form';

export const routes: Routes = [

  { path:'', redirectTo:'expenses', pathMatch:'full' },
  { path:'expenses', component:ExpenseListComponent },
  { path:'new', component:ExpenseFormComponent },
  { path:'edit/:id', component:ExpenseFormComponent }
];