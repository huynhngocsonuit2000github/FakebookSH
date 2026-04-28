import { AuthState } from './../../../../state/auth/auth.models';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterModule } from '@angular/router';
import { AuthActions } from '../../../../state/auth/auth.actions';
import { Store } from '@ngrx/store';
import {
  selectAuthError,
  selectAuthLoading,
  selectAuthState,
} from '../../../../state/auth/auth.reducer';

@Component({
  selector: 'app-signin-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    RouterModule,
  ],
  templateUrl: './signin-form.html',
  styleUrl: './signin-form.scss',
})
export class SigninForm {
  hidePassword = true;
  form: FormGroup;
  loading$;
  error$;

  constructor(
    private fb: FormBuilder,
    private store: Store,
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
    this.loading$ = this.store.select(selectAuthLoading);
    this.error$ = this.store.select(selectAuthError);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const { email, password } = this.form.value;

    this.store.dispatch(
      AuthActions.login({
        request: {
          emailOrUserName: email,
          password,
        },
      }),
    );
  }
}
