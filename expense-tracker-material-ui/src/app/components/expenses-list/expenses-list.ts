import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ExpenseService } from '../../services/expense';
import { Expense } from '../../models/expense';
import { MATERIAL_MODULES } from '../../material';
import { MatSnackBar } from '@angular/material/snack-bar';
import { map, Observable, Subscription } from 'rxjs';
import { SelectionModel } from '@angular/cdk/collections';
// import { ChangeDetectorRef } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { forkJoin } from 'rxjs';
import { StagingService } from '../../services/staging.service';
import { signal, computed } from '@angular/core';
import { catchError, of } from 'rxjs';
import { ExpenseFormComponent } from '../expense-form/expense-form';
import { StagingApiResult } from '../../models/staging-result';
import { MatMenuModule } from '@angular/material/menu';
import { SafeHtml, DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-expense-list',
  standalone: true,
  imports: [CommonModule, ...MATERIAL_MODULES, ExpenseFormComponent, MatMenuModule],
  templateUrl: './expenses-list.html',
  styleUrls: ['./expenses-list.css']
})
export class ExpenseListComponent implements OnInit, OnDestroy {

  isFormDirty = false;
  isDrawerOpen = false;
  selectedExpense: Expense | null = null;

  dataSource = new MatTableDataSource<Expense>();

  createdExpenses: Expense[] = [];
  updatedExpenses: Expense[] = [];
  deletedExpenseIds: number[] = [];

  expenses: Expense[] = [];

  displayedColumns: string[] = ['select', 'title', 'amt', 'category', 'date', 'actions'];

  // loading = false;
  // error = '';

  loading = signal(false);
  error = signal('');

  private expenseSub?: Subscription;
  private drawerSub?: Subscription;
  private saveSub?: Subscription;

  selection = new SelectionModel<Expense>(true, []);

  searchText:string = '';
  categoryFilter:string = '';
  categories: string[] = ['Food', 'Travel', 'Shopping', 'Bills']; // available categories

  constructor(
    private service: ExpenseService,
    private staging: StagingService,
    private router: Router,
    private snackBar: MatSnackBar,
    private sanitizer: DomSanitizer
    // private cd: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    // listen for drawer open events
    this.drawerSub = this.staging.openDrawer$.subscribe(() => {
      this.create();
    });

    // Load data from router state if available
    const nav = this.router.getCurrentNavigation();
    const state = nav?.extras.state as { expense: Expense };
    if (state?.expense) this.handleRouterExpense(state.expense);

    // Load frontend data only (no backend call)
    this.load();

    this.closeDrawer();
  }

  private handleRouterExpense(expense: Expense): void {
  if (!expense.id || expense.id === 0) {
    // New expense
    expense.id = 0;
    this.createdExpenses.push(expense);
    this.expenses.push(expense);
  } else {
    // Update existing expense
    const index: number = this.expenses.findIndex(e => e.id === expense.id);
    if (index !== -1) {
      this.expenses[index] = expense;
    } else {
      this.expenses.push(expense);
    }

    const alreadyUpdated: Expense | undefined = this.updatedExpenses.find(e => e.id === expense.id);
    if (!alreadyUpdated) {
      this.updatedExpenses.push(expense);
    }
  }

  // Remove deleted expenses from the list
  this.expenses = this.expenses.filter(e => !this.staging.deletedIds.includes(e.id!));

  // Refresh dataSource
  this.refreshDataSource();
}

highlight(text: string): SafeHtml {
  if (!this.searchText) return text;

  const escaped = this.searchText.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
  const regex = new RegExp(`(${escaped})`, 'gi');

  const newText = text.replace(regex, '<span class="highlight">$1</span>');

  // ✅ sanitize HTML
  return this.sanitizer.bypassSecurityTrustHtml(newText);
}

  onFormSubmit(expense: Expense) {

  const index = this.expenses.findIndex(e => e.id === expense.id);

  if (index !== -1) {
    this.expenses[index] = expense;
  } else {
    this.expenses.push(expense);
  }

  // ✅ force refresh
  this.refreshDataSource();

  // ✅ close drawer
  this.closeDrawer();
}

  load(): void {
  this.loading.set(true);

  // If we already have frontend data, just refresh the table
  if (this.expenses.length > 0) {
    this.refreshDataSource();
    this.loading.set(false);
    return;
  }

  // First-time load from backend
  this.expenseSub = this.service.getAll().subscribe({
    next: (data: Expense[]) => {
      this.expenses = data;
      this.refreshDataSource();
      this.loading.set(false);
    },
    error: (err: Error) => {
      this.error.set(err.message);
      this.loading.set(false);
    }
  });
}

  private refreshDataSource(): void {
  // Backend data: exclude deleted IDs
  const backendData: Expense[] = this.expenses
    .filter((e: Expense) => !this.staging.deletedIds.includes(e.id!)) // exclude staged deletions
    .map((e: Expense) => {
      const updated: Expense | undefined = this.staging.updated.find(u => u.id === e.id);
      return updated ? updated : e; // apply staged updates
    });

  // Created data: exclude any newly created expenses that have been removed
  const createdData: Expense[] = this.staging.created
    .filter((c: Expense) => 
      !this.staging.deletedIds.includes(c.id!) && // exclude if staged for deletion
      !backendData.some((b: Expense) => b === c || b.id === c.id) // avoid duplicates
    );

  this.dataSource.data = [...backendData, ...createdData];
}

  // ✅ FILTER LOGIC
  applyFilter(): void {
  this.selection.clear();

  this.dataSource.filterPredicate = (data: Expense, filter: string) => {
    const searchTextLower = this.searchText.trim().toLowerCase();
    const matchesText = data.title.toLowerCase().includes(searchTextLower);
    const matchesCategory = this.categoryFilter
      ? data.category === this.categoryFilter
      : true;

    return matchesText && matchesCategory;
  };

  this.dataSource.filter = '' + Math.random(); // triggers filter update
}

  search(event: Event) {
    this.searchText = (event.target as HTMLInputElement).value
      .trim()
      .toLowerCase();

    this.applyFilter();
  }

  create(): void {
    this.selectedExpense = null;
    this.isDrawerOpen = true;
  }

  edit(expense: Expense): void {
  this.selectedExpense = null;        // ✅ force change detection
  setTimeout(() => {
    this.selectedExpense = { ...expense }; // ✅ clone
    this.isDrawerOpen = true;
  });
}

  closeDrawer() {

  if (this.isFormDirty) {
    const confirmClose = confirm('You have unsaved changes. Close anyway?');
    if (!confirmClose) return;
  }

  this.isDrawerOpen = false;
  this.selectedExpense = null;
  this.isFormDirty = false;
}

  handleCancel() {
    this.closeDrawer();
  }

  remove(expense: Expense): void {

    // If it was newly created → remove from created list
    const createdIndex = this.staging.created.findIndex(e => e.id === expense.id);

    if (createdIndex !== -1) {
      this.staging.created.splice(createdIndex, 1);
    } else {
      this.staging.deletedIds.push(expense.id!);
    }

    // Remove from UI instantly
    this.expenses = this.expenses.filter(e => e.id !== expense.id);

    this.snackBar.open('Delete staged (not saved yet)', 'Close', {
      duration: 3000
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
    }
    else{
    this.selection.select(...this.dataSource.data); // ✅ FIXED
  }
}

  deleteSelected() {

    const selected = this.selection.selected;

    if (selected.length === 0) {
      this.snackBar.open('No expenses selected', 'Close', { duration: 3000 });
      return;
    }

    if (!confirm(`Delete ${selected.length} expenses?`)) return;

    selected.forEach(exp => this.remove(exp));

    this.selection.clear();
  }

saveAllChanges(): void {
  const created = this.staging.created;
  const updated = this.staging.updated;
  const deletedIds = this.staging.deletedIds;

  const upsertPayload = [...created, ...updated];
  const validDeletedIds = deletedIds.filter(id => id && id > 0);

  if (upsertPayload.length === 0 && validDeletedIds.length === 0) {
    this.snackBar.open('No changes to save', 'Close', { duration: 2000 });
    return;
  }

  const observables: Array<Observable<StagingApiResult>> = [];

  // 1. Handle Adds and Updates
  if (upsertPayload.length > 0) {
    observables.push(
      this.service.upsertMultiple(upsertPayload).pipe(
        map(res => ({ type: 'upsert', response: res } as StagingApiResult)),
        catchError(err => of({ type: 'upsert', error: true, message: err.message } as StagingApiResult))
      )
    );
  }

  // 2. Handle Deletions (The "Delete Thing")
  if (validDeletedIds.length > 0) {
    observables.push(
      this.service.deleteMultiple(validDeletedIds).pipe(
        map(res => ({ type: 'delete', response: res } as StagingApiResult)),
        catchError(err => of({ type: 'delete', error: true, message: err.message } as StagingApiResult))
      )
    );
  }

  // 3. Execute all together
  this.saveSub = forkJoin(observables).subscribe((results: StagingApiResult[]) => {
    const hasError = results.some(r => r.error || r.response === false);

    if (hasError) {
      this.snackBar.open('Some changes failed to save. Please check your connection.', 'Close', { duration: 4000 });
    } else {
      this.snackBar.open('All changes saved successfully!', 'Close', { duration: 3000 });
      this.staging.clear(); // Only clear if successful
      this.load(); // Refresh list from backend
    }
  });
}
  get hasUnsavedChanges(): boolean {
    return this.staging.created.length > 0 ||
      this.staging.updated.length > 0 ||
      this.staging.deletedIds.length > 0;
  }
  // totalChangesSignal = signal(0);

  // totalChangesSignal = computed(() => 
  //   this.staging.created.length +
  //   this.staging.updated.length +
  //   this.staging.deletedIds.length
  // );



  ngOnDestroy(): void {

    if (this.expenseSub) {
      this.expenseSub.unsubscribe();
    }
    if (this.drawerSub) {
      this.drawerSub.unsubscribe();
    }
    if (this.saveSub) {
      this.saveSub.unsubscribe();
    }

  }

}