import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExpenseService } from '../../services/expense';
import { Expense } from '../../models/expense';

@Component({
  selector: 'app-expense-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './expense-form.html',
  styleUrls: ['./expense-form.css']
})
export class ExpenseFormComponent implements OnInit {
  form!: FormGroup;
  id?: number;
  loading = false;
  error = '';

  constructor(
    private fb: FormBuilder,
    private service: ExpenseService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Initialize form after DI
    this.form = this.fb.group({
      id: [null],
      title: ['', Validators.required],
      amt: [0, [Validators.required, Validators.min(0.01)]],
      category: [''],
      date: ['', Validators.required]
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = +idParam;
      this.load(this.id);
    }
  }

  private load(id: number): void {
    this.loading = true;
    this.service.getById(id).subscribe({
      next: (exp) => {
        // patch safely with fallbacks so types align
        this.form.patchValue({
          id: exp.id ?? null,
          title: exp.title ?? '',
          amt: exp.amt ?? 0,
          category: exp.category ?? '',
          date: exp.date ? exp.date.split('T')[0] : ''
        });
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.message ?? 'Failed to load expense';
        this.loading = false;
      }
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const fv = this.form.value;
    const payload: Expense = {
      id: fv.id ?? undefined,
      title: fv.title as string,
      amt: fv.amt as number,
      category: fv.category ?? null,
      date: fv.date as string
    };

    this.loading = true;
    const obs = this.id ? this.service.update(payload) : this.service.create(payload);
    obs.subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/expenses']);
      },
      error: (err) => {
        this.error = err?.message ?? (this.id ? 'Update failed' : 'Create failed');
        this.loading = false;
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/expenses']);
  }
}
