import { Component, OnInit, OnDestroy, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExpenseService } from '../../services/expense';
import { MATERIAL_MODULES } from '../../material';
import { MatSnackBar } from '@angular/material/snack-bar';
import { firstValueFrom } from 'rxjs';
import { Expense } from '../../models/expense';
import { StagingService } from '../../services/staging.service';

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_MODULES],
  templateUrl: './expense-form.html',
  styleUrls: ['./expense-form.css']
})
export class ExpenseFormComponent implements OnInit, OnDestroy {

  originalValue!: Expense;
  isEditMode = false;
  form: FormGroup;
  id?: number;

  // ✅ Renamed to avoid conflict
  @Input() expense: Expense | null = null;
  @Input() isDrawerMode = false;
  @Output() saveExpense = new EventEmitter<Expense>();
  @Output() cancelEvent = new EventEmitter<void>();
  @Output() formDirty = new EventEmitter<boolean>();

  constructor(
    private fb: FormBuilder,
    private service: ExpenseService,
    private staging: StagingService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {

    this.form = this.fb.group({
      id: [null],
      title: ['', Validators.required],
      amt: [0, [Validators.required, Validators.min(0.01)]],
      category: ['', Validators.required],
      date: ['', Validators.required]
    });
  }

  // ================= INIT =================

  async ngOnInit() {
    // ✅ Router mode
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.id = +id;
      this.isEditMode = true;

      const expense = await firstValueFrom(this.service.getById(this.id));

      this.form.patchValue({
        ...expense,
        date: expense.date ? new Date(expense.date) : ''
      });

      this.originalValue = this.getTrimmedValue(expense);
    }
  }

  ngOnChanges(changes: SimpleChanges) {

  if (changes['expense'] && this.expense) {

    this.isEditMode = true;
    this.id = this.expense.id;

    this.form.patchValue({
      ...this.expense,
      date: this.expense.date ? new Date(this.expense.date) : ''
    });

    this.originalValue = this.getTrimmedValue(this.expense);
  }

  // 🟢 CREATE MODE RESET
  if (changes['expense'] && !this.expense) {
    this.isEditMode = false;
    this.id = undefined;
    this.form.reset();
  }
}

  // ================= HELPERS =================

  formatLocalDate(date: Date): string {
    const d = new Date(date);
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  getTrimmedValue(data: Expense): Expense {
    return {
      id: data.id,
      title: data.title?.trim(),
      amt: data.amt,
      category: data.category?.trim(),
      date: this.formatLocalDate(new Date(data.date))
    };
  }

  // ================= CHANGE DETECTION =================

  hasRealChanges(): boolean {
    if (!this.isEditMode) return true;

    const current = this.getTrimmedValue(this.form.value);
    return JSON.stringify(current) !== JSON.stringify(this.originalValue);
  }

  // ================= ROUTER SAVE (UNCHANGED BEHAVIOR) =================

  save() {

    const value = this.form.value;

    const expense: Expense = {
      id: this.id,
      title: value.title.trim(),
      category: value.category.trim(),
      amt: value.amt,
      date: this.formatLocalDate(value.date)
    };

    this.router.navigate(['/'], {
      state: { expense }
    });
  }

  // ================= MAIN SUBMIT =================

  async submit() {

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload = this.getTrimmedValue(this.form.value);

    // 🟢 CREATE
    if (!this.id) {

      payload.id = 0; 
      this.staging.created.push(payload);

      this.snackBar.open('Expense staged (not saved yet)', 'Close', { duration: 3000 });
    }

    // 🟡 UPDATE
    else {

      if (!this.hasRealChanges()) {
        this.snackBar.open('No changes detected', 'Close', { duration: 3000 });
        return;
      }

      const createdIndex = this.staging.created.findIndex(e => e.id === this.id);

      if (createdIndex !== -1) {
        this.staging.created[createdIndex] = { ...payload, id: this.id };
      } else {

        const exists = this.staging.updated.find(e => e.id === this.id);

        if (!exists) {
          this.staging.updated.push({ ...payload, id: this.id });
        }
      }

      this.snackBar.open('Update staged (not saved yet)', 'Close', { duration: 3000 });
    }

    // ================= DRAWER MODE =================
    this.saveExpense.emit({
        ...payload,
        id: payload.id
      });

      this.form.reset();

    // ================= ROUTER MODE =================
    this.router.navigate(['/expenses']);
  }

  // ================= CANCEL =================

  cancelForm() {

    const hasChanges = this.form.dirty && this.hasRealChanges();

    if (hasChanges) {
      const confirmLeave = confirm(
        'You have unsaved changes. Do you really want to cancel?'
      );

      if (!confirmLeave) return;
    }

    // Drawer mode
    if (this.expense || !this.expense) {
      this.cancelEvent.emit();
      return;
    }

    // Router mode
    this.router.navigate(['/expenses']);
  }

  ngOnDestroy() {}
}