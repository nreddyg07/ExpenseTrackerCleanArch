import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExpenseService } from '../../services/expense';
import { MATERIAL_MODULES } from '../../material';
import { MatSnackBar } from '@angular/material/snack-bar';
import { firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_MODULES],
  templateUrl: './expense-form.html',
  styleUrls: ['./expense-form.css']
})
export class ExpenseFormComponent implements OnInit, OnDestroy {
  originalValue: any;
  isEditMode = false;
  form: FormGroup;
  id?: number;

  constructor(
    private fb: FormBuilder,
    private service: ExpenseService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {

    /* Form initialized in constructor
       so we don't need form! */
    this.form = this.fb.group({
      id: [null],
      title: ['', Validators.required],
      amt: [0, [Validators.required, Validators.min(0.01)]],
      category: ['', Validators.required],
      date: ['', Validators.required]
    });

  }

  /* ngOnInit now calls a separate function */
  async ngOnInit() {

  this.initForm();

  const id = this.route.snapshot.paramMap.get('id');

  if (id) {

    this.isEditMode = true;
    this.id = +id;

    const exp = await firstValueFrom(
      this.service.getById(this.id)
    );

    this.form.patchValue(exp);

    /* store original trimmed value */
    this.originalValue = this.getTrimmedValue(exp);

  }

}

getTrimmedValue(data: any) {
  return {
    title: data.title?.trim(),
    amt: data.amt,
    category: data.category?.trim(),
    date: data.date
  };
}

  /* Reviewer asked to move logic here */
  async initForm() {

    const id = this.route.snapshot.paramMap.get('id');

    if (id) {

      this.id = +id;

      /* await instead of subscribe */
      const expense = await firstValueFrom(
        this.service.getById(this.id)
      );

      this.form.patchValue(expense);

    }

  }

  hasRealChanges(): boolean {

  if (!this.isEditMode) return true;

  const current = this.getTrimmedValue(this.form.value);

  return JSON.stringify(current) !== JSON.stringify(this.originalValue);

}

  /* reviewer asked this to be async */
  async submit() {

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload = this.getTrimmedValue(this.form.value);

    try {

      if (this.id) {
        if(!this.hasRealChanges()) {
          this.snackBar.open(
            'No changes detected','Close',{duration: 3000}
          );
          return;
        }
        await firstValueFrom(
          this.service.update(payload)
        );
      }
      else {
        await firstValueFrom(
          this.service.create(payload)
        );
      }

      this.snackBar.open(
        this.id ? 'Expense updated successfully' : 'Expense created successfully',
        'Close',
        { duration: 3000 }
      );

      this.router.navigate(['/expenses']);

    }
    catch {

      this.snackBar.open(
        'Operation failed',
        'Close',
        { duration: 3000 }
      );

    }

  }

  cancel() {

  /* if editing and changes exist */
  if (this.id && this.hasRealChanges()) {

    const confirmLeave = confirm(
      'You have unsaved changes. Do you really want to cancel?'
    );

    if (!confirmLeave) return;

  }

  this.router.navigate(['/expenses']);

}

  ngOnDestroy() {}
}