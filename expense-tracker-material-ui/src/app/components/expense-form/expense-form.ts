import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExpenseService } from '../../services/expense';
import { MATERIAL_MODULES } from '../../material';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, ...MATERIAL_MODULES],
  templateUrl: './expense-form.html',
  styleUrls: ['./expense-form.css']
})
export class ExpenseFormComponent implements OnInit {
  form!: FormGroup;

  id?: number;

  constructor(
    private fb: FormBuilder,
    private service: ExpenseService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {

    const id = this.route.snapshot.paramMap.get('id');

    this.form = this.fb.group({
      id: [null],
      title: ['', Validators.required],
      amt: [0, [Validators.required, Validators.min(0.01)]],
      category: ['', Validators.required],
      date: ['', Validators.required]
    });

    if (id) {
      this.id = +id;
      this.service.getById(this.id).subscribe(exp => {
        this.form.patchValue(exp);
      });
    }
  }

  submit() {

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const payload = this.form.value;

    const obs = this.id
      ? this.service.update(payload as any)
      : this.service.create(payload as any);

    obs.subscribe(() => {

      this.snackBar.open(
        this.id ? 'Expense updated successfully' : 'Expense created successfully',
        'Close',
        { duration: 3000 }
      );

      this.router.navigate(['/expenses']);

    });
  }

  cancel() {
    this.router.navigate(['/expenses']);
  }
}