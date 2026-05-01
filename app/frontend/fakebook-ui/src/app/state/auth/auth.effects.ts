import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { catchError, exhaustMap, map, of, switchMap, tap } from 'rxjs';
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

  restoreAuth$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.restoreAuth),
      exhaustMap(() => {
        const accessToken = localStorage.getItem('accessToken');
        const refreshToken = localStorage.getItem('refreshToken');

        if (!accessToken) {
          return of(AuthActions.restoreAuthFailure());
        }

        return this.authService.getMe().pipe(
          map((user) =>
            AuthActions.restoreAuthSuccess({
              accessToken,
              refreshToken,
              user,
            }),
          ),
          catchError(() => {
            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');

            return of(AuthActions.restoreAuthFailure());
          }),
        );
      }),
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
