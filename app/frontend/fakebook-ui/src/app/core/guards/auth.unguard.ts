import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { filter, map, switchMap, take, tap } from 'rxjs';

import { AuthActions } from '../../state/auth/auth.actions';
import { selectAuthInitialized, selectIsAuthenticated } from '../../state/auth/auth.reducer';

export const unauthGuard: CanActivateFn = () => {
  const store = inject(Store);
  const router = inject(Router);

  return store.select(selectAuthInitialized).pipe(
    tap((initialized) => {
      if (!initialized) {
        store.dispatch(AuthActions.restoreAuth());
      }
    }),
    filter((initialized) => initialized),
    take(1),
    switchMap(() =>
      store.select(selectIsAuthenticated).pipe(
        take(1),
        map((isAuthenticated) => {
          if (isAuthenticated) {
            router.navigate(['/feed']);
            return false;
          }

          return true;
        }),
      ),
    ),
  );
};
