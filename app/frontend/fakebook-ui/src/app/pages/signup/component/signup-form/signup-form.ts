import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Store } from '@ngrx/store';
import { AuthActions } from '../../../../state/auth/auth.actions';
import { selectAuthError, selectAuthLoading } from '../../../../state/auth/auth.reducer';

@Component({
  selector: 'app-signup-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './signup-form.html',
  styleUrl: './signup-form.scss',
})
export class SignupForm {
  hidePassword = true;
  form: FormGroup;

  loading$;
  error$;

  constructor(
    private fb: FormBuilder,
    private store: Store,
  ) {
    this.form = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(100)]],
      lastName: ['', [Validators.required, Validators.maxLength(100)]],
      username: ['', [Validators.required, Validators.maxLength(64)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
    });

    this.loading$ = this.store.select(selectAuthLoading);
    this.error$ = this.store.select(selectAuthError);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { firstName, lastName, email, username, password } = this.form.getRawValue();

    this.store.dispatch(
      AuthActions.register({
        request: {
          firstName,
          lastName,
          email,
          userName: username,
          password,
        },
      }),
    );
  }
}
