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
                error: error?.error?.message ?? 'Login failed',
              }),
            ),
          ),
        ),
      ),
    ),
  );

  loginSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess),
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
}
