import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, map, of, switchMap, tap } from 'rxjs';
import { AuthActions } from './auth.actions';
import { AuthService } from '../../core/services/auth.service';

@Injectable()
export class AuthEffects {
  private actions$ = inject(Actions);
  private authService = inject(AuthService);
  private router = inject(Router);

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      switchMap(({ request }) =>
        this.authService.login(request).pipe(
          map((response) => AuthActions.loginSuccess({ response })),
          catchError((error) =>
            of(
              AuthActions.loginFailure({
                error: this.getErrorMessage(error, 'Login failed'),
              }),
            ),
          ),
        ),
      ),
    ),
  );

  register$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.register),
      switchMap(({ request }) =>
        this.authService.register(request).pipe(
          map((response) => AuthActions.registerSuccess({ response })),
          catchError((error) =>
            of(
              AuthActions.registerFailure({
                error: this.getErrorMessage(error, 'Registration failed'),
              }),
            ),
          ),
        ),
      ),
    ),
  );

  authSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess, AuthActions.registerSuccess),
        tap(({ response }) => {
          localStorage.setItem('accessToken', response.accessToken);
          localStorage.setItem('refreshToken', response.refreshToken);

          this.router.navigate(['/feed']);
        }),
      ),
    { dispatch: false },
  );

  loadMe$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.loadMe),
      switchMap(() =>
        this.authService.getMe().pipe(
          map((user) => AuthActions.loadMeSuccess({ user })),
          catchError((error) =>
            of(
              AuthActions.loadMeFailure({
                error: error?.error?.message ?? 'Cannot load current user',
              }),
            ),
          ),
        ),
      ),
    ),
  );

  logout$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.logout),
        tap(() => {
          localStorage.removeItem('accessToken');
          localStorage.removeItem('refreshToken');

          this.router.navigate(['/signin']);
        }),
      ),
    { dispatch: false },
  );

  private getErrorMessage(error: unknown, fallback: string): string {
    const httpError = error as { error?: { message?: string } | string };

    if (typeof httpError?.error === 'string') {
      return httpError.error;
    }

    return httpError?.error?.message ?? fallback;
  }
}
